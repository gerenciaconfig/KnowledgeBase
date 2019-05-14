using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BoosterFunc : MonoBehaviour
    {
        protected BoosterObjectData BData { get; set; }

        public virtual void InitStart (ShootBubble hiddenShootBubble, ref bool destroyHiddenAfterShoot)
        {
            Debug.Log("base init start");
        }

        public virtual void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            Debug.Log("base apply booster");
        }

        public virtual CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            Debug.Log("base get shoot area");
            CellsGroup cG = new CellsGroup();
            return cG;
        }

    }
}