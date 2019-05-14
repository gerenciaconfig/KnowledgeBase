using System.Collections.Generic;
using System.Threading;

namespace Mkey
{
    public class PathFinder
    {
        private List<PFCell> fullPath = new List<PFCell>();
        public IList<PFCell> FullPath { get { return (fullPath == null) ?null: fullPath.AsReadOnly(); } }
        private int pathWidth = 2; //additional left cell along moving direction must be available

        /// <summary>
        /// Create all possible paths from this position
        /// </summary>
        /// <param name="A"></param>
        private void CreateGlobWayMap(Map WorkMap, PFCell A)
        {
           // UnityEngine.Debug.Log("create path to top ");
            WorkMap.ResetPath();
            List<PFCell> waveArray = new List<PFCell>();
            waveArray.Add(A);
            A.mather = A;

            bool work = true;
            while (work)
            {
                work = false;
                List<PFCell> waveArrayTemp = new List<PFCell>();
                foreach (PFCell mather in waveArray)
                {
                    if (mather.available || (A == mather && !mather.available))
                    {
                        List<PFCell> childrens = mather.neighBorns;
                        foreach (PFCell child in childrens)
                        {
                            if (!child.HaveMather() && child.available  && child.IsPassabilityFrom(mather)) /// try
                            {
                                child.mather = mather;
                                waveArrayTemp.Add(child);
                                work = true;
                            }
                        }
                    }
                }
                waveArray = waveArrayTemp;
            }
        }

        /// <summary>
        /// Create all possible paths to destination point
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        private void CreateGlobWayMap(Map WorkMap, PFCell A, PFCell B)
        {
            WorkMap.ResetPath();
            List<PFCell> waveArray = new List<PFCell>();
            waveArray.Add(A);
            A.mather = A;
            bool work = true;

            while (work)
            {
                work = false;
                List<PFCell> waveArrayTemp = new List<PFCell>();
                foreach (PFCell mather in waveArray)
                {
                    if (mather.available)
                    {
                        List<PFCell> childrens = mather.neighBorns;
                        foreach (PFCell child in childrens)
                        {
                            if (!child.HaveMather())
                            {
                                child.mather = mather;
                                waveArrayTemp.Add(child);
                                work = true;
                                if (child == B) return;
                            }
                        }
                    }
                }
                waveArray = waveArrayTemp;
            }
        }

        /// <summary>
        /// Return true if FullPathA contain start point and end point
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private bool IsWayCreated(PFCell A, PFCell B)
        {
            return (PathLenght > 0 && fullPath[0] == A && fullPath[PathLenght - 1] == B);
        }

        public void CreatePath(Map WorkMap, PFCell A, PFCell B)
        {
            fullPath = null;
            if (WorkMap == null || A == null || B == null  || !A.available|| !B.available) return;
            if (!IsWayCreated(A, B))
            {
                CreateGlobWayMap(WorkMap, A, B);
                if (IsWayExistTo(B))
                {
                    fullPath = new List<PFCell>();
                    fullPath.Add(B);
                    PFCell mather = B.mather;
                    while (mather != A.mather)
                    {
                        fullPath.Add(mather);
                        mather = mather.mather;
                    }
                    fullPath.Reverse();
                }
                else
                {
                    fullPath.Add(A);
                }
            }
        }

        /// <summary>
        /// Create the shortest path if exist, else fullPath set to null
        /// </summary>
        /// <param name="WorkMap"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public void CreatePath(Map WorkMap, PFCell A, List<PFCell> B)
        {
            fullPath = null;
            if (WorkMap == null || A ==null || B == null || B.Count == 0 || !A.available) return;

            List<PFCell> tempPath;
            CreateGlobWayMap(WorkMap, A);
            foreach (var item in B)
            {
                if (item.available)
                {
                    if (IsWayExistTo(item))
                    {
                        tempPath = new List<PFCell>();
                        tempPath.Add(item);

                        PFCell mather = item.mather;
                        while (mather != A.mather)
                        {
                            tempPath.Add(mather);
                            mather = mather.mather;
                        }
                        tempPath.Reverse();
                        if (fullPath == null || fullPath.Count > tempPath.Count) fullPath = tempPath;
                    }
                }
            }
        }

        /// <summary>
        /// Create the shortest path if exist, else fullPath set to null
        /// </summary>
        /// <param name="WorkMap"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public void CreatePathToTop(Map WorkMap, PFCell A)
        {
            fullPath = null;
            if (WorkMap == null || A == null) return;

            List<PFCell> tempPath;
            CreateGlobWayMap(WorkMap, A);
            PFCell mather;
            List<PFCell> topAvailable = new List<PFCell>();
            int minRow = int.MaxValue;

            // get top available cells
            foreach (var item in WorkMap.PFCells)
            {
                if (IsWayExistTo(item))
                {
                    if (minRow >= item.row)
                    {
                        minRow = item.row;
                        topAvailable.Add(item);
                    }
                    else
                    {
                        break;
                    }
                }
            }
           // UnityEngine.Debug.Log("min row :" + minRow);

            // create shortest path to top available cells
            foreach (var item in topAvailable)
            {
                if (item.row == minRow)
                {
                    tempPath = new List<PFCell>(topAvailable.Count);
                    tempPath.Add(item);

                    mather = item.mather;
                    while (mather != A.mather)
                    {
                        tempPath.Add(mather);
                        mather = mather.mather;
                    }
                    tempPath.Reverse();
                    if (fullPath == null || fullPath.Count > tempPath.Count) fullPath = tempPath;
                }
            }
          //  UnityEngine.Debug.Log("Path to top created " + DebugPath());
        }

        private void CreatePathThread(Map WorkMap, PFCell A, PFCell B)
        {
            ThreadPool.QueueUserWorkItem(m => CreatePath(WorkMap, A, B));
        }

        private bool IsWayExistTo(PFCell B)
        {
            return (B.HaveMather() && B.available); 
        }

        public int PathLenght { get { return fullPath.Count; } }

        public List<PFCell> GetAvailablePFPositionAround(Map WorkMap, PFCell A, int distance)
        {
            List<PFCell> lPos = new List<PFCell>();
            CreateGlobWayMap(WorkMap, A);
            foreach (var item in WorkMap.PFCells)
            {
                if(IsWayExistTo(item) && item.GetDistanceTo(A) == distance)
                {
                    lPos.Add(item);
                }
            } 
            return lPos;
        }

        public string DebugPath()
        {
            string debugString = "";
            if (fullPath != null)
            {
                foreach (var item in fullPath)
                {
                    if (item != null)
                    {
                        debugString += item.ToString();
                    }
                    else
                    {
                        debugString += "null";
                    }
                }
            }
            return debugString;
        }


    }
}
