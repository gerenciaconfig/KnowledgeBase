/*
 v1_1 (05062018)
 remove LeanTween
*/

using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class SpriteTrail : MonoBehaviour {

        public float minStep = 0.01f;
        public float maxLifeTime = 0.15f;

        private int order;
        private int layer;
        private SpriteRenderer srMain;
        private Color sourceColor;
        private TimeStack timeStack;
        List<GameObject> tempStack;

        void Start() {
            prevPos = transform.position;
            srMain = GetComponent<SpriteRenderer>();
            order = srMain.sortingOrder;
            layer = srMain.sortingLayerID;
            sourceColor = srMain.color;
            timeStack = new TimeStack(maxLifeTime + 0.1f, gameObject, (g) => { DestroyImmediate(g); });
            tempStack = new List<GameObject>();
        }

        void Update()
        {
            CreateTrail();
            timeStack.Update();
        }

        Vector3 prevPos;

        private void CreateTrail()
        {
            if (Vector3.SqrMagnitude(prevPos - transform.position) > minStep)
            {
                CreateGhost(transform.position, maxLifeTime);
                prevPos = transform.position;
            }
        }

        private void CreateGhost(Vector2 pos, float lifeTime)
        {
            GameObject temp;
            SpriteRenderer sr;
            if (timeStack.Length > 0)
            {
                temp = timeStack.GetAtPos(pos);
                sr = temp.GetComponent<SpriteRenderer>();
                sr.color = sourceColor;
            }
            else
            {
                temp = new GameObject();
#if UNITY_EDITOR
                temp.name = "trail";
#endif
                temp.transform.position = pos;
                temp.transform.localScale = transform.lossyScale;
                sr = temp.AddComponent<SpriteRenderer>();
                sr.sprite = srMain.sprite;
                sr.sortingOrder = order - 1;
                sr.sortingLayerID = layer;
            }
            tempStack.Add(temp);
        SimpleTween.Value(temp, 0.70f, 0.0f, lifeTime).SetEase(EaseAnim.EaseOutCubic). //tween alpha
            SetOnUpdate((float val) =>
            {
                sr.color = new Color(sourceColor.r, sourceColor.g, sourceColor.b, sourceColor.a * val);
            }).  // sr.color = new Color(sourceColor.r, sourceColor.g, sourceColor.b, sourceColor.a* (1 - val));sr.color = g.Evaluate(val);
            AddCompleteCallBack(() =>
            {
                timeStack.Add(temp);
                tempStack.Remove(temp);
            });
        }

        void OnDestroy()
        {
            tempStack.ForEach((g) => { DestroyImmediate(g); });
            timeStack.Destroy((gList) => { gList.ForEach((g) => { DestroyImmediate(g); }); });
        }
    }

    public class TimeStack
    {
        List<GameObject> todel;
        float lastTime;
        float saveTime;
        GameObject source;
        int order;
        System.Action<GameObject> destroy;

        public int Length
        {
            get { return todel.Count; }
        }

        public TimeStack(float saveTime, GameObject source, System.Action<GameObject> destroy)
        {
            todel = new List<GameObject>();
            this.saveTime = saveTime;
            this.source = source;
            this.destroy = destroy;

        }

        public void Add(GameObject g)
        {
            if (todel.Contains(g)) return;
            todel.Add(g);
            lastTime = Time.time;
        }

        public GameObject GetAtPos(Vector3 pos)
        {
            if (todel.Count > 0)
            {
                GameObject g = todel[0];
                todel.RemoveAt(0);
                g.transform.position = pos;
                return g;
            }
            return null;
        }

        public void Update()
        {
            if (source && todel.Count > 0 && ((Time.time - lastTime) > saveTime))
            {
                GameObject g = todel[0];
                todel.RemoveAt(0);
                if (destroy != null) destroy(g);
            }
        }

        public void Destroy(System.Action<List<GameObject>> DestroyAction)
        {
            if (DestroyAction != null) DestroyAction(todel);
        }
    }
}