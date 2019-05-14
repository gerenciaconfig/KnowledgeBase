using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BoosterFuncMulticolor : BoosterFunc
    {

        public override void InitStart(ShootBubble hiddenShootBubble, ref bool destroyHiddenAfterShoot)
        {
            destroyHiddenAfterShoot = false;
        }

        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            DestroyImmediate(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
           
            return grid.GetIdArea(freeGridCell);
        }

        private void OnDestroy()
        {

        }

    }
}