using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mkey
{
    public enum Match {ById, NotMatch }
    [CreateAssetMenu]
    public class GameObjectsSet : BaseScriptable, ISerializationCallbackReceiver
    {
        public Sprite[] backGrounds;
        public Sprite selector;
        public GameObject gridCellPrefab;
        public GameObject shootBubblePrefab;
        [Tooltip("Used only for constructor")]
        public Sprite gridCellEmptySprite;

        [SerializeField]
        private List<MainObjectData> mainObjects;
        [SerializeField]
        [HideInInspector]
        private BaseObjectData emptyObject;

        [SerializeField]
        private List<OverlayObjectData> overlayObjects;
        [SerializeField]
        private List<BoosterObjectData> boosterObjects;

        private List<BaseObjectData> targetObjects;

        private List<MainObjectData> shootBubbles; // temporary store

        #region properties
        public IList<MainObjectData> MainObjects
        {
            get { return mainObjects.AsReadOnly(); }
        }

        public BaseObjectData Empty
        {
            get { return emptyObject; }
        }


        public IList<OverlayObjectData> OverlayObjects
        {
            get { return overlayObjects.AsReadOnly(); }
        }

        public IList<BoosterObjectData> BoosterObjects { get { return boosterObjects.AsReadOnly(); } }

        public IList<BaseObjectData> TargetObjects { get { CreateTargets();  return targetObjects.AsReadOnly(); } }

        public int RegularLength
        {
            get { return MainObjects.Count; }
        }
        #endregion properties

        #region serialization
        public void OnBeforeSerialize()
        {
            //  Debug.Log("before serialize ");
            // create empty object only for constructor, prevent multiple calls
            emptyObject = new BaseObjectData();
            emptyObject.ObjectImage = gridCellEmptySprite;
            Empty.Enumerate(0);

            // set ids for game objects
            EnumerateArray(mainObjects, 10);
            EnumerateArray(overlayObjects, 100000);//  EnumerateArray(underlayObjects, 200000);
            EnumerateArray(boosterObjects, 300000);
        }

        public void OnAfterDeserialize()
        {
            //   Debug.Log("deserialized ");
        }
        #endregion serialization

        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public BaseObjectData GetRefObject(int id)
        {
            if (id == 0) return Empty;

            foreach (var item in mainObjects)
            {
                if (id == item.ID) return item;
            }

            foreach (var item in overlayObjects)
            {
                if (id == item.ID) return item;
            }

            return null;
        }

        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public MainObjectData GetMainObject(int id)
        {
            foreach (var item in mainObjects)
            {
                if (id == item.ID) return item;
            }
            return null;
        }

        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public OverlayObjectData GetOverlayObject(int id)
        {
            foreach (var item in overlayObjects)
            {
                if (id == item.ID) return item;
            }
            Debug.Log("Get overlay: " + id);
            return null;
        }

        public bool ContainMainID(int id)
        {
            return ContainID(MainObjects, id);
        }

        public bool ContainBoosterID(int id)
        {
            return ContainID(BoosterObjects, id);
        }

        public bool ContainOverlayID(int id)
        {
            return ContainID(OverlayObjects, id);
        }

        public bool ContainTargetID(int id)
        {
            return ContainID(TargetObjects, id);
        }

        /// <summary>
        /// Returns random objects array.
        /// </summary>
        /// <returns>Reference to char array</returns>
        public List<MainObjectData> GetMainRandomObjects(int count)
        {
            List<MainObjectData> r = new List<MainObjectData>(count);
            List<MainObjectData> source = mainObjects;

            for (int i = 0; i < count; i++)
            {
                int rndNumber = UnityEngine.Random.Range(0, source.Count);
                r.Add(source[rndNumber]);
            }
            return r;
        }

        /// <summary>
        /// Returns random MainObjectData array without "notInclude" list featured objects .
        /// </summary>
        public List<MainObjectData> GetMainRandomObjects(int count, List<BaseObjectData> notInclude)
        {
            List<MainObjectData> r = new List<MainObjectData>(count);
            List<MainObjectData> source = mainObjects;

            if (notInclude != null)
                for (int i = 0; i < notInclude.Count; i++)
                {
                    source.RemoveAll((mOD) => { return mOD.ID == notInclude[i].ID; });
                }

            for (int i = 0; i < count; i++)
            {
                int rndNumber = UnityEngine.Random.Range(0, source.Count);
                r.Add(source[rndNumber]);
            }
            return r;
        }

        /// <summary>
        /// Returns random MainObjectData array only shoot bubbles.
        /// </summary>
        public MainObjectData GetMainRandomObjectsShootBubble()
        {
            if (shootBubbles == null)
            {
                shootBubbles = new List<MainObjectData>(mainObjects.Count);
                foreach (var item in mainObjects)
                {
                    if (item.canUseAsShootBubbles)
                    {
                        shootBubbles.Add(item);
                    }
                }
            }

            int rndNumber = UnityEngine.Random.Range(0, shootBubbles.Count);
            return shootBubbles[rndNumber];
        }

        /// <summary>
        /// Returns MainObjectData array only shoot bubbles.
        /// </summary>
        public List<MainObjectData> GetMainObjectsShootBubbles()
        {
            if (shootBubbles == null)
            {
                shootBubbles = new List<MainObjectData>(mainObjects.Count);
                foreach (var item in mainObjects)
                {
                    if (item.canUseAsShootBubbles)
                    {
                        shootBubbles.Add(item);
                    }
                }
            }
            return shootBubbles;
        }

        public Sprite BackGround(int index)
        {
            index = Mathf.Clamp(index, 0, backGrounds.Length - 1);
            return backGrounds[index];
        }

        public int BackGroundsCount
        {
            get { return backGrounds.Length; }
        }

        public Sprite RegularCellSprite
        {
            get { return gridCellPrefab.GetComponent<SpriteRenderer>().sprite; }
        }

        public Sprite BlockedCellSprite
        {
            get { return Empty.ObjectImage; }
        }

        private void CreateTargets()
        {
            targetObjects = new List<BaseObjectData>();
            foreach (var item in overlayObjects)
            {
                if (item.canUseAsTarget) targetObjects.Add(item);
            }
        }

        internal static bool IsEmptyObject(int id)
        {
            return id == 0;
        }

        #region utils
        private void EnumerateArray<T>(ICollection<T> a, int startIndex) where T : BaseObjectData
        {
            if (a != null && a.Count > 0)
            {
                foreach (var item in a)
                {
                    item.Enumerate(startIndex++);
                }
            }
        }

        private bool ContainID<T>(ICollection<T> a, int id) where T : BaseObjectData
        {
            if (a == null || a.Count == 0) return false;
            foreach (var item in a)
            {
                if (item.ID == id) return true;
            }
            return false;
        }
        #endregion utils
    }

    [Serializable]
    public class MainObjectData : BaseObjectData
    {
        public GameObject guiTargetPrefab;
        public GameObject iddleAnimPrefab;
        public GameObject hitAnimPrefab;
        public GameObject scoreFlyerPrefab;
        public GameObject collectAnimPrefab;

        [Tooltip("Show and add score when collecting balls")]
        [SerializeField]
        private bool withScore;
        [Tooltip("This object will be used as shootBubble")]
        public bool canUseAsShootBubbles;

        [Space(8)]
        [Header("Match properties")]
        #region level object properties
        public bool isDestroyable;
        public Sprite[] protectionStateImages;
        [Tooltip("Object behavior in shoot area.")]
        public Match match;
        #endregion level object properties
        public bool WithScore
        {
            get { return withScore; }
            private set { withScore = value; }
        }
    }

    [Serializable]
    public class BoosterObjectData : BaseObjectData
    {
        public BoosterFunc prefab;
    }

    [Serializable]
    public class OverlayObjectData : BaseObjectData
    {
        public ObjectBehavior behaviorPrefab;
       // public Sprite[] protectionStateImages;
        public GameObject guiTargetPrefab;
        public GameObject iddleAnimPrefab;
        public GameObject hitAnimPrefab;
        [Tooltip("Show and add score when collecting balls")]
        [SerializeField]
        private bool withScore;
        public bool WithScore
        {
            get { return withScore; }
            private set { withScore = value; }
        }
        public GameObject collectAnimPrefab;
        [Space(8)]
        [Tooltip("Only if you collect it. In this version we can use only one object to collect")]
        [Header("Construct properties")]
        #region construct object properties
        public bool canUseAsTarget;
        #endregion construct object properties

    }

    [Serializable]
    public class BaseObjectData
    {
        [HideInInspector]
        [SerializeField]
        private string name;

        [HideInInspector]
        [SerializeField]
        private int id;

        [Space(8)]
        [Tooltip ("Picture that is used on bubble board")]
        [Header("Sprites")]
        public Sprite ObjectImage;
        [Tooltip("Picture that is used on gui")]
        public Sprite GuiImage;
        [Tooltip("Picture that is used on gui")]
        public Sprite GuiImageHover;
        [Tooltip("Picture that is used on gui")]
        public Sprite IconSprite;



        [Space(8)]
        [Header("Addit properties")]

        public AudioClip privateClip;

        #region properties
        public int ID
        {
            get { return id; }
            private set { id = value; }
        }
        public string Name
        {
            get { return name; }
        }
        #endregion properties

        public void Enumerate(int id)
        {
            this.id = id;
            name = (ObjectImage == null) ? "null" + "-" + id : ObjectImage.name + "; ID : " + id;
        }

        public bool IsEmpty()
        {
            return ID == 0;
        }
    }

    [Serializable]
    public class CellData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private int row;
        [SerializeField]
        private int column;

        public int ID { get { return id; } }
        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public CellData(int row, int column)
        {
            this.row = row;
            this.column = column;
            id = 0;
        }

        public CellData(int id, int row, int column)
        {
            this.row = row;
            this.column = column;
            this.id = id;
        }
    }

    /// <summary>
    /// Helper serializable class object with the equal ID
    /// </summary>
    [Serializable]
    public class ObjectsSetData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private int count;

        public int ID { get { return id; } }
        public int Count { get { return count; } }

        public ObjectsSetData(int id, int count)
        {
            this.id = id;
            this.count = count;
        }

        public Sprite GetImage(GameObjectsSet mSet)
        {
            return mSet.GetMainObject(id).GuiImage;
        }

        public void IncCount()
        {
            count += 1;
        }

        public void DecCount()
        {
            count -= 1;
        }

        public void SetCount(int newCount)
        {
            count = newCount;
        }
    }

    /// <summary>
    /// Helper class that contains list of object set data 
    /// </summary>
    [Serializable]
    public class ObjectSetCollection
    {
        [SerializeField]
        private List<ObjectsSetData> list;

        public IList<ObjectsSetData> ObjectsList { get { return list.AsReadOnly(); } }

        public ObjectSetCollection()
        {
            list = new List<ObjectsSetData>();
        }

        public uint Count
        {
            get { return(list == null) ? 0 : (uint)list.Count; }
        }

        public void AddById(int id, int count)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(list[i].Count + count);
                    return;
                }
            }
            list.Add(new ObjectsSetData(id, count));
        }

        public void RemoveById(int id, int count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(list[i].Count - count);
                    return;
                }
            }
        }

        public void SetCountById(int id, int count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(count);
                    return;
                }
            }
            list.Add(new ObjectsSetData(id, count));
        }

        public void CleanById(int id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        public int CountByID(int id) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                    return  list[i].Count;
            }
            return 0;
        }

        public bool ContainObjectID(int id)
        {
            return CountByID(id)>0;
        }

    }
}

