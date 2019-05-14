using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Mkey
{
    [CreateAssetMenu]
    public class LevelConstructSet : BaseScriptable
    {
        public GameObjectsSet mSet;
        public int vertSize = 15;
        public int horSize = 11;
        [HideInInspector]
        [SerializeField]
        public float distX = 0.15f;
        [HideInInspector]
        [SerializeField]
        public float distY = 0.15f;
        [HideInInspector]
        [SerializeField]
        public float scale = 1.0f;
        public int backGroundNumber = 0;
 
        public int VertSize
        {
            get { return vertSize; }
            set
            {
                if (value < 1) value = 1;
                vertSize = value;
                Clean();
                SetAsDirty();
            }
        }

        public int HorSize
        {
            get { return horSize; }
            set
            {
                if (value < 1) value = 1;
                horSize = value;
                Clean();
                SetAsDirty();
            }
        }

        public float DistX
        {
            get { return distX; }
            set
            {
                distX = value;
                SetAsDirty();
            }
        }

        public float DistY
        {
            get { return distY; }
            set
            {
                distY = value;
                SetAsDirty();
            }
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                if (value < 0) value = 0;
                scale = value;
                SetAsDirty();
            }
        }

        public MissionConstruct levelMission;

        [SerializeField]
        public List<CellData> featuredCells;

        [SerializeField]
        public List<CellData> overlays;



        public GameObjectsSet Matchset
        {
            get { return mSet; }
            set { mSet = value; SetAsDirty(); }
        }

        #region regular
        void OnEnable()
        {
           // Debug.Log("onenable " + ToString());
            if (levelMission == null) levelMission = new MissionConstruct();
            levelMission.SaveEvent = SetAsDirty;

        }

        void Awake()
        {
           // Debug.Log("awake " + ToString());
            //if (levelMission == null) levelMission = new MissionConstruct();
            //levelMission.SaveEvent = SetAsDirty;
           
        }
        #endregion regular

        public void AddFeaturedCell(CellData cd)
        {
            if (featuredCells == null) featuredCells = new List<CellData>(1);
            AddCell(featuredCells, cd);
            if (cd!=null && GameObjectsSet.IsEmptyObject(cd.ID))
            {
                if (overlays == null) overlays = new List<CellData>(1);
                AddCell(overlays, cd);
            }
        }

        public void AddOverlay(CellData cd)
        {
            Debug.Log("add overlay ");
            if (overlays == null) overlays = new List<CellData>(1);
            AddCell(overlays, cd);
        }

        public void Clean()
        {
            CleanMainObjects(featuredCells);
            CleanOveralyObjects(overlays);
          
        }

        /// <summary>
        /// Remove cells with  (c.column >= horSize) && (c.row >= vertSize)
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void CleanMainObjects(List<CellData> list)
        {
            if (list != null && mSet!=null) list.RemoveAll((c) =>
            {
                  return (!BubbleGrid.ok(c.Row, c.Column, vertSize, horSize) || !mSet.ContainMainID(c.ID));
            });
        }

        /// <summary>
        /// Remove cells with  (c.column >= horSize) && (c.row >= vertSize)
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void CleanOveralyObjects(List<CellData> list)
        {
            if (list != null && mSet != null) list.RemoveAll((c) => { return !BubbleGrid.ok(c.Row, c.Column, vertSize, horSize) || !mSet.ContainOverlayID(c.ID); });
        }

        /// <summary>
        /// Remove 
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void CleanShootBubbles(List<int> list)
        {
            if (list != null && mSet != null) list.RemoveAll((c) => { return !mSet.ContainMainID(c); });
        }

        private void AddCell(List<CellData> list, CellData cd)
        {
            if (list.Count > 0)
            {
                list.RemoveAll((c) => { return ((cd.Column == c.Column) && (cd.Row == c.Row)); });
            }
            if (!GameObjectsSet.IsEmptyObject(cd.ID))
            {
                list.Add(cd);
            }
            else
            {
                Debug.Log("empty");
            }

            SetAsDirty();
            if (list == featuredCells) Debug.Log(featuredCells.Count);
        }

        public void IncBackGround()
        {
            backGroundNumber++;
            if (backGroundNumber >= mSet.BackGroundsCount || backGroundNumber < 0) backGroundNumber = 0;
            SetAsDirty();
        }

        public void DecBackGround()
        {
            backGroundNumber--;
            if (backGroundNumber >= mSet.BackGroundsCount) backGroundNumber = 0;
            else if (backGroundNumber < 0) backGroundNumber = mSet.BackGroundsCount - 1;
            SetAsDirty();
        }

        public Sprite BackGround
        {
            get { return mSet.BackGround(backGroundNumber); }
        }

       
    }
}
