using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey {
    public class Creator : MonoBehaviour {

        public static Creator Instance;

        void Awake()
        {
            if (Instance) Destroy(gameObject);
            else { Instance = this; }
        }

        /// <summary>
        /// Instantiate prefab at position, set parent, parent lossyScale, and if (destroyTime>0) destroy result gameobject after destroytime.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="destroyTime"></param>
        internal static GameObject InstantiatePrefabAtPosition(GameObject prefab, Transform parent, Vector3 position, float destroyTime)
        {
            if (!prefab) return null;
            GameObject g = Instantiate(prefab, position, Quaternion.identity);
            if (parent)
            {
                g.transform.localScale = parent.lossyScale;
                g.transform.parent = parent;
            }
            if (destroyTime > 0) Destroy(g, destroyTime);
            return g;
        }

        /// <summary>
        /// Instantiate sprite anim prefab at position, set parent, parent lossyScale, setOrder
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="destroyTime"></param>
        internal static GameObject InstantiateAnimPrefabAtPosition(GameObject prefab, Transform parent, Vector3 position, int sortingOrder, bool destroy, Action completeCallback)
        {
            if (!prefab) return null;
            GameObject g = Instantiate(prefab, position, Quaternion.identity);
            if (parent)
            {
                g.transform.localScale = parent.lossyScale;
                g.transform.parent = parent;
            }
            SpriteRenderer srDot = g.GetComponent<SpriteRenderer>();
            srDot.sortingOrder = sortingOrder;

            AnimCallBack aC = g.GetComponent<AnimCallBack>();
            if (aC)
            {
                aC.SetEndCallBack( ()=> {if(destroy) Destroy(g); if (completeCallback != null) completeCallback(); });
            }
            return g;
        }



        /// <summary>
        /// Instantiate new 3D Sprite at position, and set parent (if parent !=null), set scale like parent lossyScale, set sortingLayerID, sortingOrder
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="renderLayer"></param>
        /// <param name="renderOrder"></param>
        /// <returns></returns>
        internal static SpriteRenderer CreateSpriteAtPosition(Transform parent, Sprite sprite, Vector3 position, int sortingLayerID, int sortingOrder)
        {
            GameObject gO = new GameObject();

            if (parent)
            {
                gO.transform.localScale = parent.lossyScale;
                gO.transform.parent = parent;
            }

            gO.transform.position = position;
            SpriteRenderer srDot = gO.AddComponent<SpriteRenderer>();
            srDot.sprite = sprite;
            srDot.sortingLayerID = sortingLayerID;
            srDot.sortingOrder = sortingOrder;
            return srDot;
        }

        /// <summary>
        /// Instantiate new 3D Sprite at position, and set parent (if parent !=null), set scale like parent lossyScale, set sortingLayerID, sortingOrder
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="renderLayer"></param>
        /// <param name="renderOrder"></param>
        /// <returns></returns>
        internal static SpriteRenderer CreateSpriteAtPosition(Transform parent, Sprite sprite, Vector3 position, int sortingOrder)
        {
            GameObject gO = new GameObject();

            if (parent)
            {
                gO.transform.localScale = parent.lossyScale;
                gO.transform.parent = parent;
            }

            gO.transform.position = position;
            SpriteRenderer srDot = gO.AddComponent<SpriteRenderer>();
            srDot.sprite = sprite;
            srDot.sortingOrder = sortingOrder;
            return srDot;
        }

        /// <summary>
        /// Instantiate new 3D Sprite at position, and set parent (if parent !=null), set scale like parent lossyScale
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="renderLayer"></param>
        /// <param name="renderOrder"></param>
        /// <returns></returns>
        internal static SpriteRenderer CreateSpriteAtPosition(Transform parent, Sprite sprite, Vector3 position)
        {
            GameObject gO = new GameObject();

            if (parent)
            {
                gO.transform.localScale = parent.lossyScale;
                gO.transform.parent = parent;
            }

            gO.transform.position = position;
            SpriteRenderer srDot = gO.AddComponent<SpriteRenderer>();
            srDot.sprite = sprite;
            return srDot;
        }

        /// <summary>
        /// Instantiate new 3D Sprite at position, and set parent (if parent !=null), set scale like parent lossyScale, set sortingLayerID, sortingOrder
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="renderLayer"></param>
        /// <param name="renderOrder"></param>
        /// <returns></returns>
        internal static SpriteRenderer CreateSpriteAtPosition(Transform parent, Sprite sprite, Material material, Vector3 position, int sortingLayerID, int sortingOrder)
        {
            SpriteRenderer sr = CreateSpriteAtPosition(parent, sprite, position, sortingLayerID, sortingOrder);
            if (sr && material) sr.material = material;
            return sr;
        }
    }

}