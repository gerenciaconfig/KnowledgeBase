using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
namespace Mkey
{
    public class TouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler, IDropHandler, IPointerExitHandler
    {
        private List<Collider2D> hitList;
        private List<Collider2D> newHitList;
        private TouchPadEventArgs tpea;
        private static bool touched = false;
        private static bool isActive = true;
        private int pointerID;
        private Vector2 screenTouchPos;
        private Vector2 oldPosition;
        public static TouchPad Instance;

        /// <summary>
        /// Return true if touchpad is touched with mouse or finger
        /// </summary>
        public static bool IsTouched
        {
            get
            {
                return touched;
            }
        }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            hitList = new List<Collider2D>();
            newHitList = new List<Collider2D>();
        }

        #region callbacks
        public void OnPointerDown(PointerEventData data)
        {
            if (isActive)
            {
                if (!touched)
                {
                    touched = true;
                    tpea = new TouchPadEventArgs();
                    screenTouchPos = data.position;
                    oldPosition = screenTouchPos;
                    pointerID = data.pointerId;

                    tpea.SetTouch(screenTouchPos, Vector2.zero, TouchPhase.Began);
                    hitList.Clear();
                    hitList.AddRange(tpea.hits);
                    if (hitList.Count > 0)
                    {
                        for (int i = 0; i < hitList.Count; i++)
                        {
                            ExecuteEvents.Execute<ICustomMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.PointerDown(tpea));
                           // if (tpea.firstSelected == null) tpea.firstSelected = hitList[i].transform.gameObject.GetInterface<ICustomMessageTarget>();
                        }
                    }
                }
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (isActive)
            {
                if (data.pointerId == pointerID)
                {
                    screenTouchPos = data.position;

                    tpea.SetTouch(screenTouchPos, screenTouchPos - oldPosition, TouchPhase.Moved);
                    oldPosition = screenTouchPos;

                    newHitList = new List<Collider2D>(tpea.hits); // garbage

                    //0 ---------------------------------- send drag begin message --------------------------------------------------
                    for (int i = 0; i < hitList.Count; i++)
                    {
                        if (hitList[i]) ExecuteEvents.Execute<ICustomMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.DragBegin(tpea));
                    }
                }
                hitList = newHitList;
                // Debug.Log("stp drag begin");
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (isActive)
            {
                if (data.pointerId == pointerID)
                {
                    screenTouchPos = data.position;

                    tpea.SetTouch(screenTouchPos, screenTouchPos - oldPosition, TouchPhase.Moved);
                    oldPosition = screenTouchPos;

                    newHitList = new List<Collider2D>(tpea.hits); // garbage

                    //1 ------------------ send drag exit message and drag message --------------------------------------------------
                    foreach (Collider2D cHit in hitList)
                    {
                        if (newHitList.IndexOf(cHit) == -1)
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragExit(tpea));
                        }
                        else
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.Drag(tpea));
                        }

                    }

                    //2 ------------------ send drag enter message -----------------------------------------------------------------
                    for (int i = 0; i < newHitList.Count; i++)
                    {
                        if (hitList.IndexOf(newHitList[i]) == -1)
                        {
                            if (newHitList[i]) ExecuteEvents.Execute<ICustomMessageTarget>(newHitList[i].gameObject, null, (x, y) => x.DragEnter(tpea));
                        }
                    }

                    hitList = newHitList;
                }
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (isActive)
            {
                if (data.pointerId == pointerID)
                {
                    screenTouchPos = data.position;
                    tpea.SetTouch(screenTouchPos, screenTouchPos - oldPosition, TouchPhase.Ended);
                    oldPosition = screenTouchPos;

                    touched = false;
                    foreach (Collider2D cHit in hitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }
                    hitList.Clear();
                    newHitList = new List<Collider2D>(tpea.hits);
                    foreach (Collider2D cHit in newHitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragDrop(tpea));
                    }
                    newHitList.Clear();
                }
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (isActive)
            {
                if (data.pointerId == pointerID)
                {
                    screenTouchPos = data.position;
                    tpea.SetTouch(screenTouchPos, screenTouchPos - oldPosition, TouchPhase.Ended);
                    oldPosition = screenTouchPos;

                    touched = false;
                    foreach (Collider2D cHit in hitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }
                    hitList.Clear();
                    newHitList = new List<Collider2D>(tpea.hits);
                    foreach (Collider2D cHit in newHitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragDrop(tpea));
                    }
                    newHitList.Clear();
                }
            }
        }

        public void OnDrop(PointerEventData data)
        {
            if (isActive)
            {
                if (data.pointerId == pointerID)
                {

                }
            }

        }
        #endregion callbacks

        /// <summary>
        /// Return world position of touch.
        /// </summary>
        public Vector3 GetWorldTouchPos()
        {
            return Camera.main.ScreenToWorldPoint(screenTouchPos);
        }

        /// <summary>
        /// Return world position of touch.
        /// </summary>
        public Vector2 GetScreenTouchPos()
        {
            return screenTouchPos;
        }

        /// <summary>
        /// Enable or disable touch pad callbacks handling.
        /// </summary>
        internal static void SetTouchActivity(bool activity)
        {
            isActive = activity;
        }

    }
}
