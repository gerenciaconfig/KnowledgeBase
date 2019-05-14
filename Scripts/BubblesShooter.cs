using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey {
    /// <summary>
    /// FromBoardAtStart - use bubbles IDs on board at level start to get random shoot Bubble,
    /// FromCurrentBoard -  use bubbles IDs on board at current time to get random shoot Bubble (it help player)
    /// </summary>
    public enum GetShootBubbleMode { FromBoardAtStart, FromCurrentBoard}
    public class BubblesShooter : MonoBehaviour
    {
        #region debug
        [Space(8, order = 0)]
        [Header("Debug ", order = 1)]
        [SerializeField]
        private bool debug = false;
        [SerializeField]
        private bool showDebugLines = false;
        [SerializeField]
        private bool debugSwapPath = false;
        #endregion debug

        #region ShootBubble
        private static ShootBubble shootBubble;
        [SerializeField]
        private Transform bubblePosition;
        [SerializeField]
        private Transform nextBubblePosition;
        [SerializeField]
        private Transform instantiateBubblePosition;
        private static ShootBubble shootBubbleNext;
        public Action ShootEvent; // raise after pointerup
        private static ShootBubble hiddenShootBubble;
        private bool destroyHiddenAfterShoot = false;
        private int minBubbleScore;
        private int maxBubbleScore;
        public int GoodShoots { get; private set; }
        public int BubbleScore { get { return Mathf.Min(GoodShoots, 8) * 5 + minBubbleScore; }}
        private int AdditScore { get { return Mathf.Min(GoodShoots, 8) * 5 + 15; } }
        public int ShootScore { get { return ShootAreaLength*(BubbleScore + AdditScore); } }
        public int ShootAreaLength { get { return (shootGridCellsArea != null) ? shootGridCellsArea.Length : 0; } }
        public GetShootBubbleMode getShootBubbleMode = GetShootBubbleMode.FromBoardAtStart;


        [Space(8, order = 0)]
        [Header("Shooting", order = 1)]
        [SerializeField]
        private float speed = 5f;
        [SerializeField]
        private EaseAnim ease;
        private List<MainObjectData> shootBubblesOnBoartAtStart;        // shoot bubbles sequece created at startup
        private List<MainObjectData> shootBubblesOnBoartAtCurrTime;     // shoot bubbles sequece created at curr time
        #endregion ShootBubble

        #region shootline
        private GameObject shootLine;
        [SerializeField]
        private int multiRaysCount = 4;

        private Vector3 startScale;
        private Vector3 endScale;

        [SerializeField]
        private int nReflections = 10;
        [SerializeField]
        private float lastRayLength = 0.2f;


        private float swapArcHeight = 1f;

        public bool comleteShootLine = false;
        private List<Transform> shootLineObjects;
        private MultiRaySelector<MainObject> mRS;
        #endregion shootline

        [SerializeField]
        private float swapSpeed = 30f;

        [SerializeField]
        private Animator candyMachineAnim;
        [SerializeField]
        private Transform cannonAnchor;

        public static BubblesShooter Instance;

        #region path
        private PolyLine swapPath;
        private PolyLine swapPathReverse;
        private PolyLine initPath;
        #endregion path

        #region targets
        private static GridCell hitTargetGridCell;
        private static GridCell freeTargetGridCell;
        private static CellsGroup shootGridCellsArea; // not include free target gridcell
        #endregion targets

        #region touch range
        private Vector2 leftBound = new Vector2(-1, 0.7f); 
        private Vector2 rightBound = new Vector2(1, 0.7f);
        private float sqDist = 4; // minimal squared distance 
        #endregion touch ange

        #region properties
        public static bool CanShoot{ get { return freeTargetGridCell; } }
        #endregion properties

        #region regular
        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else
            {
                Instance = this;
            }

        }

        public void InitStart(int minBubbleScore, int maxBubbleScore)
        {
            this.minBubbleScore = minBubbleScore;
            this.maxBubbleScore = maxBubbleScore;
            int count = GameBoard.LcSet.levelMission.MovesConstrain;
            GoodShoots = 0;
            BubblesPlayer.Instance.SetScore(0);

            // create shoot bubbles sequence from board
            GetShootBubblesFromBoard();
            shootBubblesOnBoartAtStart = shootBubblesOnBoartAtCurrTime;// the same as  shootBubblesOnBoartAtCurrTime
            shootLineObjects = new List<Transform>();

            // create multiray selector and shootline
            shootLine = new GameObject();
            shootLine.name = "ShootLine";
            shootLine.transform.parent = transform;
            mRS = new MultiRaySelector<MainObject>(showDebugLines, multiRaysCount, nReflections);

            // create swapPath
            swapPath = new PolyLine(Curv.ArcCurve2D(bubblePosition.position, nextBubblePosition.position, 3, -swapArcHeight));
            swapPathReverse = swapPath.Reverse();
            initPath = new PolyLine(Curv.ArcCurve2D(instantiateBubblePosition.position, nextBubblePosition.position, 3, swapArcHeight));

            // set boosters eventhandlers
            BoostersHolder bHolder = BubblesPlayer.Instance.BoostHolder;
            foreach (var item in bHolder.Boosters)
            {
                item.ActivateEvent += BoosterActivateEventHandler;
                item.DeActivateEvent += BoosterDeActivateEventHandler;
            }
            ShootEvent += ShootEventHandler;
            ShootEvent += Booster.ShootEventHandler;
            SetNextBubble(null);
        }

        private void OnDestroy()
        {
            // remove boosters eventhandlers
            BoostersHolder bHolder = BubblesPlayer.Instance.BoostHolder;
            if (bHolder != null)
            {
                foreach (var item in bHolder.Boosters)
                {
                    item.ActivateEvent -= BoosterActivateEventHandler;
                    item.DeActivateEvent -= BoosterDeActivateEventHandler;
                }
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            ShowSwapPath();
#endif
        }
        #endregion regular

        #region instantiate new shootbubble
        private ShootBubble InstantiateShootBubble(GameObject prefab, Vector3 position, MainObjectData mData)
        {
            if(mData == null)
            {
               if(debug) Debug.Log("mData == null");
                return null;
            }
            ShootBubble sB = Instantiate(prefab, position, Quaternion.identity).GetComponent<ShootBubble>();
#if UNITY_EDITOR
            sB.name = "ShootBubble: " + mData.Name;
#endif
            sB.SetData(mData, null, GameBoard.Instance.animatedMaterial, SortingOrder.MainToFront, true, ShootBubbleClickEventHandler);
            sB.transform.localScale = transform.lossyScale;
            sB.transform.parent = transform;
            return sB;
        }

        private ShootBubble InstantiateShootBubbleBooster(Vector3 position, Booster booster)
        {
            if (booster == null || booster.bData == null)
            {
                if(debug)Debug.Log("booster == null");
                return null;
            }

            ShootBubble sB = Instantiate (booster.bData.prefab, position, Quaternion.identity).GetComponent<ShootBubble>();
#if UNITY_EDITOR
            sB.name = "ShootBubble booster: " + booster.bData.Name;
#endif
            sB.SetData(null, booster, GameBoard.Instance.animatedMaterial, SortingOrder.MainToFront, true, ShootBubbleClickEventHandler);
            sB.transform.localScale = transform.lossyScale;
            sB.transform.parent = transform;

            return sB;
        }

        private void SetNextBubble(Action completeCallBack)
        {
           if(debug) Debug.Log("SetNextBubble");

            GameObject prefab = GameBoard.GameObjSet.shootBubblePrefab;
            if (!prefab) {if(debug) Debug.Log("Matchset.shootBubblePrefab failed"); if (completeCallBack != null) completeCallBack(); return; }

            // select sequence 
            List<MainObjectData> currShootBubblesSequence = shootBubblesOnBoartAtStart; // default

            if(getShootBubbleMode == GetShootBubbleMode.FromCurrentBoard)
            {
                GetShootBubblesFromBoard();
                currShootBubblesSequence = shootBubblesOnBoartAtCurrTime;
            }

            if (currShootBubblesSequence == null || currShootBubblesSequence.Count == 0) { if (completeCallBack != null) completeCallBack(); return; }

            if (hiddenShootBubble) // if exist hidden after booster using
            {
                shootBubble = hiddenShootBubble;
                hiddenShootBubble = null;
                shootBubble.gameObject.SetActive(true);
            }

            TweenSeq sbSeq = new TweenSeq();
            if (!shootBubbleNext) // instantiate and move shootBubbleNext to nextBubblePosition
            {
                sbSeq.Add((callBack) =>
                {
                    shootBubbleNext = InstantiateShootBubble(prefab, instantiateBubblePosition.position, currShootBubblesSequence.GetRandomPos());
                    MoveAlongPath(shootBubbleNext.gameObject, initPath, initPath.Length/swapSpeed, 0, EaseAnim.EaseLinear, callBack);
                });
            }

            if (!shootBubble)
            {
                sbSeq.Add((callBack) => // move shootBubbleNext to position
                {
                    MoveAlongPath(shootBubbleNext.gameObject, swapPathReverse, swapPathReverse.Length/ swapSpeed, 0, EaseAnim.EaseLinear,
                        () =>
                        {
                            shootBubble = shootBubbleNext;
                            callBack();
                        });
                });


                sbSeq.Add((callBack) => // instantiate and move shootBubbleNext to nextBubblePosition
                {
                    shootBubbleNext = InstantiateShootBubble(prefab, instantiateBubblePosition.position, currShootBubblesSequence.GetRandomPos());
                    MoveAlongPath(shootBubbleNext.gameObject, initPath, 0.5f* initPath.Length/swapSpeed, 0, EaseAnim.EaseLinear,
                    () =>
                    {
                       callBack();
                    });
                });
            }

            sbSeq.Add((callBack) => // instantiate and move shootBubbleNext to nextBubblePosition
            {
                if (completeCallBack != null) completeCallBack();
                callBack();

            });
            sbSeq.Start();
        }

        /// <summary>
        /// fill array shootBubblesOnBoartAtCurrTime 
        /// </summary>
        private void GetShootBubblesFromBoard()
        {
            Dictionary<int, int> moDict = GameBoard.Instance.grid.GetMainObjectsDataDict();
            shootBubblesOnBoartAtCurrTime = new List<MainObjectData>();
            foreach (var item in moDict)
            {
                MainObjectData mData = GameBoard.GameObjSet.GetMainObject(item.Key);
                if (mData.canUseAsShootBubbles)
                {
                    shootBubblesOnBoartAtCurrTime.Add(mData);
                }
            }
        }
        #endregion instantiate new shootbubble

        #region shoot shootline
        public void ShowShootLine()
        {
            if (!shootBubble) return;
            shootLine.SetActive(true);
            CreateShootLine();
        }

        public void HideShootLine()
        {
            if(shootLine) shootLine.SetActive(false);
            HideTargetSelectors();
            if (shootBubble)
            {
                shootBubble.ActivateCollider(true); // need for swaping
            }

            RestoreMuzzleRotation();
        }

        private void CreateShootLine()
        {
            GridCell topGridCell = null;
            shootBubble.ActivateCollider(false);

            MainObject targetObject = mRS.SelectTarget(shootBubble.gameObject, TouchPad.Instance.GetWorldTouchPos(), lastRayLength, comleteShootLine, 0.5f, ref topGridCell);
            GridCell hitGridCell =  (targetObject) ? targetObject.ParentCell : null;
            if (!hitGridCell) hitGridCell = topGridCell;

            SetShootTargets(hitGridCell, (hitGridCell) ? mRS.HitPoint : Vector2.zero,  shootBubble);

            if (!hitGridCell)
            {
                DisableShootLine();
                return;
            }
            // create shoot line objects
            int objectsCount = (int)mRS.RayLength;
            startScale = shootBubble.transform.localScale * 0.5f;
            endScale = startScale * 0.2f;
            Vector3 deltaScale = (endScale - startScale) / (objectsCount - 1.0f);

            Sprite s = shootBubble.sprite;
            if (shootLineObjects.Count < objectsCount)
            {
                shootLineObjects.AddRange(CreateSpritesArray(objectsCount - shootLineObjects.Count, shootLine.transform, 25));
            }

            float dist = mRS.RayLength / objectsCount;   
            for (int i = 0; i < shootLineObjects.Count; i++)
            {
                if (i < objectsCount)
                {
                    shootLineObjects[i].GetComponent<SpriteRenderer>().sprite = s; 
                    shootLineObjects[i].position = GetMultiLinePosition((i + 1) * dist, mRS.ShootLinePoints, mRS.ShootLineLengths, mRS.LastRayIndex + 1);
                    shootLineObjects[i].localScale = startScale + deltaScale * i;
                    shootLineObjects[i].gameObject.SetActive(true);
                }
                else
                {
                    shootLineObjects[i].gameObject.SetActive(false);
                }
            }

            RotateMuzzle();
        }

        private void RotateMuzzle()
        {
            var touchPos = TouchPad.Instance.GetWorldTouchPos();
            var cannonPos = cannonAnchor.position;
            var upVector = touchPos - cannonPos;
            upVector.z = 0;
            upVector.Normalize();
            cannonAnchor.up = upVector;
        }

        private void RestoreMuzzleRotation()
        {
            cannonAnchor.up = new Vector3(0, 1, 0);
        }

        private void DisableShootLine()
        {
            if (shootLineObjects == null || shootLineObjects.Count == 0) return;
            for (int i = 0; i < shootLineObjects.Count; i++)
            {
                shootLineObjects[i].gameObject.SetActive(false);
            }

            RestoreMuzzleRotation();
        }

        /// <summary>
        /// Start shootbubble, completeCallback will be call after shootBubble reach the target gridcell
        /// </summary>
        /// <param name="completeCallBack"></param>
        public void Shoot(Action completeCallBack)
        {
            if (!shootBubble || !freeTargetGridCell)
            {
                if (completeCallBack != null) completeCallBack();
                return;
            }

            GameObject sBGO = shootBubble.gameObject;
            ShootBubble sB = shootBubble;
            shootBubble = null;
            mRS.CachePathToFreeTarget(freeTargetGridCell.transform.position);
            OnShootEvent();
            float rLength = mRS.ShootLineLengthsFT[mRS.ShootLineLengthsFT.Length - 1];
            float time = rLength / speed;

            SimpleTween.Value(sBGO, 0, rLength, time).
                SetOnUpdate((float d) =>
                {
                    sBGO.transform.position = GetMultiLinePosition(d, mRS.ShootLinePointsFT, mRS.ShootLineLengthsFT, mRS.ShootLinePointsFT.Length - 1);
                }).SetEase(ease).
                AddCompleteCallBack(() =>
                {
                    sB.ApplyShootBubbleToGrid(hitTargetGridCell, freeTargetGridCell, shootGridCellsArea);
                    SetNextBubble(null);
                    if (debug) Debug.Log("bubble group: " + shootGridCellsArea.ToString() + " length :" + shootGridCellsArea.Length);
                    if (completeCallBack != null) completeCallBack();
                    // Vítor Barcellos - dando start na animação de tiro da máquina de doces
                    candyMachineAnim.SetTrigger("Shoot");
                }
                );
        }

        /// <summary>
        /// Collect all bubbles from shootarea
        /// </summary>
        /// <param name="completeCallBack"></param>
        public void ShootCollect(Action completeCallBack)
        {
            if (shootGridCellsArea != null && shootGridCellsArea.Length >= 2)
            {
                if (debug) Debug.Log("shoot collect");
              //  shootGridCellsArea.Add(freeTargetGridCell);                     // add grid cell with shoot bubble to collection group
                shootGridCellsArea.ShootCollect(BubbleScore, true, completeCallBack); // collect 
                BubblesPlayer.Instance.AddScore(ShootScore);
                GoodShoots += 1;
            }
            else
            {
                GoodShoots = 0;
                if (debug) Debug.Log("empty shoot collect ");
                if (completeCallBack != null) completeCallBack();
            }
        }

        /// <summary>
        /// hide selectors
        /// </summary>
        private void HideTargetSelectors()
        {
            if (hitTargetGridCell) hitTargetGridCell.ShowTargetSelector(false, null);
            if (freeTargetGridCell) freeTargetGridCell.ShowTargetSelector(false, null);
            if (shootGridCellsArea != null) { shootGridCellsArea.ShowTargetSelector(false, null); }
        }

        public void CleanTargets()
        {
            if(debug) Debug.Log("clean");
            hitTargetGridCell = null;
            freeTargetGridCell = null;
            shootGridCellsArea = null;
        }

        /// <summary>
        /// Set new target gridcell, free target gridcell for shoot and shoot area
        /// </summary>
        /// <param name="newHitGCell"></param>
        /// <param name="hitPoint"></param>
        /// <param name="showShootAreaSelector"></param>
        /// <param name="showFreeTargetSelector"></param>
        private void SetShootTargets(GridCell newHitGCell, Vector2 hitPoint, ShootBubble sB)
        {
            // cache old targets
            CellsGroup oldShootArea = shootGridCellsArea;
            GridCell oldFreeTargetGridCell = freeTargetGridCell;
            GridCell oldHitTargetGridCell = hitTargetGridCell;

            hitTargetGridCell = newHitGCell;
            if (hitTargetGridCell) // found target - select new target
            {
                NeighBorns nCells = new NeighBorns(hitTargetGridCell, 0);
                if (hitTargetGridCell.IsEmpty && hitTargetGridCell.IsTopObjectGridcell) // is empty top cell
                {
                    freeTargetGridCell = hitTargetGridCell;
                }
                else
                {

                    bool left = hitPoint.x < hitTargetGridCell.transform.position.x;
                    bool bottom = hitPoint.y < hitTargetGridCell.transform.position.y;

                    freeTargetGridCell = nCells.GetEmpty(left, bottom);
                }

                shootGridCellsArea = sB.GetShootArea(hitTargetGridCell, freeTargetGridCell, GameBoard.Instance.grid);
                if (debug) Debug.Log("shoot area length: " + shootGridCellsArea.Length);
            }
            else  // not found target - deselect existing previous targets
            {
                Debug.Log("set shoot targets null");
                freeTargetGridCell = null;
                shootGridCellsArea = null;
            }

            SelectShootArea(sB,  oldHitTargetGridCell,  oldFreeTargetGridCell, oldShootArea);
        }

        private static void SelectShootArea(ShootBubble sB, GridCell oldHitGCell,  GridCell oldFreeTargetGridCell, CellsGroup oldShootArea)
        {
            if (hitTargetGridCell) // found target - select new target
            {
                hitTargetGridCell.ShowTargetSelector(sB.ShowHitTargetSelector);
                if (oldShootArea != null) oldShootArea.ShowTargetSelector(false);
                if (shootGridCellsArea != null) shootGridCellsArea.ShowTargetSelector(sB.ShowShootAreaSelector);
                if (freeTargetGridCell) freeTargetGridCell.ShowTargetSelector(sB.ShowFreeTargetSelector);

                if (oldHitGCell && oldHitGCell != hitTargetGridCell) oldHitGCell.ShowTargetSelector(false);                                           // deselect existing previous target
                if (oldFreeTargetGridCell && oldFreeTargetGridCell != freeTargetGridCell) oldFreeTargetGridCell.ShowTargetSelector(false);   // deselect existing previous free target
            }
            else  // not found target - deselect existing previous targets
            {
                if (oldHitGCell) oldHitGCell.ShowTargetSelector(false);
                if (oldFreeTargetGridCell) oldFreeTargetGridCell.ShowTargetSelector(false);
                if (oldShootArea != null) oldShootArea.ShowTargetSelector(false);
            }
        }
        #endregion shoot shootline

        #region utils
        /// <summary>
        /// Return position on multiline for distance
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="linePoints"></param>
        /// <param name="lengths"></param>
        /// <returns></returns>
        private Vector2 GetMultiLinePosition(float dist, Vector2[] linePoints, float[] lengths, int lastPointPosition)
        {
            int spI = -1;
            float dI = 0;
            Vector2 sP = Vector3.zero;
            Vector2 eP = Vector2.zero;

            for (int i = 0; i < lengths.Length; i++)
            {
                if (dist <= lengths[i])
                {
                    sP = linePoints[i];
                    eP = linePoints[i + 1];
                    spI = i;
                    dI = (i == 0) ? dist : dist - lengths[i - 1];
                    return (sP + (eP - sP).normalized * dI);
                }
            }
            return linePoints[lastPointPosition];
        }

        private List<Transform> CreateSpritesArray(int count, Transform parent,  int renderOrder)
        {
            List<Transform> res = new List<Transform>(count);
            for (int i = 0; i < count; i++)
            {
                GameObject g = new GameObject();
                g.transform.parent = parent;
                SpriteRenderer sR = g.AddComponent<SpriteRenderer>();
                sR.sortingOrder = renderOrder;
                res.Add(g.transform);
            }
            return res;
        }

        private void MoveAlongPath(GameObject gObject, PolyLine path, float time, float delay, EaseAnim easeAnim, Action completeCallBack)
        {
            SimpleTween.Value(gObject, 0, path.Length, time).
                   SetOnUpdate((float d) =>
                   {
                       if (gObject) gObject.transform.position = path.GetPolyLinePosition(d);

                   }).SetEase(easeAnim).SetDelay(delay).
                   AddCompleteCallBack(() =>
                   {
                       if (completeCallBack != null) completeCallBack();
                   });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TouchInRange()
        {
            Vector2 tPos = TouchPad.Instance.GetWorldTouchPos();// Debug.Log(tPos);
            Vector2 dPos = tPos - (Vector2)transform.position;

            if (dPos.sqrMagnitude < sqDist) return false; // too close
            float sProd = leftBound.x * dPos.y - leftBound.y * dPos.x; // vector product = A x B = A.x*B.y - B.x*A.y
            if (sProd >= 0) return false;

            sProd = rightBound.x * dPos.y - rightBound.y * dPos.x;
            if (sProd < 0) return false;
            return true;
        }
        #endregion utils

        #region swap
        /// <summary>
        /// Swap bubbles
        /// </summary>
        /// <param name="completeCallBack"></param>
        public void Swap(Action completeCallBack)
        {
            ParallelTween pT = new ParallelTween();

            ShootBubble temp = shootBubble;
            shootBubble = shootBubbleNext;
            shootBubbleNext = temp;
            pT.Add((callBack)=>
            {
                MoveAlongPath(shootBubbleNext.gameObject, swapPath, swapPath.Length / swapSpeed, 0, EaseAnim.EaseLinear, callBack);
            });

            pT.Add((callBack) =>
            {
                MoveAlongPath(shootBubble.gameObject, swapPathReverse, swapPathReverse.Length / swapSpeed, 0, EaseAnim.EaseLinear, callBack);
            });
            pT.Start(completeCallBack);

            // Vítor Barcellos - dando start na animação de tiro da máquina de doces
            candyMachineAnim.SetTrigger("Shoot");

        }

        /// <summary>
        /// Show path only for debug purposes
        /// </summary>
        public void ShowSwapPath()
        {
            if (!debugSwapPath) return;
            if (swapPath == null) return;
            swapPath.Display();
            initPath.Display();
        }
        #endregion swap

        #region handlers
        private void BoosterActivateEventHandler(Booster b)
        {
            destroyHiddenAfterShoot = false;
            ShootBubble sB = InstantiateShootBubbleBooster(bubblePosition.position, b);
            if (!hiddenShootBubble)
            {
                hiddenShootBubble = shootBubble; // save shootbubble
                hiddenShootBubble.gameObject.SetActive(false);
            }
            else // current shootBubble is booster ->  destroy
            {
                ShootBubble tShootBubble = shootBubble;
                Destroy(tShootBubble.gameObject);
            }
            sB.GetComponent<BoosterFunc>().InitStart(hiddenShootBubble, ref destroyHiddenAfterShoot); // pass the hidden to the booster func, booster can destroy hidden shootbubble after shoot 
            shootBubble = sB; // set new shoot bubble
        }

        private void BoosterDeActivateEventHandler(Booster b)
        {
            ShootBubble tS = shootBubble;
            Destroy(tS.gameObject);
            shootBubble = hiddenShootBubble;
            if(shootBubble) shootBubble.gameObject.SetActive(true);
            hiddenShootBubble = null;
        }

        private void ShootBubbleClickEventHandler()
        {
            if (Booster.ActiveBooster != null)
            {
                Booster.ActiveBooster.DeActivateBooster();
            }
            Swap(null);
        }

        private void ShootEventHandler()
        {
            if (destroyHiddenAfterShoot)
            {
                if (debug) Debug.Log("Destroy hidden shootbubble");
                ShootBubble tS = hiddenShootBubble;
                if (hiddenShootBubble) Destroy(tS.gameObject);
                hiddenShootBubble = null;
            }
        }
        #endregion handlers

        #region event raise
        private void OnShootEvent()
        {
            if (ShootEvent != null)
                ShootEvent();
        }
        #endregion event raise
    }

    #region target selector
    // ray container
    public class SurfRay
    {
        public RaycastHit2D hit { get; private set; }
        public Ray2D ray { get; private set; }
        public float RayHitMagnitude { get; private set;  } // public Vector2 StartPoint { get; private set; }

        public SurfRay(Vector2 dir, Vector2 startPoint)
        {
           ray = new Ray2D(startPoint, dir); // StartPoint = startPoint;
           
           if( hit = Physics2D.Raycast(ray.origin, ray.direction))
            {
                RayHitMagnitude = (hit.point - startPoint).magnitude;// RayHitMagnitude = (hit.point - StartPoint).magnitude;
            }
            else
            {
                RayHitMagnitude = float.MaxValue;
            }
        }
    }

    // multi ray container
    public class SurfRaysBundle
    {
        public SurfRay[] Bundle { get; private set; }
        public Vector2 EndCenterPoint { get; private set; }
        public Vector2 ReflectedDir { get; private set; }
        public Transform Target { get; private set; }
        public float Length { get; private set; } // return min ray length
        public float SumLength { get; private set; } // return min ray length
        public RaycastHit2D Hit { get; private set; }
        public Vector2 DirectionOne // normalized direction
        {
            get; private set;
        }
        private SurfRay shortestRay; // 

        /// <summary>
        /// Create multi ray bundle from bubble surface
        /// </summary>
        /// <param name="count"></param>
        /// <param name="startCenterPoint"></param>
        /// <param name="direction"></param>
        /// <param name="radius"></param>
        /// <param name="rayStartOffsets"></param>
        public SurfRaysBundle(Vector2 startCenterPoint, Vector2 direction, Vector2[] rayStartOffsets, bool debug)
        {
            DirectionOne = direction.normalized;
            Vector2 dirNormal = new Vector3(-DirectionOne.y, DirectionOne.x);
           
            Bundle = new SurfRay[rayStartOffsets.Length];

            for (int i = 0; i < rayStartOffsets.Length; i++)
            {
                Bundle[i] = new SurfRay(direction, startCenterPoint + dirNormal * rayStartOffsets[i].x + DirectionOne * rayStartOffsets[i].y);

#if UNITY_EDITOR
                if (debug)
                {
                    DebugDrawCircle(Bundle[i].ray.origin, 0.1f, Color.green); 
                    if (Bundle[i].hit)
                    {
                        DebugDrawCircle(Bundle[i].hit.point, 0.1f, Color.red);
                      //  Debug.Log(i + ") hit.normal " + Bundle[i].hit.normal + " ; hit point: " + Bundle[i].hit.point + " ; dir: " + direction + " ; length: " + Bundle[i].RayHitMagnitude + " ; " + Bundle[i].hit.transform);
                    }
                }
#endif
            }

            //search shortest ray
            shortestRay = Bundle[0];
            Length = shortestRay.RayHitMagnitude;       
            for (int i = 1; i < Bundle.Length; i++)
            {
                if((Bundle[i].hit) && Bundle[i].RayHitMagnitude < shortestRay.RayHitMagnitude)
                {
                    shortestRay = Bundle[i];
                }
            }
             
            if (Hit = shortestRay.hit)
            {
                Length = shortestRay.RayHitMagnitude;  
                Target = Hit.transform;
                EndCenterPoint = startCenterPoint + DirectionOne * Length * 0.999f; // 0.999f for avoid collider penetration 
                ReflectedDir = Vector2.Reflect(shortestRay.ray.direction, Hit.normal);
            }
        }

#if UNITY_EDITOR
        public void DebugLineForEach()
        {
            if (Bundle != null)
            {
                for (int i = 0; i < Bundle.Length; i++)
                {
                    if (Bundle[i].hit)
                    {
                        Debug.DrawLine(Bundle[i].ray.origin, Bundle[i].hit.point, Color.black);// Debug.DrawLine(Bundle[i].StartPoint, Bundle[i].hit.point, Color.black);
                    }
                }
            }
        }

        private void DebugDrawCircle(Vector2 center, float radius, Color color)
        {
            int count = 20;
            float da = 2 * Mathf.PI / count;
            Vector2[] pos = new Vector2[count + 1];
            for (int i = 0; i < count; i++)
            {
                float ida = i * da;
                pos[i] = center + new Vector2(Mathf.Cos(ida) * radius, Mathf.Sin(ida) * radius);
            }
            pos[count] = pos[0];
            for (int i = 0; i < count; i++)
            {
                Debug.DrawLine(pos[i], pos[i + 1], color);
            }
        }
#endif
    }

    // multi ray selector
    public class MultiRaySelector<T> where T : MonoBehaviour
    {
        public SurfRaysBundle[] surfRayBundles { get; private set; }
        private Vector2[] rayStartOffsets;
        private float bubbleRadius = 0;
        private int raysCount;
        private int fullRaysCount;
        public T Target { get; private set; }
        private int nReflections; //reflections count
        private Vector2 centerPoint;
        private Vector2 reflectDirection;
        public int LastRayIndex { get; private set; }
        private bool showDebugLines;

        #region shoot line
        public float[] ShootLineLengths { get; private set; }
        public Vector2[] ShootLinePoints { get; private set; } // shoot line 
        public float RayLength { get; private set; }
        public Vector2 HitPoint { get; private set; }
        #endregion shoot line

        #region shoot path
        public float[] ShootLineLengthsFT { get; private set; } 
        public Vector2[] ShootLinePointsFT { get; private set; }// path to free target
        #endregion shoot path

        public MultiRaySelector(bool showDebugLines, int raysCount, int nReflections)
        {
            this.raysCount = raysCount;
            this.nReflections = nReflections;
            fullRaysCount = raysCount * 2 + 1;
            rayStartOffsets = new Vector2[fullRaysCount];
            surfRayBundles = new SurfRaysBundle[nReflections + 1];
            this.showDebugLines = showDebugLines;
            ShootLinePoints = new Vector2[surfRayBundles.Length + 1];
            ShootLineLengths = new float[surfRayBundles.Length];
        }

        public T SelectTarget (GameObject shootBubble, Vector2 wTouchPosition, float lastRayLength,  bool completeShootLine, float scale, ref GridCell topGridCell)
        {
            float r = shootBubble.GetComponent<CircleCollider2D>().radius * shootBubble.transform.lossyScale.x * scale;
            ShootLinePoints[0] = shootBubble.transform.position; // first position

                //1) create rayStartOffsets
                if (bubbleRadius != r)
                {
                    bubbleRadius = r;
                    rayStartOffsets[0] = new Vector2(0, r);
                    rayStartOffsets[raysCount] = new Vector2(r, 0);
                    rayStartOffsets[raysCount + raysCount] = new Vector2(-r, 0);

                    float dD = bubbleRadius / raysCount;
                    float dDi = 0;
                    float sR = bubbleRadius * bubbleRadius;
                    for (int i = 1; i < raysCount; i++)
                    {
                        dDi = dD * i;
                        rayStartOffsets[i] = new Vector2(dDi, Mathf.Sqrt(sR - (dDi * dDi)));
                        rayStartOffsets[i + raysCount] = new Vector2(-dDi, rayStartOffsets[i].y);
                    }
                }


            //2 create reflection rays
            SurfRaysBundle srBundle = null;
            Target = null;
            LastRayIndex = -1;
            for (int i = 0; i <= nReflections; i++)
            {
                if (i == 0) // start ray to touch position
                {
                    centerPoint = shootBubble.transform.position;
                    reflectDirection = wTouchPosition - centerPoint;
                    srBundle = new SurfRaysBundle(centerPoint, reflectDirection, rayStartOffsets, showDebugLines);
                    surfRayBundles[0] = srBundle;
#if UNITY_EDITOR
                    if (showDebugLines)
                    {
                        srBundle.DebugLineForEach();
                        DebugDrawCircle(centerPoint, bubbleRadius, Color.cyan);
                    }
#endif
                }
                else if (srBundle != null && srBundle.Hit) // reflected rays
                {
                    centerPoint = srBundle.EndCenterPoint;
                    reflectDirection = srBundle.ReflectedDir;
                    srBundle = new SurfRaysBundle(centerPoint, reflectDirection, rayStartOffsets, showDebugLines);
                    surfRayBundles[i] = srBundle;
#if UNITY_EDITOR
                    if (showDebugLines && srBundle.Hit.collider)
                    {
                        Debug.Log(i + ") " + srBundle.Hit.collider.name);
                        srBundle.DebugLineForEach();
                        DebugDrawCircle(centerPoint, bubbleRadius, Color.cyan);
                    }
#endif
                }
                else if (srBundle != null && !srBundle.Hit)
                {
                    srBundle = null;
                }

                if ((Target = (srBundle != null && srBundle.Hit && srBundle.Hit.transform) ? srBundle.Hit.transform.GetComponent<T>() : null) || (topGridCell = (srBundle != null && srBundle.Hit && srBundle.Hit.transform) ? srBundle.Hit.transform.GetComponent<GridCell>() : null) )
                {
                    LastRayIndex = i;
                    HitPoint = surfRayBundles[LastRayIndex].Hit.point;
                    CacheVertexAndLengts(completeShootLine, lastRayLength);  if(showDebugLines) Debug.Log(i + ") " +srBundle.Hit.transform.name);
                    break; 
                }
            }
            return Target;
        }

        /// <summary>
        /// Cache shootline
        /// </summary>
        /// <param name="completeShootLine"></param>
        /// <param name="lastRayLength"></param>
        private void CacheVertexAndLengts(bool completeShootLine, float lastRayLength)
        {
            RayLength = 0;
            if (LastRayIndex == 0)
            {
                ShootLinePoints[1] = surfRayBundles[0].EndCenterPoint;
            }
            else
            {
                for (int i = 0; i <= LastRayIndex; i++)
                {
                    if (i < LastRayIndex)
                        ShootLinePoints[i + 1] = surfRayBundles[i].EndCenterPoint;
                    else // last shoot ray - not complete
                    {
                        Vector2 startPoint = surfRayBundles[i-1].EndCenterPoint;
                        Vector2 endPoint = surfRayBundles[i].EndCenterPoint;
                        ShootLinePoints[i + 1] = (completeShootLine) ? endPoint : startPoint + lastRayLength * (endPoint - startPoint);
                    }
                }
            }

            // cache lengths
            for (int i = 1; i < LastRayIndex + 2; i++)
            {
                RayLength += (ShootLinePoints[i] - ShootLinePoints[i - 1]).magnitude;
                ShootLineLengths[i - 1] = RayLength;

#if UNITY_EDITOR
                if (showDebugLines) Debug.DrawLine(ShootLinePoints[i - 1], ShootLinePoints[i], Color.green);
#endif
            }
        }

        /// <summary>
        /// Cache shoot path
        /// </summary>
        /// <param name="lastTarget"></param>
        public void CachePathToFreeTarget(Vector2 lastTarget)
        {
            int length = LastRayIndex + 2;
            ShootLinePointsFT = new Vector2[length];
            for (int i = 0; i < length - 1; i++)
            {
                ShootLinePointsFT[i] = ShootLinePoints[i];
            }
            ShootLinePointsFT[length - 1] = lastTarget;


            length = LastRayIndex + 1;
            ShootLineLengthsFT = new float[length];
            for (int i = 0; i < length - 1; i++)
            {
                ShootLineLengthsFT[i] = ShootLineLengths[i];
            }
            float l = (ShootLinePointsFT[ShootLinePointsFT.Length - 2] - lastTarget).magnitude;
            ShootLineLengthsFT[length - 1] =(length>1) ? ShootLineLengthsFT[length - 2] +  l : l;
        }

#if UNITY_EDITOR
        private void DebugDrawCircle(Vector2 center, float radius, Color color)
        {
            int count = 20;
            float da = 2 * Mathf.PI / count;
            Vector2[] pos = new Vector2[count + 1];
            for (int i = 0; i < count; i++)
            {
                float ida = i * da;
                pos[i] = center + new Vector2(Mathf.Cos(ida) * radius, Mathf.Sin(ida) * radius);
            }
            pos[count] = pos[0];
            for (int i = 0; i < count; i++)
            {
                Debug.DrawLine(pos[i], pos[i + 1], color);
            }
        }
#endif
    }

    public class LineSegment
    {
        public Vector2 P1 { get; private set; }
        public Vector2 P2 { get; private set; }
        public float Magnitude { get { return (magnitude >= 0) ? magnitude : magnitude = (P2 - P1).magnitude; } }
        private float magnitude = -1;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public LineSegment(Vector2 p1, Vector2 dir, float length)
        {
            P1 = p1;
            P2 = p1 + dir.normalized * length;
        }

        public LineSegment(LineSegment ls, bool calcMagnitude)
        {
            P1 = ls.P1;
            P2 = ls.P2;
        }

        public bool IsIntersected(LineSegment ls)  //кормен - каждый из отрезков пересекает прямую с други отрезком или конечная точка одного лежит на другом 
        {
            Func<Vector2, Vector2, float> vectProd = (v1, v2) =>
            {
                return v1.x * v2.y - v2.x * v1.y;
            };

            Vector2 p3 = ls.P1;
            Vector2 p4 = ls.P2;

            Vector2 p1p3 = p3 - P1;
            Vector2 p1p2 = P2 - P1;
            Vector2 p1p4 = p4 - P1;
            Vector2 p3p4 = p4 - p3;

            float d1 = vectProd((p1p3), (p1p2)) * vectProd((p1p4), (p1p2));
            float d2 = vectProd((-p1p3), (p3p4)) * vectProd((P2 - p3), (p3p4));

            return ((d1 <= 0) && (d2 <= 0));
        }

        public enum PointPosition { LEFT, RIGHT, BEYOND, BEHIND, BETWEEN, ORIGIN, DESTINATION }; // СЛЕВА, СПРАВА, ВПЕРЕДИ, ПОЗАДИ, МЕЖДУ, НАЧАЛО, КОНЕЦ

        /// <summary>
        /// Return relative point position to line
        /// https://dxdy.ru/topic81630.html
        /// 
        /// http://informatics.mccme.ru/course/view.php?id=22
        /// Ласло М. Вычислительная геометрия и компьютерная графика на C++
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public PointPosition Classify(Vector2 p)
        {
            /*
                1) https://acmp.ru/article.asp?id_text=172
                при общей начальной точке двух векторов их векторное произведение 
                больше нуля, если второй вектор направлен влево от первого,       
                и меньше нуля, если вправо. 
                2) либо через скалярное произведение повернутого на -90 градусов вектора на точку, 
                хотя и не очень очевидно
             */
            Vector2 a = P2 - P1; // 1
            Vector2 b = p - P1; // 2
            double sa = a.x * b.y - b.x * a.y; // 3 b' = d rotate -90 = (b.y, -bx); dot prduct a*b' = a.x*b.y - b.x*a.y;
            if (sa > 0.0)
                return PointPosition.LEFT;
            if (sa < 0.0)
                return PointPosition.RIGHT;
            if ((a.x * b.x < 0.0) || (a.y * b.y < 0.0))
                return PointPosition.BEHIND;
            if (a.sqrMagnitude < b.sqrMagnitude)
                return PointPosition.BEYOND;
            if (P1 == p)
                return PointPosition.ORIGIN;
            if (P2 == p)
                return PointPosition.DESTINATION;
            return PointPosition.BETWEEN;
        }

        /// <summary>
        /// Return new segment with offset. If dist>0 positive offset.
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public LineSegment OffsetSegment(float dist)
        {
            Vector2 dL = P2 - P1;
            Vector2 offsetDir = (dist > 0) ? new Vector2(dL.y, -dL.x).normalized * dist : new Vector2(-dL.y, dL.x).normalized * -(dist); // normal vector to dL
            return new LineSegment(P1 + offsetDir, P2 + offsetDir);
        }

        /// <summary>
        /// Return intersection point between 2 segments
        /// </summary>
        /// <param name="lS"></param>
        /// <returns></returns>
        public Vector2 GetIntersectionPointWith(LineSegment lS)
        {
            Vector2 iP = new Vector2();
            return iP;
        }

        /// <summary>
        /// Display with transform from local scpace to world, with white color
        /// </summary>
        /// <param name="t"></param>
        /// <param name="color"></param>
        public void Display(Transform t)
        {
            Display(t, Color.white);
        }

        /// <summary>
        /// Display with transform from local scpace to world
        /// </summary>
        /// <param name="t"></param>
        /// <param name="color"></param>
        public void Display(Transform t, Color color)
        {
            Debug.DrawLine(t.TransformPoint(P1), t.TransformPoint(P2), color);
        }

        /// <summary>
        /// Display with white color
        /// </summary>
        /// <param name="t"></param>
        /// <param name="color"></param>
        public void Display()
        {
            Display(Color.white);
        }

        public void Display(Color color)
        {
            Debug.DrawLine(P1, P2, color);
        }
    }

    public class PolyLine
    {
        public List<Vector3> linePoints;
        public List<float> lengths;
        private List<LineSegment> lineSegments;
        public float Length {get { return (lengths == null || lengths.Count==0) ? 0 : lengths[lengths.Count - 1]; }}
        public Vector3 Direction { get { return (linePoints == null || linePoints.Count < 2) ? Vector3.zero : linePoints[linePoints.Count - 1] - linePoints[0]; } }
        private Vector3 directionOne = new Vector3(11,11,11);
        public Vector3 DirectionOne { get { return (directionOne.x > 10) ? directionOne = Direction.normalized : directionOne;} }

        public PolyLine(IEnumerable<Vector3> pLPoints)
        {
            lineSegments = new List<LineSegment>();
            if (pLPoints == null)
            {
                linePoints = new List<Vector3>();
                return;
            }
            linePoints = new List<Vector3>(pLPoints);

            for (int i = 0; i < linePoints.Count - 1; i++)
            {
                lineSegments.Add(new LineSegment(linePoints[i], linePoints[i + 1]));
            }

            float accDist = 0;
            lengths = new List<float>(linePoints.Count);
            lengths.Add(accDist);

            for (int posI = 0; posI < lineSegments.Count; posI++)
            {
                accDist += lineSegments[posI].Magnitude;
                lengths.Add(accDist);
            }
        }

        public PolyLine(IEnumerable<Vector2> pLPoints)
        {
            lineSegments = new List<LineSegment>();
            if (pLPoints == null)
            {
                linePoints = new List<Vector3>();
                return;
            }
            linePoints = new List<Vector3>();
            foreach (var item in  pLPoints)
            {
                linePoints.Add(item);
            }

            for (int i = 0; i < linePoints.Count - 1; i++)
            {
                lineSegments.Add(new LineSegment(linePoints[i], linePoints[i + 1]));
            }

            float accDist = 0;
            lengths = new List<float>(linePoints.Count);
            lengths.Add(accDist);

            for (int posI = 0; posI < lineSegments.Count; posI++)
            {
                accDist += lineSegments[posI].Magnitude;
                lengths.Add(accDist);
            }
        }

        private List<int> GetNeighBornSegments(LineSegment lSegment)
        {
            List<int> res = new List<int>(1);
            int index = lineSegments.IndexOf(lSegment);

            if (index >= 0 && index < lineSegments.Count)
            {
                if (index > 0) res.Add(index - 1);
                if (index < lineSegments.Count - 1) res.Add(index + 1);
            }
            return res;
        }

        /// <summary>
        /// return true if polyline self intersected
        /// </summary>
        /// <returns></returns>
        public bool IsSelfIntersected()
        {
            List<LineSegment> ls;
            for (int i = 0; i < lineSegments.Count; i++)
            {
                ls = new List<LineSegment>(lineSegments);
                int minI = (i > 0) ? i - 1 : 0;
                int maxI = (i < lineSegments.Count - 1) ? i + 1 : lineSegments.Count - 1;
                ls.RemoveRange(minI, maxI - minI + 1);
                for (int lsi = 0; lsi < ls.Count; lsi++)
                {
                    if (lineSegments[i].IsIntersected(ls[lsi])) return true;
                }
            }
            return false;
        }

        private List<LineSegment> OffsetSegments(float dist)
        {
            List<LineSegment> oSegments = new List<LineSegment>(lineSegments.Count);
            for (int i = 0; i < lineSegments.Count; i++)
            {
                oSegments.Add(lineSegments[i].OffsetSegment(dist));
            }
            return oSegments;
        }

        public void Display(Transform t)
        {
            lineSegments.ForEach((l) => { l.Display(t); });
        }

        public void Display()
        {
            lineSegments.ForEach((l) => { l.Display(); });
        }

        public void DisplayOffset(Transform t, float dist)
        {
            List<LineSegment> oSegments = OffsetSegments(dist);
            oSegments.ForEach((l) => { l.Display(t); });
        }

        /// <summary>
        /// Return position on multiline for distance
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="linePoints"></param>
        /// <param name="lengths"></param>
        /// <returns></returns>
        public Vector2 GetPolyLinePosition(float dist)
        {
            int spI = -1;
            float dI = 0;
            Vector2 sP = Vector3.zero;
            Vector2 eP = Vector2.zero;

            for (int i = 1; i < lengths.Count; i++)
            {
                if (dist <= lengths[i])
                {
                    sP = linePoints[i-1];
                    eP = linePoints[i];
                    spI = i;
                    dI = dist - lengths[i - 1];
                    return (sP + (eP - sP).normalized * dI);
                }
            }
            return linePoints[linePoints.Count-1];
        }

        /// <summary>
        /// Return position on multiline for distance and current segment number
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="linePoints"></param>
        /// <param name="lengths"></param>
        /// <returns></returns>
        public Vector2 GetPolyLinePosition(float dist, out int segNumber)
        {
            int spI = -1;
            float dI = 0;
            Vector2 sP = Vector3.zero;
            Vector2 eP = Vector2.zero;
            segNumber = 0;
            for (int i = 1; i < lengths.Count; i++)
            {
                if (dist <= lengths[i])
                {
                    sP = linePoints[i - 1];
                    segNumber = i-1;
                    eP = linePoints[i];
                    spI = i;
                    dI = dist - lengths[i - 1];
                    return (sP + (eP - sP).normalized * dI);
                }
            }
            return linePoints[linePoints.Count - 1];
        }

        public PolyLine Reverse()
        {
            List<Vector3> nPoints = new List<Vector3>(linePoints.Count);
            for (int i = linePoints.Count-1; i >=0; i--)
            {
                nPoints.Add(linePoints[i]);
            }
            return new PolyLine(nPoints);
        }
    }
    #endregion target selector
}



