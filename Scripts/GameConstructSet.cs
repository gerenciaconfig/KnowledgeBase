using UnityEngine;
using System;
using System.Collections.Generic;
using Mkey;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mkey
{
    [CreateAssetMenu]
    public class GameConstructSet : BaseScriptable
    {
        [Space(8, order = 0)]
        [Header("Constructed Levels", order = 1)]
        public List<LevelConstructSet> levelSets;

        [Space(8, order = 0)]
        [Header("Daily Gift", order = 1)]
        [HideInInspector]
        [SerializeField]
        public GiftConstruct gift;

        public void AddLevel(LevelConstructSet lc)
        {
            Clean();
            levelSets.Add(lc);
            SetAsDirty();
        }

        /// <summary>
        /// Return LevelConstructSet for levelNumber. If levelNumber out of range - return LevelConstruct for 1 levelNumber.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public LevelConstructSet GetLevelConstructSet(int level)
        {
            if (InRange(level)) return levelSets[level];
            else if (levelSets != null) return levelSets[levelSets.Count - 1];
            return null;
        }

        public int LevelCount
        {
            get { if (levelSets != null) return levelSets.Count; else return 0; }
        }

        #region regular
        private void OnEnable()
        {
            //Debug.Log("onenable and clean: " + name);
            //if (gift == null) gift = new GiftConstruct();
            //gift.SaveEvent = SetAsDirty;
        }
        #endregion regular

        public void Clean()
        {
            bool needClean = false;
            if (levelSets == null) { levelSets = new List<LevelConstructSet>(); needClean = true; };

            if (!needClean)
                foreach (var item in levelSets)
                {
                    if (item == null)
                    {
                        needClean = true;
                        break;
                    }
                }

            if (needClean)
            {
                levelSets = levelSets.Where(item => item != null).ToList();
                SetAsDirty();
            }
        }

        public void InsertBeforeLevel(int levelIndex, LevelConstructSet lcs)
        {
            Clean();
            levelSets.Insert(levelIndex, lcs);
            SetAsDirty();
        }

        public void InsertAfterLevel(int levelIndex, LevelConstructSet lcs)
        {
            Clean();
            levelSets.Insert(levelIndex + 1, lcs);
            SetAsDirty();
        }

        public void RemoveLevel(int levelIndex)
        {
            Clean();
#if UNITY_EDITOR
            ScriptableObjectUtility.DeleteResourceAsset(levelSets[levelIndex]);
#endif
            levelSets.RemoveAt(levelIndex);
            SetAsDirty();
        }

        private bool InRange(int level)
        {
            return (levelSets != null && levelSets.Count > level && level >= 0);
        }
    }
}

