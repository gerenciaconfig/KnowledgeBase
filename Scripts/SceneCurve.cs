﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mkey
{
    [ExecuteInEditMode]
    public enum DisplayMode {None, Display, DisplaySelected }
    public class SceneCurve : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private CRCurve curve;
        [SerializeField]
        private DisplayMode displayMode = DisplayMode.Display;
        [SerializeField]
        private Color color = Color.white;

        [SerializeField]
        [HideInInspector]
        private bool created = false;

        public bool Created { get { return created; } }

        [Space(8, order = 0)]
        [Header("Curve change event", order = 1)]
        public UnityEvent ChangeCurveEvent;

        private void OnChangeSpline()
        {
            if (ChangeCurveEvent != null) ChangeCurveEvent.Invoke();
            //Debug.Log("On change");
        }

        /// <summary>
        /// Return handles count
        /// </summary>
        public int HandlesCount
        {
            get
            {
                if (!created) return 0;
                return curve.HandlesCount;
            }
        }

        /// <summary>
        /// Return path length
        /// </summary>
        public float Length
        {
            get
            {
                if (!created) return 0; 
                return curve.Length;
            }
        }

        #region regular
        void Awake()
        {
            if (!Created) SetInitial();
        }

        private void OnDrawGizmos()
        {
            if (displayMode == DisplayMode.Display)
            {
             //   Debug.Log("on  draw : " + created);
                Gizmos.color = color;
                if (created) curve.Display(transform);
            }
        }

        private void OnDrawGizmosSelected()
        {

            if (displayMode == DisplayMode.DisplaySelected)
            {
                Gizmos.color = color;
                if (created) curve.Display(transform);
            }
        }
        #endregion regular

        /// <summary>
        /// Create initial curve
        /// </summary>
        public void SetInitial()
        {
            Vector3[] points = null;
            RectTransform rT;
            if (rT= GetComponent<RectTransform>()) // if gui curve
            {
                Vector2 sd2 = rT.sizeDelta / 2;
                Vector2 sd6 = rT.sizeDelta / 6;
                points = new Vector3[4] {
            new Vector3(0f, -sd2.y, 0f),
            new Vector3(0f, -sd6.y, 0f),
            new Vector3(0f, sd6.y, 0f),
            new Vector3(0f, sd2.y, 0f)
        };
            }

            else
            {
                points = new Vector3[3] {Vector3.left, Vector3.down, Vector3.right };
            }

            curve = new CRCurve(new List<Vector3>(points));
            OnChangeSpline();
            created = true;
            Debug.Log("created" + created);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }

        public void RemovePoint(int selectedIndex)
        {
            if (curve == null) return;
            curve.RemovePoint(selectedIndex);
            OnChangeSpline();
        }

        public void AddPoint(int selectedIndex)
        {
            if (curve == null) return;
            curve.AddPoint(selectedIndex);
            OnChangeSpline();
        }

        public void ChangePoint(int selectedIndex, Vector3 position)
        {
            if (curve == null) return;
            curve.ChangePoint(selectedIndex,position);
            OnChangeSpline();
        }

        public void AddPoint(Vector3 position)
        {
            if (curve == null) return;
            curve.AddPoint(position);
            OnChangeSpline();
        }

        public Vector3 GetHandlePosition(int index)
        {
            return curve.GetHandlePosition(index);
        }

        public  void MoveAlongPath(GameObject gObject, Transform relativeTo, float time, float delay, EaseAnim easeAnim, Action completeCallBack)
        {
            SimpleTween.Value(gObject, 0, curve.Length, time).
                   SetOnUpdate((float d) =>
                   {
                       if (gObject) gObject.transform.position = relativeTo.TransformPoint(curve.GetPosition(d));

                   }).SetEase(easeAnim).SetDelay(delay).
                   AddCompleteCallBack(() =>
                   {
                       if (completeCallBack != null) completeCallBack();
                   });
        }
    }

    /// <summary>
    /// CatmullRom curve
    /// </summary>
    [System.Serializable]
    public class CRCurve : MCurve
    {
        public CRCurve(List<Vector3> handlesPositions):base(handlesPositions)
        {
            CreateControlPositons();
            CacheDistances();
        }

        #region override
        /// <summary>
        /// Return List of position on curve with equal distances inbetween
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override List<Vector3> GetPositions(int count)
        {
            List<Vector3> pos = new List<Vector3>();
            float delta = (Length) / (count - 1);
            for (int i = 0; i < count - 1; i++)
            {
                pos.Add(GetPosition(i * delta));
            }
            pos.Add(GetPosition(Length));
            return pos;
        }

        /// <summary>
        /// Return position on spline at distance
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public override Vector3 GetPosition(float dist)
        {
            int point = 0;
            int numPoints = distances.Length;
            float pathLength = Length;

            dist = (dist == pathLength) ? pathLength : Mathf.Repeat(dist, pathLength);

            while (point < distances.Length - 1 && distances[point] <= dist)
            {
                ++point;// Debug.Log("point: "+point + " ; dis: " + dist);
            }

            int p1n = ((point - 1) + numPoints) % numPoints;
            int p2n = point;

            float t = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

            int p0n = ((point - 2) + numPoints) % numPoints;
            int p3n = (point + 1) % numPoints;
            p2n = p2n % numPoints;

            Vector3 P0 = controlPositions[p0n];
            Vector3 P1 = controlPositions[p1n];
            Vector3 P2 = controlPositions[p2n];
            Vector3 P3 = controlPositions[p3n];
            return GetCatmullRomPosition(t, P0, P1, P2, P3);
        }

        public override void RemovePoint(int selectedIndex)
        {
            if (HandlesCount < 4) return;
            Debug.Log("Remove point: " + selectedIndex);
            handlesPositions.RemoveAt(selectedIndex);
            CreateControlPositons();
            CacheDistances(); 
        }

        public override void AddPoint(int selectedIndex)
        {
            base.AddPoint(selectedIndex);
            CreateControlPositons();
            CacheDistances();
        }

        public override void ChangePoint(int selectedIndex, Vector3 position)
        {
            base.ChangePoint(selectedIndex,position);
            CreateControlPositons();
            CacheDistances();
        }

        public override void AddPoint(Vector3 position)
        {
            base.AddPoint(position);
            CreateControlPositons();
            CacheDistances();
        }

        /// <summary>
        /// Create positions list for curve calculating
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private void CreateControlPositons()
        {
            controlPositions = new List<Vector3>();
            controlPositions.Add(handlesPositions[0]); // hidden duplicate 0 point
            handlesPositions.ForEach((p) => { controlPositions.Add(p); });
            controlPositions.Add(handlesPositions[HandlesCount - 1]); // hidden duplicate last point
        }

        public override void Display(Transform transform)
        {
            if (HandlesCount < 2) return;
            for (int i = 0; i < controlPositions.Count; i++)
            {
                if ((i == 0 || i == controlPositions.Count - 2 || i == controlPositions.Count - 1))
                {
                    continue;
                }
                Display(transform, i);
            }
        }
        #endregion override

        //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }

        private void Display(Transform transform, int pos)
        {
            Vector3 p0 = controlPositions[ClampListPos(pos - 1, controlPositions.Count)];
            Vector3 p1 = controlPositions[pos];
            Vector3 p2 = controlPositions[ClampListPos(pos + 1, controlPositions.Count)];
            Vector3 p3 = controlPositions[ClampListPos(pos + 2, controlPositions.Count)];

            Vector3 lastPos = p1;

            float resolution = 0.2f;

            int loops = Mathf.FloorToInt(1f / resolution);

            for (int i = 1; i <= loops; i++)
            {
                float t = i * resolution;
                Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
                Gizmos.DrawLine(transform.TransformPoint(lastPos), transform.TransformPoint(newPos));
                lastPos = newPos;
            }
            //Debug.Log("display: "+ HandlesCount);
        }

        private  void CacheDistances()
        {
            int count = controlPositions.Count;
            distances = new float[count];

            float accumulateDistance = 0;
            for (int i = 0; i < count; ++i)
            {
                var t1 = controlPositions[(i) % count];
                var t2 = controlPositions[(i + 1) % count];
                if (t1 != null && t2 != null)
                {
                    distances[i] = accumulateDistance;
                    accumulateDistance += (t1 - t2).magnitude;
                }
            }
        }
    }

    public class MultiLine : MCurve // polyline
    {
        public MultiLine(List<Vector3> handlesPositions) : base(handlesPositions)
        {
            CacheDistances();
        }

        public override void Display(Transform transform)
        {
            if (HandlesCount < 2) return;
            for (int i = 0; i < HandlesCount - 1; i++)
            {
                Gizmos.DrawLine(transform.TransformPoint(handlesPositions[i]), transform.TransformPoint(handlesPositions[i + 1]));
            }
        }

        private void CacheDistances()
        {
            int count = HandlesCount;
            distances = new float[count];

            float accumulateDistance = 0;
            for (int i = 0; i < count; ++i)
            {
                var t1 = handlesPositions[(i) % count];
                var t2 = handlesPositions[(i + 1) % count];
                if (t1 != null && t2 != null)
                {
                    distances[i] = accumulateDistance;
                    accumulateDistance += (t1 - t2).magnitude;
                }
            }
        }
    }

    /*
      public class LineCreator : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private bool curveCreated;
        [SerializeField]
        public List<Vector3> handlesPositions;
        [SerializeField]
        private bool useLineInfoColor = true;

        #region properties
        /// <summary>
        /// Return handles count
        /// </summary>
        public int HandlesCount
        {
            get
            {
                if (handlesPositions == null) return 0;
                return handlesPositions.Count;
            }
        }
        #endregion properties

        #region regular
        private void Start()
        {
            CreateInitialHandles();
        }
        #endregion regular

        public void OnChangeLine()
        {
            
        }

        public void RemovePoint(int selectedIndex)
        {
            Debug.Log("Remove point: " + selectedIndex);
            handlesPositions.RemoveAt(selectedIndex);
        }

        public void AddPoint(int selectedIndex)
        {
            Debug.Log("Add point: " + selectedIndex);
            Vector3 p0 = handlesPositions[selectedIndex];
            Vector3 p1 = handlesPositions[selectedIndex + 1];
            Vector3 pn = (p1 + p0) / 2.0f;
            handlesPositions.Insert(selectedIndex + 1, pn);
        }

        public void SetInitial()
        {
            curveCreated = false;
            CreateInitialHandles();
        }

        #region private methods
        private void CreateInitialHandles()
        {
            if (curveCreated) return;
            Debug.Log("create initial curve");
            LineBehavior lineBehavior = GetComponent<LineBehavior>();
            int rCount = 0;
            handlesPositions = new List<Vector3>();
            if (lineBehavior && lineBehavior.rayCasters!=null)
            {
                foreach (var item in lineBehavior.rayCasters)
                {
                    if (item)
                    {
                        handlesPositions.Add(transform.InverseTransformPoint(item.transform.position));
                        rCount++;
                    }
                }

                if (rCount >= 2)
                {
                    Vector3 cPoint = handlesPositions[0];
                    float dx = cPoint.x - handlesPositions[1].x;
                    handlesPositions.Insert(0, new Vector3(cPoint.x + 0.5f* dx, cPoint.y, cPoint.z));

                    cPoint = handlesPositions[handlesPositions.Count - 1];
                    handlesPositions.Add(new Vector3(cPoint.x - 0.5f * dx, cPoint.y, cPoint.z));
                }
                curveCreated = true;
            }
           // Debug.Log("Initial curve created:" + p0 + ":" + p1 + ":" + p2 + ":" + p3);
        }

        public void Display()
        {
            if (HandlesCount < 2) return;
            for (int i = 0; i < HandlesCount-1; i++)
            {
                Debug.DrawLine(transform.TransformPoint(handlesPositions[i]), transform.TransformPoint(handlesPositions[i+1]), Color.white);
            }
            Debug.Log("draw" + name);
        }

        public Color GetLineColor()
        {
            if (useLineInfoColor && GetComponent<LineBehavior>())
            {
                return GetComponent<LineBehavior>().lineInfoColor;
            }
            return Color.white;
        }
        #endregion private methods

    }
     */


    /// <summary>
    /// Common curve class
    /// </summary>
    [System.Serializable]
    public class MCurve
    {
        [Tooltip("Local positions of curve handles")]
        public List<Vector3> handlesPositions;

        [SerializeField]
        [HideInInspector]
        public List<Vector3> controlPositions; // cached intermediate positons (catjmull rom)

        [SerializeField]
        [HideInInspector]
        public float[] distances;

        #region properties
        /// <summary>
        /// Return path length
        /// </summary>
        public float Length
        {
            get
            {
                if (distances == null || distances.Length == 0) return 0;
                return distances[distances.Length - 1];
            }
        }

        public int HandlesCount
        {
            get { return (handlesPositions != null) ? handlesPositions.Count : 0; }
        }
        #endregion properties

        public MCurve(List<Vector3> handlesPositions)
        {
            this.handlesPositions = handlesPositions;
        }

        #region virtual
        public virtual void RemovePoint(int selectedIndex)
        {
            Debug.Log("Remove point: " + selectedIndex);
            handlesPositions.RemoveAt(selectedIndex);
        }

        public virtual void AddPoint(int selectedIndex)
        {
            Debug.Log("Add point: " + selectedIndex);
            Vector3 p0 = handlesPositions[selectedIndex];
            Vector3 p1 = handlesPositions[selectedIndex + 1];
            Vector3 pn = (p1 + p0) / 2.0f;
            handlesPositions.Insert(selectedIndex + 1, pn);
        }

        public virtual void ChangePoint(int selectedIndex, Vector3 position)
        {
            handlesPositions[selectedIndex] = position;
        }

        public virtual void AddPoint(Vector3 position)
        {
            handlesPositions.Add(position);
        }

        /// <summary>
        /// Get  local osition on curve at distance
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public virtual Vector3 GetPosition(float dist)
        {
            return Vector3.zero;
        }

        public virtual List<Vector3> GetPositions(int count)
        {
            return new List<Vector3>();
        }

        protected int ClampListPos(int pos, int count)
        {
            if (pos < 0)
            {
                pos = count - 1;
            }

            if (pos > count)
            {
                pos = 1;
            }
            else if (pos > count - 1)
            {
                pos = 0;
            }

            return pos;
        }

        public virtual void Display(Transform transform)
        {
            Debug.Log("base display");
        }
        #endregion virtual

        public Vector3 GetHandlePosition(int index)
        {
            return handlesPositions[index];
        }

    }
}