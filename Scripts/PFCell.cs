using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    [System.Serializable]
    public class PFCell
    {

        public bool available; 
        public PFCell mather;  //public byte openClose = 0; public int fCost = 0;public int gCost = 0; public int hCost = 0;public int stepNumber = 0;
        public int row;
        public int col;
        public List<PFCell> neighBorns;
        public GridCell gCell { get; private set; }
        public PFCell Left { get; private set; }
        public PFCell Right { get; private set; }
        public PFCell TopLeft { get; private set; }
        public PFCell TopRight { get; private set; }
        public PFCell BottomLeft { get; private set; }
        public PFCell BottomRight { get; private set; }
     

        public PFCell(GridCell gCell)
        {
            this.gCell = gCell;
            mather = null;
            row = gCell.Row;
            col = gCell.Column;
        }

        public void CreateNeighBorns()
        {
            if (gCell == null) return;
            NeighBorns nBs = new NeighBorns(gCell); // gridcell neighborns
            neighBorns = new List<PFCell>(nBs.Cells.Count);

            foreach (var n in nBs.Cells)
            {
                neighBorns.Add(n.pfCell);
            }

            Left = (nBs.Left) ? nBs.Left.pfCell : null;
            Right = (nBs.Right) ? nBs.Right.pfCell : null;
            TopLeft = (nBs.TopLeft) ? nBs.TopLeft.pfCell : null;
            TopRight = (nBs.TopRight) ? nBs.TopRight.pfCell : null;
            BottomLeft = (nBs.BottomLeft) ? nBs.BottomLeft.pfCell : null;
            BottomRight = (nBs.BottomRight) ? nBs.BottomRight.pfCell : null;
        }

        public bool IsPassabilityFrom(PFCell a) // depends on width,  we use width = 1.5 for anchor, (half wave)
        {
            // min 2 neighborns isavailabe
            List<PFCell> availableNeighBorns = GetAvailableNeighBorns();
            if (availableNeighBorns.Count == 6) return true;


            foreach (var item in availableNeighBorns)
            {
                if (item.IsNeighBorn(a)) return true;
            }
            return false;
        }

        public List<PFCell> GetAvailableNeighBorns()
        {
            List<PFCell> availableNeighBorns = new List<PFCell>(neighBorns.Count);
            foreach (var item in neighBorns)
            {
                if (item.available)
                    availableNeighBorns.Add(item);
            }
            return availableNeighBorns;
        }

        /// <summary>
        /// Return mather != null
        /// </summary>
        /// <returns></returns>
        public bool HaveMather()
        {
            return mather != null;
        }

        public int GetDistanceTo(PFCell other)
        {
            return Mathf.Abs(other.row - row) + Mathf.Abs(other.col - col);
        }

        public override string ToString()
        {
            string res = (available) ? "available; " : "not available; ";
            res += ((HaveMather()) ? "have mather; " : "not mather; ");
            if (gCell)
                res += gCell.ToString();

            else res+=" gcell null ";
            return res;
        }

        public bool IsNeighBorn(PFCell a)
        {
            foreach (var item in neighBorns)
            {
                if (item == a) return true;
            }
            return false;
        }
    }
}
