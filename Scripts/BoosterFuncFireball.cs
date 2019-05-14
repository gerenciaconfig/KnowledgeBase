using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BoosterFuncFireball : BoosterFunc
    {

        /*
        Description:
        
        collect verical column with hitTarget
        show target selector and shoot collection area
        */

        public override void InitStart(ShootBubble hiddenShootBubble, ref bool destroyHiddenAfterShoot)
        {
            destroyHiddenAfterShoot = true;
        }

        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            DestroyImmediate(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            return grid.GetColumnArea(hitGridCell);
        }

        private void OnDestroy()
        {

        }
    }
}