using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public enum GameMode { Play, Edit }
    public enum GameState { Shooting, DisableTouch }

    public class GameBoard : MonoBehaviour
    {
        bool debug = false;
        [SerializeField]
        private TextMesh movesCounter;
        [SerializeField]
        private TextMesh timeCounter;

        public Transform GridContainer;
        public Material animatedMaterial;
        public static GameMode gMode = GameMode.Play; // Play or Edit
        public static GameState gState = GameState.Shooting; // Play or Edit

        public PFAnchor anchor;

        public SpriteRenderer backGround;
        public Sprite BackGround
        {
            get { return backGround.sprite; }
            set { if (backGround) backGround.sprite = value; }
        }

        public GameObject gConstructor;
        public BubbleGrid grid;

        public GameConstructSet gcSet;
        public static GameObjectsSet GameObjSet
        {
            get { return LcSet.mSet; }
        }
        public static LevelConstructSet LcSet
        {
            get { return Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel); }
        }
        public static int MaxLevelsCount
        {
            get { return Instance.gcSet.LevelCount; }
        }

        public static GameBoard Instance;

        public List<Booster> allBoosters;

        public WinController WController { get; private set; }

        [SerializeField]
        private int minBubbleScore = 10;
        [SerializeField]
        private int maxBubbleScore = 40;

        public static bool showLevelMission = false;
        private bool resultShowed = false;

        #region regular
        void Awake()
        {
            if (Instance) Destroy(gameObject);
            else
            {
                Instance = this;
            }
            BubblesPlayer.Destroy(); // need to reload player data
            if (gcSet && LcSet && GameObjSet) BubblesPlayer.Instance.CreateBoosterHolder(GameObjSet); // create boosters for current  bevore start all objects - 
        }

        void Start()
        {
            if (!gcSet)
            {
                Debug.Log("Game construct set not found!!!");
                return;
            }
            
            if (!LcSet)
            {
                Debug.Log("Level construct set not found!!! - " + BubblesPlayer.CurrentLevel);
                return;
            }

            if (!GameObjSet)
            {
                Debug.Log("MatcSet not found!!! - " + BubblesPlayer.CurrentLevel);
                return;
            }

            if (gMode == GameMode.Edit)
            {
                Debug.Log("start edit mode");
#if UNITY_EDITOR

                CreateGameBoard(true);
                gConstructor.GetComponent<GameConstructor>().InitStart(gcSet);
                if (anchor) anchor.gameObject.SetActive(false);
#endif
            }

            else if (gMode == GameMode.Play)
            {
                Debug.Log("start play mode");
                CreateGameBoard(true);
                if (gConstructor) DestroyImmediate(gConstructor.gameObject);
                BubblesShooter.Instance.InitStart(minBubbleScore, maxBubbleScore);
                BubblesPlayer.Instance.SetAverageScore(grid.GetObjectCountWithScore() * (minBubbleScore + maxBubbleScore)/2);
                Debug.Log("AverageScore: " + BubblesPlayer.Instance.AverageScore);
                SoundMasterController.Instance.UpdateLevelBkgMusik();
                WController = new WinController(LcSet.levelMission, this, BubblesShooter.Instance,
                    () =>
                    {
                        
                         // BubblesPlayer.Instance.PassLevel();
                        // GuiController.Instance.ShowVictory();// next features
                    }, 
                    () => 
                    {
                        // next features
                        // BubblesPlayer.Instance.AddLifes(-1);
                        // GuiController.Instance.ShowLoss();
                    },
                    () => {if(HeaderGUIController.Instance) HeaderGUIController.Instance.RefreshTargets(); },
                    timeCounter, movesCounter);
                
                if (showLevelMission)
                {
                    GuiController.Instance.ShowMission(()=> { Debug.Log("close mission");  });
                }
                //InfiniteLifeTimer.Instance.InitStart();
                //LifeIncTimer.Instance.InitStart();
                if (HeaderGUIController.Instance) HeaderGUIController.Instance.Refresh();
               
            }

            //FooterGUIController.Instance.InitStart();
            resultShowed = false;
        }

        private void Update()
        {
            if (gMode == GameMode.Edit) return;
            if (WController == null) return;

            if (WController.GameResult == GameResult.Loose && !resultShowed)
            {
                resultShowed = true;
                BubblesPlayer.Instance.AddLifes(-1);
                GuiController.Instance.ShowLoss();
                return;
            }

            if (WController.GameResult == GameResult.Win && !resultShowed)
            {
                resultShowed = true;
                BubblesPlayer.Instance.PassLevel();
                GuiController.Instance.ShowVictory();
                return;
            }

            WController.Update(Time.time);
            if (gState == GameState.Shooting)
            {
                Shooting();
            }
            else if (gState == GameState.DisableTouch)
            {
              
            }
        }

        private void OnDestroy()
        {
           BubblesPlayer.Destroy();
           if(mainSeq!=null) mainSeq.Break();
        }

        #endregion regular

        #region states
        private void GoToShootMode()
        {
            SetControlActivity(true, true);
            gState = GameState.Shooting;
        }

        TweenSeq mainSeq;
        CellsGroup detCells;
        private void Shooting()
        {
           // BubblesShooter.Instance.ShowSwapPath();

            if (TouchManager.IsTouched && BubblesShooter.Instance.TouchInRange())
            {
                BubblesShooter.Instance.ShowShootLine();
            }
            else if (TouchManager.IsTouched && !BubblesShooter.Instance.TouchInRange())
            {
                BubblesShooter.Instance.HideShootLine();
                BubblesShooter.Instance.CleanTargets();
            }
            else if (!TouchManager.IsTouched)
            {
                BubblesShooter.Instance.HideShootLine(); //Debug.Log("Can Shoot " + BubblesShooter.CanShoot);

                if (BubblesShooter.CanShoot) // shoot
                {
                    GoToDTMode(); // diasable touch
                    mainSeq = new TweenSeq();
                    mainSeq.Add((callBack) => // move bubble to free target, hit target, and check result
                    {
                        BubblesShooter.Instance.Shoot(callBack);
                    });

                    mainSeq.Add((callBack) => // collect all bubbles in shootarea
                    {
                        BubblesShooter.Instance.ShootCollect(callBack);
                    });

                    mainSeq.Add((callBack) => //  collect all fall down objects
                    {
                        BubblesShooter.Instance.CleanTargets();
                        detCells = new CellsGroup();
                        detCells.AddRange(grid.GetDetacheCells());
                        detCells.FallDownCollect(BubblesShooter.Instance.BubbleScore, null);
                        WController.CheckResult(false, false);// check shoot result
                        grid.AddEmptyRow();
                       
                        callBack();
                    });

                    if (WController.GameLevelType == LevelType.AnchorLevel) // update anchor path
                        mainSeq.Add((callBack) =>
                        {
                            if (anchor && anchor.gameObject.activeSelf) anchor.UpdatePath();
                            WController.CheckResult(false, false);// check shoot result
                            callBack();
                        });

                    mainSeq.Add((callBack) =>
                    {
                        grid.MoveToVisible(() =>
                        {
                            if (WController.GameResult == GameResult.Win)
                            {
                                detCells = new CellsGroup();
                                detCells.AddRange(grid.GetNotEmptyCells());
                                detCells.FallDownCollect(BubblesShooter.Instance.BubbleScore, null);
                            }
                            WController.CheckResult(false, false);// check shoot result after falldown
                            GoToShootMode();
                            callBack();
                        });
                    });

                    mainSeq.Start();
                }
            }
        }

        private void GoToDTMode()
        {
            SetControlActivity(false, false);
            gState = GameState.DisableTouch;
        }
        #endregion states

        internal void SetControlActivity(bool activityGrid, bool activityMenu)
        {
            TouchManager.SetTouchActivity(activityGrid);
           if(HeaderGUIController.Instance) HeaderGUIController.Instance.SetControlActivity(activityMenu);
           if(FooterGUIController.Instance) FooterGUIController.Instance.SetControlActivity(activityMenu);
        }

        /// <summary>
        /// Create gameboard and move to bottom visible row
        /// </summary>
        /// <param name="move"></param>
        internal void CreateGameBoard(bool move)
        {
            Debug.Log("level set: " + LcSet.name);
            Debug.Log("curr level: " + BubblesPlayer.CurrentLevel);
            BackGround = LcSet.BackGround;
            int vertSize = (LcSet.vertSize > 0) ? LcSet.vertSize : 10;
            int horSize = (LcSet.horSize > 0) ? LcSet.horSize : 10;

            List<CellData> mainObjectsData = LcSet.featuredCells;
            List<CellData> overObjectsData = LcSet.overlays;

            float scale = LcSet.Scale;
            if (gMode == GameMode.Play)
            {
                if (grid != null) grid.DestroyGrid();
                grid = new BubbleGrid(LcSet, GridContainer, SortingOrder.Base, gMode);
                grid.AddEmptyRow();
            }
            else
            {
                if (grid != null)
                {
                    grid.Rebuild(LcSet, SortingOrder.Base, gMode);
                }
                else
                {
                    grid = new BubbleGrid(LcSet, GridContainer, SortingOrder.Base, gMode);
                }
#if UNITY_EDITOR
                // set cells delegates for constructor
                for (int i = 0; i < grid.Cells.Count; i++)
                {
                    grid.Cells[i].PointerDownEvent = (c) =>
                    {
                        if (c.Row < LcSet.VertSize) // don't using reserved rows
                            gConstructor.GetComponent<GameConstructor>().Cell_Click(c);
                    };

                    grid.Cells[i].DragEnterEvent = (c) =>
                    {
                        if (c.Row < LcSet.VertSize) // don't using reserved rows
                            gConstructor.GetComponent<GameConstructor>().Cell_Click(c);
                    };
                }
#endif
            }
            if (move)
                grid.MoveToVisible(() =>
                {
                    if (gMode == GameMode.Play)
                    {
                        if (anchor)
                        {
                            if (WController.GameLevelType == LevelType.AnchorLevel)
                            {
                                anchor.gameObject.SetActive(true);
                                anchor.InitStart(grid, grid.GetBottomRow()[5]);
                            }
                            else
                            {
                                anchor.gameObject.SetActive(false);
                            }
                        }
                        if (WController.GameLevelType == LevelType.TimeLevel)
                        {
                            WController.Timer.Start();
                        }
                    }
                });
        }
    }

    public class MatchGroup : CellsGroup
    {
        /// <summary>
        /// Return true if both groups has minimum one equal cell
        /// </summary>
        /// <param name="mGroup"></param>
        /// <returns></returns>
        public bool IsIntersectWithGroup(MatchGroup mGroup)
        {
            if (mGroup == null || mGroup.Length == 0) return false;
            for (int i = 0; i < cells.Count; i++)
            {
                if (mGroup.Contain(cells[i])) return true;
            }
            return false;
        }

        /// <summary>
        /// Merge two groups
        /// </summary>
        /// <param name="mGroup"></param>
        public void Merge(MatchGroup mGroup)
        {
            if (mGroup == null || mGroup.Length == 0) return;
            for (int i = 0; i < mGroup.cells.Count; i++)
            {
                Add(mGroup.cells[i]);
            }
        }

        /// <summary>
        /// Return true if groups contain all equal cells
        /// </summary>
        /// <param name="mGroup"></param>
        /// <returns></returns>
        public bool IsEqual(MatchGroup mGroup)
        {
            if (Length != mGroup.Length) return false;
            foreach (GridCell c in cells)
            {
                if (!mGroup.Contain(c)) return false;
            }
            return true;
        }

    }

    public class CellsGroup
    {
        public List<GridCell> cells;
        public GridCell lastAddedCell;

        public bool Contain(GridCell mCell)
        {
            return cells.Contains(mCell);
        }

        public int Length
        {
            get { return cells.Count; }
        }

        public CellsGroup()
        {
            cells = new List<GridCell>();
        }

        public void Add(GridCell mCell)
        {
            if (mCell == null) return;
            if (!cells.Contains(mCell))
            {
                cells.Add(mCell);
                lastAddedCell = mCell;
            }
        }

        public void AddRange(List<GridCell> mCells)
        {
            if (mCells == null) return;
            for (int i = 0; i < mCells.Count; i++)
            {
                Add(mCells[i]);
            }
        }

        public void CancelTween()
        {
            cells.ForEach((c) => { c.CancelTween(); });
        }

        public override string  ToString()
        {
            string s = "";
            cells.ForEach((c) => { s += c.ToString(); });
            return s;
        }

        /// <summary>
        /// async collect matched objects in a group
        /// </summary>
        /// <param name="completeCallBack"></param>
        internal void ShootCollect(int privateScore, bool sequenced, Action completeCallBack)
        {
            if (sequenced)
            {
                TweenSeq collectTween = new TweenSeq();
                foreach (GridCell c in cells)
                {
                    collectTween.Add((callBack) => { c.CollectShootAreaObject(callBack, true, false, true, privateScore); });
                }
                collectTween.Add((callBack) => { if (completeCallBack != null) completeCallBack(); callBack(); });
                collectTween.Start();
            }
            else
            {
                ParallelTween collectTween = new ParallelTween();
                foreach (GridCell c in cells)
                {
                    collectTween.Add((callBack) => { c.CollectShootAreaObject(callBack, true, false, true, privateScore); });
                }
                collectTween.Start(completeCallBack);
            }
        }

        /// <summary>
        /// async collect falled down objects in a group
        /// </summary>
        /// <param name="completeCallBack"></param>
        internal void FallDownCollect(int privateScore, Action completeCallBack)
        {
            if (cells.Count > 0)
            {
                ParallelTween collectTween = new ParallelTween();
                foreach (GridCell c in cells)
                {
                    collectTween.Add((callBack) =>
                    {
                        c.CollectFalledDown(callBack, true, true, privateScore);
                    });
                }
                collectTween.Start(completeCallBack);
            }
            else
            {
                if (completeCallBack != null) completeCallBack();
            }
        }

        internal void Remove(GridCell mCell)
        {
            if (mCell == null) return;
            if (Contain(mCell))
            {
                cells.Remove(mCell);
            }
        }

        internal void Remove(List<GridCell> mCells)
        {
            if (mCells == null) return;
            for (int i = 0; i < mCells.Count; i++)
            {
                if (Contain(mCells[i]))
                {
                    cells.Remove(mCells[i]);
                }
            }
        }

        internal void ShowTargetSelector(bool select, Sprite selector)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].ShowTargetSelector(select, selector);
            }
        }

        internal void ShowTargetSelector(bool select)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].ShowTargetSelector(select, GameBoard.GameObjSet.selector);
            }
        }
    }

    public class Row<T> : GridCellArray<T> where T : GridCell
    {
        public Row(int size) : base(size) { }

        public MainObject GetLeftMOFromCell(int index)
        {
            GridCell mc = GetLowerCell(index);
            MainObject mo = (mc) ? mc.Mainobject : null;
            return mo;
        }

        public MainObject GetRightMOFromCell(int index)
        {
            GridCell mc = GetHigherCell(index);
            MainObject mo = (mc) ? mc.Mainobject : null;
            return mo;
        }

        public bool RowIsEmpty()
        {
            for (int i = 0; i < Length; i++)
            {
                if (cells[i].Mainobject) return false;
            }
            return true;
        }

        /// <summary>
        /// Flag, thet set by grid. True if is top row for grid objects
        /// Grid can have aditional rows from top for Anchor path
        /// </summary>
        public bool isTopObjectRow = false;

        public bool IsSeviceRow = false;
    }

    public class GridCellArray<T> : GenInd<T> where T : GridCell
    {
        public GridCellArray(int size) : base(size) { }

        /// <summary>
        /// Return true if All objects IDs are equal
        /// </summary>
        /// <param name="mcs"></param>
        /// <returns></returns>
        public static bool AllMainObjectsIsEqual(GridCell[] mcs)
        {
            if (mcs == null || !mcs[0] || mcs.Length < 2) return false;
            for (int i = 1; i < mcs.Length; i++)
            {
                if (!mcs[i]) return false;
                if (!mcs[0].IsMainObjectEquals(mcs[i])) return false;
            }
            return true;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < cells.Length; i++)
            {
                s += cells[i].ToString();
            }
            return s;
        }

        public GridCell GetMinUsed()
        {
            for (int i = 0; i < Length; i++)
            {
                // if (cells[i].IsUsed) return cells[i];
            }
            return null;
        }

        public GridCell GetMaxUsed()
        {
            for (int i = Length - 1; i >= 0; i--)
            {
                //  if (cells[i].IsUsed) return cells[i];
            }
            return null;
        }

        public Vector3 GetCenterUsed()
        {
            GridCell min = GetMinUsed();
            GridCell max = GetMaxUsed();
            return (max.transform.position + min.transform.position) / 2.0f;
        }

        /// <summary>
        /// Max used - min used
        /// </summary>
        /// <returns></returns>
        public Vector2 GetUsedLength()
        {
            GridCell min = GetMinUsed();
            GridCell max = GetMaxUsed();
            return new Vector2(max.Column - min.Column, max.Row - min.Row);
        }

        public List<T> GetNotEmptyCells()
        {
            List<T> res = new List<T>();
            for (int i = 0; i < Length; i++)
            {
                if (!this[i].IsEmpty) res.Add(this[i]);
            }
            return res;
        }

        public List<T> GetEmptyCells()
        {
            List<T> res = new List<T>();
            for (int i = 0; i < Length; i++)
            {
                if (this[i].IsEmpty) res.Add(this[i]);
            }
            return res;
        }
    }

    public class GenInd<T> where T : class
    {
        public T[] cells;
        public int Length;
        public bool ErrFlag;

        public GenInd(int size)
        {
            cells = new T[size];
            Length = size;
        }

        public T this[int index]
        {
            get { if (ok(index)) { ErrFlag = false; return cells[index]; } else { ErrFlag = true; return null; } }
            set { if (ok(index)) { ErrFlag = false; cells[index] = value; } else { ErrFlag = true; } }
        }

        private bool ok(int index)
        {
            return (index >= 0 && index < Length);
        }

        /// <summary>
        /// return cells with higher indexes from top to bottom 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<T> GetHigherCellsTB(int index)
        {
            List<T> cs = new List<T>();
            if (ok(index))
            {
                int i = Length - 1;
                while (i > index)
                {
                    cs.Add(cells[i]);
                    i--;
                }
            }
            return cs;
        }

        /// <summary>
        /// return cells with higher indexes from bottom to top 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<T> GetHigherCellsBT(int index)
        {
            List<T> cs = new List<T>();
            if (ok(index + 1))
            {
                int i = index + 1;
                while (i < Length)
                {
                    cs.Add(cells[i]);
                    i++;
                }
            }
            return cs;
        }

        public T GetHigherCell(int index)
        {
            if (ok(index + 1))
            {
                return cells[index + 1];
            }
            return null;
        }

        public List<T> GetLowerCells(int index)
        {
            List<T> cs = new List<T>();
            if (ok(index))
            {
                int i = 0;
                while (i < index)
                {
                    cs.Add(cells[i]);
                    i++;
                }
            }
            return cs;
        }

        public T GetLowerCell(int index)
        {
            if (ok(index - 1))
            {
                return cells[index - 1];
            }
            return null;
        }

        public T GetMiddleCell()
        {
            int number = Length / 2;

            return cells[number];
        }

    }

    public class DataState
    {
        public int target0Count;
        public int target1Count;
        public int target2Count;

        public int score;

        public int moviesCount;
        int[] boostersCount;

        public DataState(GameBoard matchBoard)
        {
            // target0Count = matchBoard.target0.currCount;
            // target1Count = matchBoard.target1.currCount;
            //  target2Count = matchBoard.target2.currCount;

            //  score = matchBoard.Score;
            //  moviesCount = MatchBoard.MoviesCount;
            //  boostersCount = new int[matchBoard.allBoosters.Count];
            // for (int i = 0; i < matchBoard.allBoosters.Count; i++)
            //  {
            //    boostersCount[i] = matchBoard.allBoosters[i].Count;
            // }
        }

        public void RestoreState(GameBoard matchBoard)
        {
            //   matchBoard.target0.currCount = target0Count;
            //  matchBoard.target1.currCount = target1Count;
            //  matchBoard.target2.currCount = target2Count;

            //  matchBoard.Score = score;
            //   MatchBoard.MoviesCount = moviesCount;
            //   for (int i = 0; i < matchBoard.allBoosters.Count; i++)
            //  {
            //     matchBoard.allBoosters[i].Count = boostersCount[i];
            //  }
        }
    }

    #region boosters
    public class Booster
    {
        public BoosterObjectData bData;
        public static Booster ActiveBooster { get; private set; }
        public Action  FooterClickEvent;          
        public Action  ChangeCountEvent;
        public Action <Booster> ActivateEvent;
        public Action <Booster> DeActivateEvent;
        private string saveKey; // = savePrefix + "booster" + bData.ID.ToString();
        private bool saveData;

        #region properties
        /// <summary>
        /// Show in footer or no
        /// </summary>
        public bool Use
        {
            get; set;
        }

        public int Count
        {
            get; private set;
        }

        /// <summary>
        /// return true if this booster == activebooster
        /// </summary>
        public bool IsActive
        {
            get { return this == ActiveBooster; }
        } 
        #endregion properties

        /// <summary>
        /// Create new booster object and set saved count (if (saveData ==true))
        /// </summary>
        /// <param name="bData"></param>
        /// <param name="saveData"></param>
        public Booster(BoosterObjectData bData, bool saveData, string savePrefix)
        {
            this.bData = bData;
            this.saveData = saveData;

            Count = 0;
            if (string.IsNullOrEmpty(savePrefix)) savePrefix = "";

            saveKey = savePrefix + "booster" + bData.ID.ToString();
            if (saveData)
            {
                if (!PlayerPrefs.HasKey(saveKey) || PlayerPrefs.GetInt(saveKey) < 0)
                {
                    PlayerPrefs.SetInt(saveKey, 0);
                }
                Count = PlayerPrefs.GetInt(saveKey);
            }
            Debug.Log("Create - " + ToString());

            Use = true;
        }

        /// <summary>
        /// Return field image
        /// </summary>
        /// <returns></returns>
        public Sprite GetImage()
        {
            return bData.ObjectImage;
        }

        /// <summary>
        /// Retun gui sprite
        /// </summary>
        /// <returns></returns>
        public Sprite GetGUIImage()
        {
            return bData.GuiImage;
        }

        /// <summary>
        /// Retun gui hover sprite
        /// </summary>
        /// <returns></returns>
        public Sprite GetGUIHoverImage()
        {
            return bData.GuiImageHover;
        }

        #region change count
        /// <summary>
        /// Increase count and refresh gui
        /// </summary>
        public void IncCount()
        {
            Count++;
            Save();
            OnChangeCountEvent();
        }

        /// <summary>
        /// Increase count and refresh gui
        /// </summary>
        public void AddCount(int count)
        {
            Count = Mathf.Max(0, Count + count);
            Save();
            OnChangeCountEvent();
        }

        /// <summary>
        /// Decrease count and refresh gui
        /// </summary>
        public void DecCount()
        {
            Count = Mathf.Max(0, --Count);
            Save();
            OnChangeCountEvent();
        }
        #endregion change count

        public override string ToString()
        {
            return (bData.Name + " : " + Count.ToString() + " items" );
        }

        /// <summary>
        /// Use PlayerPrefs to save count.
        /// </summary>
        private void Save()
        {
            if (saveData)
            {
                PlayerPrefs.SetInt(saveKey, Count);
            }
        }

        #region raise events
        public void OnFooterClickEvent()
        {
            if (FooterClickEvent != null) FooterClickEvent();
        }

        private void OnChangeCountEvent()
        {
            if (ChangeCountEvent != null) ChangeCountEvent();
        }

        private void OnActivateEvent()
        {
            if (ActivateEvent != null)
            {
                Debug.Log(ToString() + " Raise activate event   ");
                ActivateEvent(this);
            }
        }

        private void OnDeActivateEvent()
        {
            if (DeActivateEvent != null)
            {
                Debug.Log(ToString() + " Raise deactivate event   ");
                DeActivateEvent(this);
            }
        }

        #endregion raise events

        #region handlers
        public static void ShootEventHandler()
        {
            if (ActiveBooster != null)
            {
                ActiveBooster.DecCount();
                ActiveBooster = null;
            }
        }

        private void FooterClickEventHandler()
        {
            if (!IsActive && Count > 0)     // activate booster
            {
                ActivateBooster();
            }
            else if (IsActive)              // deactivate booster
            {
                DeActivateBooster();
            }
            else if (!IsActive && Count == 0 && ActiveBooster != null) // open shop  
            {
                ActiveBooster.DeActivateBooster();
            }
        }
        #endregion handlers

        public void ActivateBooster()
        {
            ActiveBooster = this;
            Debug.Log(ToString() + " activated   ");
            OnActivateEvent();
        }

        /// <summary>
        /// Set  ActiveBooster = null, raise DeActivateEvent
        /// </summary>
        public void DeActivateBooster()
        {
            ActiveBooster = null;
            Debug.Log(ToString() + " deactivated ");
            OnDeActivateEvent();
        }

        /// <summary>
        /// Create new footer booster instance 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefab"></param>
        /// <param name="boost"></param>
        /// <param name="GotoShopHandler"></param>
        /// <returns></returns>
        public FooterBoosterHelper CreateFooterBooster(RectTransform parent,  GameObject prefab, Action GotoShopHandler)
        {
            GameObject footerBooster = UnityEngine.Object.Instantiate(prefab);
            footerBooster.transform.localScale = parent.transform.lossyScale;
            footerBooster.transform.SetParent(parent);
            FooterBoosterHelper footerBoosterHelper = footerBooster.GetComponent<FooterBoosterHelper>();
            footerBoosterHelper.boosterImage.sprite = bData.GuiImage;

            // add footer click handlers
            FooterClickEvent = () => { if (GotoShopHandler != null && Count == 0) GotoShopHandler(); };
            FooterClickEvent += FooterClickEventHandler;
            footerBoosterHelper.boosterButton.onClick.AddListener(OnFooterClickEvent);

            footerBoosterHelper.booster = this;
            return footerBoosterHelper;
        }
    }

    public class BoostersHolder
    {
        private List<Booster> boosters;
        private bool saveData;
        private string savePrefix;

        #region properties
        public IList<Booster> Boosters { get { return boosters.AsReadOnly(); } }
        #endregion properties

        public BoostersHolder(GameObjectsSet goSet, bool saveData, string savePrefix)
        {
            this.saveData = saveData;
            this.savePrefix = savePrefix;

            IList<BoosterObjectData> bDataList = goSet.BoosterObjects;
            boosters = new List<Booster>();
            foreach (var item in bDataList)
            {
                boosters.Add(new Booster(item, saveData, savePrefix));
            }
            Debug.Log("Boosters count: " + boosters.Count);
        }

        internal Booster GetBoosterById(int id)
        {
            foreach (var item in boosters)
            {
                if (item.bData.ID == id) return item;
            }
            return null;
        }
    }
    #endregion boosters
    
}
