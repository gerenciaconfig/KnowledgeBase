using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey {
    public class ShootBubble : MonoBehaviour, ICustomMessageTarget
    {
        private bool debug = false;
        public MainObjectData MData { get; private set; }
       
        public Booster booster;
        public Sprite sprite { get; private set; }

        internal void SetData(MainObjectData mData, Booster booster, Material mat, int sortingOrder, bool enableCollider, Action onClick)
        {
            MData = mData;
            this.booster = booster;
            BoosterObjectData BData =(booster != null)? booster.bData : null;
            SpriteRenderer shootBubbleSR = GetComponent<SpriteRenderer>();
            shootBubbleSR.material = mat;
            shootBubbleSR.sprite = (MData!=null) ? MData.ObjectImage : BData.ObjectImage;
            sprite = shootBubbleSR.sprite;
            shootBubbleSR.sortingOrder = sortingOrder;
            CircleCollider2D cC = GetComponent<CircleCollider2D>();
            cC.enabled = enableCollider;
            cC.isTrigger = true; // avoid bouncing
            this.PointerDownDel += (tpe)=> { if (onClick != null) onClick(); };
        }

        internal void ActivateCollider(bool activate)
        {
            GetComponent<CircleCollider2D>().enabled = activate;
        }

        public  void ApplyToTarget(GridCell hitGCell, GridCell freeGCell, CellsGroup group)
        {
           if(debug) Debug.Log("apply");
        }

        #region  properties show selectors
        [SerializeField]
        private bool showHitTargetSelector = true;
        [SerializeField]
        private bool showFreeTargetSelector = true;
        [SerializeField]
        private bool showShootAreaSelector = true;
        #endregion show selectors

        #region  properties show selectors
        /// <summary>
        /// If has boosterfunc return boosterunc ShowHitTargetSelector else private
        /// </summary>
        public bool ShowHitTargetSelector { get {return showHitTargetSelector; } }
        /// <summary>
        /// If has boosterfunc return boosterunc ShowFreeTargetSelector else private
        /// </summary>
        public bool ShowFreeTargetSelector { get { return  showFreeTargetSelector; } }
        /// <summary>
        /// If has boosterfunc return boosterunc ShowShootAreaSelector else private
        /// </summary>
        public bool ShowShootAreaSelector { get { return showShootAreaSelector; } }
        #endregion properties

        /// <summary>
        /// Get shoot area around target free grid cell
        /// </summary>
        /// <param name="hitGCell"></param>
        /// <param name="freeGCell"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public CellsGroup GetShootArea(GridCell hitGCell, GridCell freeGCell, BubbleGrid grid)
        {
            if (debug) Debug.Log("shootbubble get shoot area");
            BoosterFunc bF = GetComponent<BoosterFunc>();
            if (bF)
            {
              return bF.GetShootArea(hitGCell, freeGCell, grid);
            }
            else
            {
                return grid.GetIdArea(freeGCell, MData.ID);
            }
          
        }

        public void ApplyShootBubbleToGrid(GridCell hitGridCell, GridCell freeGridCell, CellsGroup shootGridCellsArea)
        {
            if (debug) Debug.Log("apply shoot bubble");

            if (MData != null) // regular object shoot bubble, set new mainobject and destroy shootbubble
            {
              if (shootGridCellsArea.Length < 2)  BubbleGrid.Instance.SetMainObject(freeGridCell, MData);
                
                DestroyImmediate(gameObject);
            }
            else // possible booster
            {
                BoosterFunc bF = GetComponent<BoosterFunc>();
                if (bF)
                {
                    bF.ApplyBooster(hitGridCell, freeGridCell, shootGridCellsArea);
                }
            }

            if (hitGridCell) hitGridCell.ShootHit(null);
        }

        #region touch
        public Action<TouchPadEventArgs> PointerDownDel;
        public Action<TouchPadEventArgs> DragBeginDel;
        public Action<TouchPadEventArgs> DragEnterDel;
        public Action<TouchPadEventArgs> DragExitDel;
        public Action<TouchPadEventArgs> DragDropDel;
        public Action<TouchPadEventArgs> PointerUpDel;
        public Action<TouchPadEventArgs> DragDel;

        GameObject dataIcon;
        public void PointerDown(TouchPadEventArgs tpea)
        {
            if (PointerDownDel != null) PointerDownDel(tpea);
        }

        public void DragBegin(TouchPadEventArgs tpea)
        {
            if (DragBeginDel != null) DragBeginDel(tpea);
        }

        public void DragEnter(TouchPadEventArgs tpea)
        {
            if (DragEnterDel != null) DragEnterDel(tpea);
        }

        public void DragExit(TouchPadEventArgs tpea)
        {
            if (DragExitDel != null) DragExitDel(tpea);
        }

        public void DragDrop(TouchPadEventArgs tpea)
        {
            if (DragDropDel != null) DragDropDel(tpea);
        }

        public void PointerUp(TouchPadEventArgs tpea)
        {
            if (PointerUpDel != null) PointerUpDel(tpea);
        }

        public void Drag(TouchPadEventArgs tpea)
        {
            if (DragDel != null) DragDel(tpea);
        }

        public GameObject GetDataIcon()
        {
            return dataIcon;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        #endregion touch
    }
}
