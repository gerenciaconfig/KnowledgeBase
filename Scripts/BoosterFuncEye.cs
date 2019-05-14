using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BoosterFuncEye : BoosterFunc
    {
        /*
         Description:

         use regular object as shootbubble
         show free greedcell target selector 
         */

        private int mainID; // save main id from shootbubble

        public override void InitStart(ShootBubble hiddenShootBubble , ref bool destroyHiddenAfterShoot)
        {
            //use activeShootBubble to create booster
            GameObject g = new GameObject();
            SpriteRenderer sR = g.AddComponent<SpriteRenderer>();

            sR.sprite = hiddenShootBubble.sprite;
            sR.sortingOrder = hiddenShootBubble.GetComponent<SpriteRenderer>().sortingOrder;
            g.transform.localScale = transform.lossyScale;
            g.transform.position = transform.position;
            g.transform.parent = transform;
            GetComponent<SpriteRenderer>().sortingOrder += 1;
            destroyHiddenAfterShoot = true;
            mainID = hiddenShootBubble.MData.ID;
        }

        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            if(group.Length<2)  BubbleGrid.Instance.SetMainObject(freeGridCell, mainID);
            DestroyImmediate(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            return grid.GetIdArea(freeGridCell, mainID);
        }

        private void OnDestroy()
        {

        }

    }
}