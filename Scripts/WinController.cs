using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Mkey
{
    public enum GameResult { Win, Loose, None}
    public class WinController
    {
        private int movesRest;
        private int timeRest;  //in seconds
        
        //constructData
        private int timeCostrain;
        private int movesCostrain;
        private bool loopTopRow;
        public int TopRowBubblesCountToCollect { get; private set; }
        private int topRowBubblesCountAtStart;
        private int topRowBubblesCountAtCheckTime;
        private bool raiseAnchor;
        private ObjectSetCollection targets;
        private Dictionary<int, int> onBoardTargetsAtStart;         // from board at start - <id, count>
        private Dictionary<int, int> onBoardTargetsAtCheckTime;     // from board at check time - <id, count>
        private Dictionary<int, int> targetsToCollect;              // from targets - <id, count>

        private PFAnchor anchor;
        public SessionTimer Timer { get; private set; }
        private GameObject goTimer;
        public bool UseTimer { get; private set; }
        private Action LevelWinEvent;
        private Action LevelLooseEvent;
        private Action CheckTargetResultEvent;//for current target
        public GameResult GameResult { get; private set; }
        private bool useObjecTargets = false;
        private BubbleGrid bGrid;
        public LevelType GameLevelType { get; private set; }
        private int targetID; // use only one target fish
        private  int counterTweenID;
        private bool timeLeftShowed = false;

        public WinController(MissionConstruct mc, GameBoard gB, BubblesShooter bubblesShooter, Action LevelWin, Action LevelLoose, Action CheckTargetResult, TextMesh outTimerText, TextMesh outMovesText)
        {
            bGrid = gB.grid;
            anchor = gB.anchor;
            LevelWinEvent += LevelWin;
            LevelLooseEvent += LevelLoose;
            CheckTargetResultEvent += CheckTargetResult;

            timeCostrain =  mc.TimeConstrain;
            movesCostrain = mc.MovesConstrain;
            movesRest = movesCostrain;
            loopTopRow =  mc.LoopTopRow;
            TopRowBubblesCountToCollect = Mathf.Min(mc.BubblesCount, bGrid.TopObjectRow.GetNotEmptyCells().Count); // default 6 bubbles in mission construct
           // Debug.Log( "loop: " + mc.BubblesCount + " : " + bGrid.TopObjectRow.GetNotEmptyCells().Count);
            raiseAnchor = mc.RaiseAnchor;
            targets =  mc.Targets;
            GameLevelType = mc.GetLevelType();

            topRowBubblesCountAtStart = bGrid.TopObjectRow.GetNotEmptyCells().Count;

            targetsToCollect = new Dictionary<int, int>();
            onBoardTargetsAtStart = new Dictionary<int, int>();
            onBoardTargetsAtCheckTime = onBoardTargetsAtStart;

            useObjecTargets = (targets != null && targets.Count > 0);


            if (useObjecTargets)
            {
                foreach (var item in targets.ObjectsList) // use only first target
                {
                    targetID = item.ID;
                    int id = item.ID;
                    int count = item.Count;
                    int bCount = bGrid.GetObjectsCountByID(id);
                    Debug.Log("target: " + id + " : " + Mathf.Min(count, bCount));
                    if (!targetsToCollect.ContainsKey(id) && Mathf.Min(count, bCount) > 0)
                    {
                        targetsToCollect.Add(id, Mathf.Min(count, bCount));
                        onBoardTargetsAtStart.Add(id, Mathf.Min(count, bCount));
                        Debug.Log(" add target: " + id + " : " + Mathf.Min(count, bCount));
                    }

                    break; // use only first target
                }
            
            }

            GameResult = GameResult.None;
            if (timeCostrain>0)
            {
                Timer = new SessionTimer(timeCostrain);
                //Timer.Start();

                UseTimer = true;
                Timer.OnTickRestSeconds = (sec) => { timeRest = (int)sec; if (timeRest <= 30 && !timeLeftShowed) { timeLeftShowed = true; GuiController.Instance.ShowMessageTimeLeft("Warning!", "30 seconds left", 1); }  };
                Timer.OnTimePassed = () => { if (GameResult == GameResult.None) { CheckResult(false, false); } };

                if (outMovesText) outMovesText.gameObject.SetActive(false);

                if (outTimerText)
                {
                    if (outTimerText) outTimerText.gameObject.SetActive(true);
                    Timer.OnTickRestSeconds += (sec) => { outTimerText.text = sec.ToString(); };
                }
            }
            else
            {
                if (outTimerText)
                {
                    if (outTimerText) outTimerText.gameObject.SetActive(false);
                }
                if (outMovesText)
                {
                    outMovesText.gameObject.SetActive(true);
                    outMovesText.text = movesRest.ToString();
                }
                bubblesShooter.ShootEvent += () =>
                {
                    if (!UseTimer)
                    {
                        movesRest--;
                        if (outMovesText)
                        {
                            outMovesText.text = movesRest.ToString();
                            if (movesRest == 5)
                             counterTweenID  =   SimpleTween.Value(outMovesText.gameObject, 1.0f, 0.5f, 0.5f).SetEase(EaseAnim.EaseLinear).
                                  SetOnUpdate((float val) => {if( this!=null && outMovesText) outMovesText.color = new Color(1, val, val, 1); else SimpleTween.Cancel(counterTweenID, false); }).SetCycled().ID;
                            if (movesRest == 0) SimpleTween.Cancel(outMovesText.gameObject, false);
                        }
                    }
                };
            }

            Debug.Log("GameLevelType : " + GameLevelType);
            CheckTopRow();
            CheckTargetsOnBoard();
        }

        /// <summary>
        /// Check shoot result after all shoot changes (shootarea collect, fall down collect, update path)
        /// </summary>
        /// <param name="completeCallBack"></param>
        public void CheckResult(bool raiseLooseEvent, bool raiseWinEvent )
        {
            GameResult = GameResult.None;
            CheckTargetsOnBoard();
            CheckTopRow();
           // Debug.Log("resultr (targ, anchor, toprow)" + ObjectTargetsIsCollected() + " : " + AnchorIsRaised() + " : " + TopRowIsCleaned());
            
            if (CheckTargetResultEvent != null)
            {
                CheckTargetResultEvent();
            }

            if( ObjectTargetsIsCollected() && AnchorIsRaised() && TopRowIsCleaned())
            {
                GameResult = GameResult.Win;
                if (LevelWinEvent != null && raiseWinEvent) LevelWinEvent();
                return;
            }

            if((UseTimer && Timer.IsTimePassed) || (!UseTimer && movesRest <= 0))
            {
                GameResult = GameResult.Loose;
                if (LevelLooseEvent != null && raiseLooseEvent) LevelLooseEvent();
                return;
            }
        }

        private bool ObjectTargetsIsCollected()
        {
            if (GameLevelType!= LevelType.FishLevel) return true; // time level is high priority
            foreach (var item in targetsToCollect)
            {
                if(onBoardTargetsAtStart.ContainsKey(item.Key) && onBoardTargetsAtCheckTime.ContainsKey(item.Key))
                {
                    int collectCount = onBoardTargetsAtStart[item.Key] - onBoardTargetsAtCheckTime[item.Key];
                    if (collectCount < item.Value) return false;
                }
            }
            return true; 
        }

        private bool AnchorIsRaised()
        {
            if (GameLevelType != LevelType.AnchorLevel) return true;
            return anchor.HavePathToTop();
        }

        /// <summary>
        /// Return true if toprow is empty for time level. 
        /// For not time level return true if not use LoopTopRow as target.
        /// For not time level return (topRowBubblesCountAtStart - topRowBubblesCountAtCheckTime) >= topRowBubblesCountToCollect.
        /// </summary>
        /// <returns></returns>
        private bool TopRowIsCleaned()
        {
            if (GameLevelType == LevelType.TimeLevel)
            {
                return bGrid.TopObjectRow.RowIsEmpty();
            }
            else if(GameLevelType == LevelType.LoopTopRowLevel)
            {
                if (!loopTopRow) return true;
                return (topRowBubblesCountAtStart - topRowBubblesCountAtCheckTime) >= TopRowBubblesCountToCollect;
            }
            return true;
        }

        public void Update(float time)
        {
            if (UseTimer) {
                Timer.Update(time);
                //Debug.Log("updte timer");
            }
        }

        public bool HasTimeToShoot()
        {

            return(GameLevelType == LevelType.TimeLevel)? timeRest > 0 : true;
        }

        public bool HasShoots()
        {
            return (GameLevelType != LevelType.TimeLevel) ? movesRest > 0 : true;
        }

        /// <summary>
        /// Fill dictionary onBoardTargetsAtCheckTime
        /// </summary>
        private void CheckTargetsOnBoard()
        {
            onBoardTargetsAtCheckTime = new Dictionary<int, int>();
            foreach (var item in targetsToCollect)
            {
                int id = item.Key;
                int count = bGrid.GetObjectsCountByID(id);

                if (!onBoardTargetsAtCheckTime.ContainsKey(id))
                {
                    onBoardTargetsAtCheckTime.Add(id, count);
                }
            }
        }

        private void CheckTopRow()
        {
            topRowBubblesCountAtCheckTime = bGrid.TopObjectRow.GetNotEmptyCells().Count;
        }

        public int GetTargetsCountToCollect(int id)
        {
            if (targetsToCollect.ContainsKey(id) && onBoardTargetsAtStart.ContainsKey(id))
                return Mathf.Min(targetsToCollect[id], onBoardTargetsAtStart[id]);
            return 0;
        }

        public int GetTargetsCountOnBoard(int id)
        {
            if (onBoardTargetsAtCheckTime.ContainsKey(id))
                return onBoardTargetsAtCheckTime[id];
            return 0;
        }

        public void GetCurrTarget(out int collectedCount, out int toCollectCount)
        {
             collectedCount = 0;
             toCollectCount = 0;
            switch (GameLevelType)
            {
                case LevelType.LoopTopRowLevel:
                    toCollectCount = TopRowBubblesCountToCollect;
                    collectedCount = topRowBubblesCountAtStart - topRowBubblesCountAtCheckTime;
                    break;
                case LevelType.TimeLevel:
                    toCollectCount = TopRowBubblesCountToCollect;
                    collectedCount = topRowBubblesCountAtStart - topRowBubblesCountAtCheckTime;
                    break;
                case LevelType.AnchorLevel:
                    toCollectCount = 1;
                    collectedCount = (AnchorIsRaised()) ? 1 : 0;
                    break;
                case LevelType.FishLevel: // first target
                    if (onBoardTargetsAtStart.ContainsKey(targetID) && onBoardTargetsAtCheckTime.ContainsKey(targetID) && targetsToCollect.ContainsKey(targetID))
                    {
                        toCollectCount = GetTargetsCountToCollect(targetID);
                        collectedCount = onBoardTargetsAtStart[targetID] - onBoardTargetsAtCheckTime[targetID];
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public class CollectData
    {
        public int countAtStart;
        public int countAtChekTime;

        public int Collected { get { return countAtStart - countAtChekTime; } }
    }
}
