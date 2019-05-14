using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Mkey
{
    public class GridCell : MonoBehaviour, ICustomMessageTarget
    {
        #region debug
        bool debug = false;
        #endregion debug

        #region row column
        public Row<GridCell> GRow { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public List<Row<GridCell>> rows { get; private set; }
        public PFCell pfCell;
        public bool IsEvenRow { get; private set; }
        #endregion row column

        #region objects
        [SerializeField]
        public MainObject Mainobject {get; private set; }
        public OverlayObject Overlay { get; private set; }
        #endregion objects

        #region cache fields
        private CircleCollider2D coll2D;
        private SpriteRenderer sRenderer;
        private GameObject targetSelector;
        #endregion cache fields

        #region events
        public Action<GridCell> PointerDownEvent;
        public Action<GridCell> DragEnterEvent;
        #endregion events

        #region properties 
        /// <summary>
        /// Return true if mainobject and mainobject IsMatchedById || IsMatchedWithAny
        /// </summary>
        /// <returns></returns>
        public bool IsMatchable
        {
            get { return (Mainobject && Mainobject.IsMatchedById); }
        }

        /// <summary>
        /// Return true if mainobject == null
        /// </summary>
        public bool IsEmpty
        {
            get { return !Mainobject; }
        }

        /// <summary>
        /// Return true if gridcell row==0
        /// </summary>
        public bool IsTopGridcell
        {
            get { return Row == 0; }
        }

        /// <summary>
        /// Return true if gridcell row is top used for gameobects (not service)
        /// </summary>
        public bool IsTopObjectGridcell
        {
            get { return GRow.isTopObjectRow; }
        }

        /// <summary>
        /// Return true if MO IsExploidable  and !FullProtected
        /// </summary>
        /// <returns></returns>
        public bool IsExploidable
        {
            get
            {
                if (Mainobject && Mainobject.IsExploidable) return true;
                return false;
            }
        }
        #endregion properties 

        #region Objects behavior
        internal void SetMainObject(MainObjectData mObjectData, float radius, GameMode gMode)
        {
            if (mObjectData == null) { return; }
            if (!Mainobject)
            {
                Mainobject = MainObject.Create(this, mObjectData, gMode == GameMode.Play, radius, true);
              //  Debug.Log("set mo : " +Mainobject.ToString() );
                if (gMode == GameMode.Play)
                {
                    if (Mainobject == null && IsTopObjectGridcell)
                    {
                        if (coll2D) coll2D.enabled = true; // use as target for shooting
                    }
                    if (Mainobject != null && IsTopObjectGridcell)
                    {
                        if (coll2D) coll2D.enabled = false;
                    }
                }
                else
                {
                    if (coll2D) coll2D.enabled = true;
                }

            }

            else
                Mainobject.SetData(mObjectData);
            // Debug.Log("Set main object: " + ToString());
        }

        internal void SetOverlay(OverlayObjectData mObjectData)
        {
            if (mObjectData == null) return;
            if (!Overlay)
                Overlay = OverlayObject.Create(this, mObjectData);
            else
                Overlay.SetData(mObjectData);

        }

        /// <summary>
        /// Set grid cell main object to null, run CollectDelegate (if set) and completeCallBack
        /// </summary>
        /// <param name="completeCallBack"></param>
        internal void CollectShootAreaObject(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            if (!Mainobject)
            {
                if (completeCallBack != null) completeCallBack();
                return;
            }
            Mainobject.ShootAreaCollect(completeCallBack, showPrivateScore, addPrivateScore, decProtection, privateScore);
            Mainobject = null;
            if(IsTopObjectGridcell) coll2D.enabled = true;
            if (Overlay) Overlay.ShootAreaCollect(null, showPrivateScore, addPrivateScore, decProtection, privateScore);
            Overlay = null;
        }

        internal void CollectFalledDown(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            if (!Mainobject)
            {
                if (completeCallBack != null) completeCallBack();
                return;
            }
            Mainobject.gameObject.layer = 2; // ignore shoot raycasting
            Mainobject.FallDownCollect(completeCallBack, showPrivateScore, addPrivateScore, privateScore);
            Mainobject = null;
            if (IsTopObjectGridcell) coll2D.enabled = true;
            if (Overlay)
            {
                Overlay.gameObject.layer = 2; // ignore shoot raycasting
                Overlay.FallDownCollect(null, showPrivateScore, addPrivateScore, privateScore);
            }
            Overlay = null;
        }

        /// <summary>
        /// Side hit from shoot bubble, it worked with destroayble mainobject
        /// </summary>
        internal void ShootHit(Action completeCallBack)
        {
            if (!Mainobject)
            {
                if (completeCallBack != null) completeCallBack();
                return;
            }
            Mainobject.ShootHit(completeCallBack);
            if (Mainobject && Mainobject.Protection <= 0) Mainobject = null;

            if (Overlay)
            {
                Overlay.ShootHit(null);
            }
        }
        #endregion Objects behavior

        /// <summary>
        ///  used by instancing for cache data
        /// </summary>
        internal void Init(int cellRow, int cellColumn, List<Row<GridCell>> rows, GameMode gMode)
        {
            Row = cellRow;
            Column = cellColumn;
            GRow = rows[cellRow];
            IsEvenRow = (cellRow % 2 == 0);
            this.rows = rows;

#if UNITY_EDITOR
            name = ToString();
#endif
            sRenderer = GetComponent<SpriteRenderer>();
            if(sRenderer) sRenderer.sortingOrder = SortingOrder.Base;
            coll2D = GetComponent<CircleCollider2D>();
            if (gMode == GameMode.Play && !IsTopObjectGridcell) DestroyImmediate(GetComponent<CircleCollider2D>());
            if (gMode == GameMode.Play) DestroyImmediate(GetComponent<SpriteRenderer>()); // 
        }

        /// <summary>
        ///  return true if main MainObjects of two cells are equal
        /// </summary>
        internal bool IsMainObjectEquals(GridCell other)
        {
            if (other == null) return false;
            if (other.Mainobject == null) return false;
            if (Mainobject == null) return false;

            return Mainobject.Equals(other.Mainobject);//Check whether the MainObject properties are equal. 
        }

        /// <summary>
        ///  cancel any tween on main MainObject object
        /// </summary>
        internal void CancelTween()
        {
            if (Mainobject)
            {
                Mainobject.CancellTweensAndSequences();
            }
            if (Overlay)
            {
                Overlay.CancellTweensAndSequences();
            }
        }

        /// <summary>
        /// DestroyImeediate MainObject, OverlayProtector, UnderlayProtector
        /// </summary>
        internal void DestroyGridObjects()
        {
            if (Mainobject) { DestroyImmediate(Mainobject.gameObject); Mainobject = null; }
            if (Overlay) { DestroyImmediate(Overlay.gameObject); Overlay = null; }
        }
    
        internal void ShowTargetSelector(bool show, Sprite sprite)
        {
            if (targetSelector && show) // need and also exist
            {
                return;
            }
            else if(!targetSelector && show && sprite) // need but not exist - create new 
            {
                targetSelector = Creator.CreateSpriteAtPosition(transform, sprite, transform.position, SortingOrder.TargetSelector).gameObject;
            }
            else if (targetSelector && !show) // not need but exist
            {
                DestroyImmediate(targetSelector);
            }
        }

        internal void ShowTargetSelector(bool show)
        {
            ShowTargetSelector(show, GameBoard.GameObjSet.selector);
        }

        #region raise events
        private void OnPointerDownEvent(GridCell gCell)
        {
            if (PointerDownEvent != null) PointerDownEvent(gCell);
        }

        private void OnDragEnterEvent(GridCell gCell)
        {
            if (DragEnterEvent != null) DragEnterEvent(gCell);
        }
        #endregion raise events

        #region touchbehavior only for construct
        public void PointerDown(TouchPadEventArgs tpea)
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                OnPointerDownEvent(this);
            }
        }

        public void Drag(TouchPadEventArgs tpea)
        {

        }

        public void DragBegin(TouchPadEventArgs tpea)
        {

        }

        public void DragDrop(TouchPadEventArgs tpea)
        {

        }

        public void DragEnter(TouchPadEventArgs tpea)
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                Debug.Log("drag enter " + ToString());
                DragEnterEvent(this);
            }
        }

        public void DragExit(TouchPadEventArgs tpea)
        {

        }

        public void PointerUp(TouchPadEventArgs tpea)
        {

        }

        public GameObject GetDataIcon()
        {
            return gameObject;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        #endregion touchbehavior only for construct

        #region override 
        public override string ToString()
        {
            return "cell : [ row: " + Row + " , col: " + Column + "]";
        }
        #endregion override

    }

    /// <summary>
    /// Get neighborns for gridcell 
    /// </summary>
    public class NeighBorns
    {
        public GridCell Main { get; private set; }
        public GridCell Left { get; private set; }
        public GridCell Right { get; private set; }
        public GridCell TopLeft { get; private set; }
        public GridCell TopRight { get; private set; }
        public GridCell BottomLeft { get; private set; }
        public GridCell BottomRight { get; private set; }
        public List<GridCell> Cells { get; private set; }
        public List<GridCell> EqualIDCells { get; private set; }
        public List<GridCell> EmptyCells { get; private set; }
        public List<GridCell> NotEmptyCells { get; private set; }


        /// <summary>
        /// Create NeighBorns  cells arrays: EqualIdCells (for id), EmptyCells, NotEmptyCells
        /// </summary>
        /// <param name="main"></param>
        /// <param name="id"></param>
        public NeighBorns(GridCell main, int id)
        {
            Main = main;
            int leftCol = main.Column - 1;
            int rightCol = main.Column + 1;
            Row<GridCell> botRow = (main.Row + 1 < main.rows.Count) ? main.rows[main.Row + 1] : null;
            Row<GridCell> topRow = (main.Row - 1 >= 0) ? main.rows[main.Row - 1] : null;

            Left = main.GRow[leftCol];
            Right = main.GRow[rightCol];

            TopLeft = (topRow != null) ? topRow[(main.IsEvenRow) ? leftCol : main.Column] : null;
            TopRight = (topRow != null) ? topRow[(main.IsEvenRow) ? main.Column : rightCol] : null;

            BottomLeft = (botRow != null) ? botRow[(main.IsEvenRow) ? leftCol : main.Column] : null;
            BottomRight = (botRow != null) ? botRow[(main.IsEvenRow) ? main.Column : rightCol] : null;

            FillArrays(id);
        }


        /// <summary>
        /// Create NeighBorns  Cells array
        /// </summary>
        /// <param name="main"></param>
        /// <param name="id"></param>
        public NeighBorns(GridCell main)
        {
            Main = main;
            int leftCol = main.Column - 1;
            int rightCol = main.Column + 1;
            Row<GridCell> botRow = (main.Row + 1 < main.rows.Count) ? main.rows[main.Row + 1] : null;
            Row<GridCell> topRow = (main.Row - 1 >= 0) ? main.rows[main.Row - 1] : null;

            Left = main.GRow[leftCol];
            Right = main.GRow[rightCol];

            TopLeft = (topRow != null) ? topRow[(main.IsEvenRow) ? leftCol : main.Column] : null;
            TopRight = (topRow != null) ? topRow[(main.IsEvenRow) ? main.Column : rightCol] : null;

            BottomLeft = (botRow != null) ? botRow[(main.IsEvenRow) ? leftCol : main.Column] : null;
            BottomRight = (botRow != null) ? botRow[(main.IsEvenRow) ? main.Column : rightCol] : null;

            Cells = new List<GridCell>();
            if (Left != null) Cells.Add(Left);
            if (Right != null) Cells.Add(Right);
            if (TopLeft != null) Cells.Add(TopLeft);
            if (BottomLeft != null) Cells.Add(BottomLeft);
            if (TopRight != null) Cells.Add(TopRight);
            if (BottomRight != null) Cells.Add(BottomRight);

            FillArrays();
        }

        private void FillArrays(int id)
        {
            Cells = new List<GridCell>();
            if (Left != null) Cells.Add(Left);
            if (Right != null) Cells.Add(Right);
            if (TopLeft != null) Cells.Add(TopLeft);
            if (BottomLeft != null) Cells.Add(BottomLeft);
            if (TopRight != null) Cells.Add(TopRight);
            if (BottomRight != null) Cells.Add(BottomRight);

            EmptyCells = new List<GridCell>();
            NotEmptyCells = new List<GridCell>();
            EqualIDCells = new List<GridCell>();
          
                for (int i = 0; i < Cells.Count; i++)
                {
                    GridCell c = Cells[i];
                    if (c)
                    {
                        if (!c.IsEmpty)
                        {
                            NotEmptyCells.Add(c);
                        }
                        else
                        {
                            EmptyCells.Add(c);
                        }

                        if ( c.Mainobject && (c.Mainobject.ID == id) )
                        {
                            EqualIDCells.Add(c);
                        }
                    }
                }
        }

        private void FillArrays()
        {
            Cells = new List<GridCell>();
            if (Left != null) Cells.Add(Left);
            if (Right != null) Cells.Add(Right);
            if (TopLeft != null) Cells.Add(TopLeft);
            if (BottomLeft != null) Cells.Add(BottomLeft);
            if (TopRight != null) Cells.Add(TopRight);
            if (BottomRight != null) Cells.Add(BottomRight);

            EmptyCells = new List<GridCell>();
            NotEmptyCells = new List<GridCell>();
            EqualIDCells = new List<GridCell>();

            for (int i = 0; i < Cells.Count; i++)
            {
                GridCell c = Cells[i];
                if (c)
                {
                    if (!c.IsEmpty)
                    {
                        NotEmptyCells.Add(c);
                    }
                    else
                    {
                        EmptyCells.Add(c);
                    }
                }
            }
        }

        public GridCell GetEmptyLeftBottom()
        {
            if (BottomLeft && BottomLeft.IsEmpty) return BottomLeft;
            if (Left && Left.IsEmpty) return Left;
            if (TopLeft && TopLeft.IsEmpty) return TopLeft;
            return null;
        }

        public GridCell GetEmptyLeftMiddle()
        {
            if (Left && Left.IsEmpty) return Left;
            if (BottomLeft && BottomLeft.IsEmpty) return BottomLeft;
            if (TopLeft && TopLeft.IsEmpty) return TopLeft;
            return null;
        }

        public GridCell GetEmptyLefTop()
        {
            if (TopLeft && TopLeft.IsEmpty) return TopLeft;
            if (Left && Left.IsEmpty) return Left;
            if (BottomLeft && BottomLeft.IsEmpty) return BottomLeft;
            return null;
        }

        public GridCell GetEmptyRightBottom()
        {
            if (BottomRight && BottomRight.IsEmpty) return BottomRight;
            if (Right && Right.IsEmpty) return Right;
            if (TopRight && TopRight.IsEmpty) return TopRight;
            return null;
        }

        public GridCell GetEmptyRightMiddle()
        {
            if (Right && Right.IsEmpty) return Right;
            if (BottomRight && BottomRight.IsEmpty) return BottomRight;
            if (TopRight && TopRight.IsEmpty) return TopRight;
            return null;
        }

        public GridCell GetEmptyRightTop()
        {
            if (TopRight && TopRight.IsEmpty) return TopRight;
            if (Right && Right.IsEmpty) return Right;
            if (BottomRight && BottomRight.IsEmpty) return BottomRight;
            return null;
        }

        public GridCell GetEmpty(bool left, bool bottom)
        {
            if (left)
            {
                if (bottom)
                    return GetEmptyLeftBottom();
                else
                    return GetEmptyLeftMiddle();
            }
            else
            {
                if (bottom)
                    return GetEmptyRightBottom();
                else
                    return GetEmptyRightMiddle();
            }
        }

        public override string ToString()
        {
            return ("All cells : "+ToString(Cells) + " ;Empty cells: " + ToString(EmptyCells) + " ; Not Empty : " + ToString(NotEmptyCells));
        }

        public string ToString( List<GridCell>list) 
        {
            string res = "";
            foreach (var item in list)
            {
                res += item.ToString();
            }
            return res;
        }
    }
}