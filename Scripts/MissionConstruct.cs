using System;
using System.Collections.Generic;
using UnityEngine;
/*
    you can choose several mission targets in edit mode, but wincontroller worked wit only one target with high priority (highest priority -0)
 */
namespace Mkey
{
    public enum LevelType { LoopTopRowLevel, TimeLevel, AnchorLevel, FishLevel }
    [Serializable]
    public class MissionConstruct
    {
        [SerializeField]
        private string description = "Mission";
        public string Description { get { return description; } }

        // time level
        [Space(8)]
        [SerializeField]
        private int timeConstrain = 0;
        public int TimeConstrain { get { return timeConstrain; } }  // priority 0 - remove all bubbles from board

        // loop top row
        [Space(8)]
        [SerializeField]
        private bool loopTopRow = true;
        public bool LoopTopRow { get { return loopTopRow; } }       // priority 1 - default (if not selected)
        //  [SerializeField]
        private int bubblesCount = 6; // default 6
        public int  BubblesCount { get { return bubblesCount; } }   // bubbles collect from top row (all default)

        // raise anchor
        [Space(8)]
        [SerializeField]
        private bool raiseAnchor = false;
        public bool RaiseAnchor { get { return raiseAnchor; } }     // priority 2  

        // collect targets, like save fish
        [SerializeField]
        private ObjectSetCollection targets;
        public ObjectSetCollection Targets { get { return targets; } } // priority 3

        [SerializeField]
        private int movesConstrain = 0;
        public int MovesConstrain { get { return movesConstrain; } }

        public Action ChangeMovesCountEvent;
        public Action ChangeTimeEvent;
        public Action ChangeDescriptionEvent;
        public Action ChangeLoopTopRowEvent;
        public Action ChangeRaiseAnchorEvent;
        public Action SaveEvent; // need to save object data, used for constructor
        public Action ChangeObjectTargetsEvent;
        public Action ChangeTopRowBubblesCountEvent;

        public MissionConstruct()
        {
            targets = new ObjectSetCollection();
        }

        public LevelType GetLevelType()
        {
            LevelType levelType = LevelType.LoopTopRowLevel; // default
            if (timeConstrain > 0) levelType = LevelType.TimeLevel; // high priority
            else if (raiseAnchor) levelType = LevelType.AnchorLevel; 
            else if (targets.Count >0) levelType = LevelType.FishLevel;
            return levelType;
        }

        #region movesConstartin
        /// <summary>
        /// Add moves
        /// </summary>
        /// <param name="count"></param>
        public void AddMoves(int count)
        {
            int tCount = Mathf.Max(0, movesConstrain + count);
            bool changed = (movesConstrain != tCount);
            movesConstrain = tCount;
            if (changed)
            {
                OnChangeMovesCountEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Set lifes gift count
        /// </summary>
        /// <param name="count"></param>
        public void SetMovesCount(int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (movesConstrain != count);
            movesConstrain = count;
            if (changed)
            {
                OnChangeMovesCountEvent();
                OnSaveEvent();
            }
        }
        #endregion movesConstrain

        #region timeConstrtain
        /// <summary>
        /// Add time to timeConstrain
        /// </summary>
        /// <param name="seconds"></param>
        public void AddTime(int seconds)
        {
            int tCount = Mathf.Max(0, timeConstrain + seconds);
            bool changed = (timeConstrain != tCount);
            timeConstrain = tCount;
            if (changed)
            {
                OnChangeTimeEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Set time constrain
        /// </summary>
        /// <param name="seconds"></param>
        public void SetTime(int seconds)
        {
            seconds = Mathf.Max(0, seconds);
            bool changed = (timeConstrain != seconds);
            timeConstrain = seconds;
            if (changed)
            {
                OnChangeTimeEvent();
                OnSaveEvent();
            }
        }
        #endregion timeConstrain

        #region loopTopRow
        /// <summary>
        /// Set  loop top row condition
        /// </summary>
        /// <param name="seconds"></param>
        public void SetLoopTopRow(bool loop)
        {
            bool changed = loop != loopTopRow;
            loopTopRow = loop;
            if (changed)
            {
                OnChangeLoopTopRowEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Add top row bubbles
        /// </summary>
        /// <param name="seconds"></param>
        public void AddTopRowCount(int count)
        {
            int tCount = Mathf.Max(0, bubblesCount + count);
            bool changed = (bubblesCount != tCount);
            bubblesCount = tCount;
            if (changed)
            {
                OnChangeTopRowBubblesCountEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Set top row bubbles
        /// </summary>
        /// <param name="seconds"></param>
        public void SetBubblesCount(int count)
        {
            count = Mathf.Max(1, count);
            bool changed = (bubblesCount!= count);
            bubblesCount = count;
            if (changed)
            {
                OnChangeTopRowBubblesCountEvent();
                OnSaveEvent();
            }
        }

        #endregion

        #region raiseAnchor
        /// <summary>
        /// Set  loop top row condition
        /// </summary>
        /// <param name="seconds"></param>
        public void SetRaiseAnchor(bool raise)
        {
            bool changed = (raise != raiseAnchor);
            raiseAnchor = raise;
            if (changed)
            {
                OnChangeRaiseAnchorEvent();
                OnSaveEvent();
            }
        }
        #endregion 

        #region description
        /// <summary>
        /// Set description
        /// </summary>
        /// <param name="seconds"></param>
        public void SetDescription(string description)
        {
            bool changed = this.description != description;
            this.description = (string.IsNullOrEmpty(description)) ? string.Empty : description;
            if (changed)
            {
                OnChangeDescriptionEvent();
                OnSaveEvent();
            }
        }
        #endregion lifes

        #region object targets
        /// <summary>
        /// Add target by id
        /// </summary>
        /// <param name="count"></param>
        private void AddTarget(int ID)
        {
                targets.AddById(ID,1);
                OnChangeObjectTargetsEvent();
                OnSaveEvent();
        }

        /// <summary>
        /// Add target by id
        /// </summary>
        /// <param name="count"></param>
        public void AddTarget(int ID, int count)
        {
            bool changed = (count != 0);
            if (changed)
            {
                for (int i = 0; i < count; i++)
                {
                    targets.AddById(ID, count);
                }
                OnChangeObjectTargetsEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Remove booster gift, count
        /// </summary>
        /// <param name="count"></param>
        public void RemoveTarget(int ID, int count)
        {
            bool changed = (count != 0);
            if (changed)
            {
                targets.RemoveById(ID, count);
                OnChangeObjectTargetsEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Retun count of gift boosters by id
        /// </summary>
        /// <param name="boosterID"></param>
        /// <returns></returns>
        public int GetTargetCount(int ID)
        {
            return targets.CountByID(ID);
        }

        /// <summary>
        /// Set coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void SetTargetCount( int ID, int count)
        {
            bool changed = (targets.CountByID(ID) != count);

            if (changed)
            {
                targets.SetCountById(ID, count);
                OnChangeObjectTargetsEvent();
                OnSaveEvent();
            }
        }
        #endregion boosters

        #region raise events
        private void OnChangeMovesCountEvent()
        {
            if (ChangeMovesCountEvent != null) ChangeMovesCountEvent();
        }

        private void OnChangeTimeEvent()
        {
            if (ChangeTimeEvent != null) ChangeTimeEvent();
        }

        private void OnChangeDescriptionEvent()
        {
            if (ChangeDescriptionEvent != null) ChangeDescriptionEvent();
        }

        private void OnSaveEvent()
        {
            if (SaveEvent != null) SaveEvent();
        }

        private void OnChangeObjectTargetsEvent()
        {
            if (ChangeObjectTargetsEvent != null) ChangeObjectTargetsEvent();
        }

        private void OnChangeLoopTopRowEvent()
        {
            if (ChangeLoopTopRowEvent != null) ChangeLoopTopRowEvent();
        }

        private void OnChangeRaiseAnchorEvent()
        {
            if (ChangeRaiseAnchorEvent != null) ChangeRaiseAnchorEvent();
        }

        private void OnChangeTopRowBubblesCountEvent()
        {
            if (ChangeTopRowBubblesCountEvent != null) ChangeTopRowBubblesCountEvent();
        }
        #endregion raise events
    }
}
