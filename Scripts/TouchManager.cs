using UnityEngine;
using UnityEngine.EventSystems;
using System;


namespace Mkey
{
    public enum DirectionType { Right, Left, Top, Bottom }
    public class TouchManager : MonoBehaviour
    {

        public static TouchManager Instance;

        /// <summary>
        /// Return true if touchpad is touched with mouse or finger
        /// </summary>
        public static bool IsTouched
        {
            get
            {
                return TouchPad.IsTouched;
            }
        }

        void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Return true touch pad run on mobile device
        /// </summary>
        public static bool IsMobileDevice()
        {
            //check if our current system info equals a desktop
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                //we are on a desktop device, so don't use touch
                return false;
            }
            //if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                //we are on a mobile device, so lets use touch input
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enable or disable touch pad callbacks handling.
        /// </summary>
        internal static void SetTouchActivity(bool activity)
        {
            TouchPad.SetTouchActivity(activity);
        }

        public Action<TouchPadEventArgs> PointerDownDel;

        public Action<TouchPadEventArgs> DragBeginDel;

        public Action<TouchPadEventArgs> DragEnterDel;

        public Action<TouchPadEventArgs> DragExitDel;

        public Action<TouchPadEventArgs> DragDropDel;

        public Action<TouchPadEventArgs> PointerUpDel;

        public Action<TouchPadEventArgs> DragDel;

    }

    /// <summary>
    /// Interface for handling touchpad events.
    /// </summary>
    public interface ICustomMessageTarget : IEventSystemHandler
    {
        void PointerDown(TouchPadEventArgs tpea);
        void DragBegin(TouchPadEventArgs tpea);
        void DragEnter(TouchPadEventArgs tpea);
        void DragExit(TouchPadEventArgs tpea);
        void DragDrop(TouchPadEventArgs tpea);
        void PointerUp(TouchPadEventArgs tpea);
        void Drag(TouchPadEventArgs tpea);
        GameObject GetDataIcon();
        GameObject GetGameObject();
    }

    public class CustomMessageTarget : MonoBehaviour, ICustomMessageTarget
    {
        public Action <TouchPadEventArgs> PointerDownDel;
        public Action <TouchPadEventArgs> DragBeginDel;
        public Action <TouchPadEventArgs> DragEnterDel;
        public Action <TouchPadEventArgs> DragExitDel;
        public Action <TouchPadEventArgs> DragDropDel;
        public Action <TouchPadEventArgs> PointerUpDel;
        public Action <TouchPadEventArgs> DragDel;

        GameObject dataIcon;
        public void PointerDown(TouchPadEventArgs tpea)
        {
            if (PointerDownDel != null) PointerDownDel(tpea);
        }

        public void DragBegin(TouchPadEventArgs tpea)
        {
            if (DragBeginDel != null) DragBeginDel(tpea);
        }

        public void DragEnter(TouchPadEventArgs tpea)
        {
            if (DragEnterDel != null) DragEnterDel(tpea);
        }

        public void DragExit(TouchPadEventArgs tpea)
        {
            if (DragExitDel != null) DragExitDel(tpea);
        }

        public void DragDrop(TouchPadEventArgs tpea)
        {
            if (DragDropDel != null) DragDropDel(tpea);
        }

        public void PointerUp(TouchPadEventArgs tpea)
        {
            if (PointerUpDel != null) PointerUpDel(tpea);
        }

        public void Drag(TouchPadEventArgs tpea)
        {
            if (DragDel != null) DragDel(tpea);
        }

        public GameObject GetDataIcon()
        {
            return dataIcon;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

    }

    [Serializable]
    public class TouchPadEventArgs
    {
        /// <summary>
        /// First selected object.
        /// </summary>
        public ICustomMessageTarget firstSelected;
        /// <summary>
        /// The cast results.
        /// </summary>
        public Collider2D[] hits;
        /// <summary>
        /// Priority dragging direction.  (0,1) or (1,0)
        /// </summary>
        public Vector2 PriorAxe
        {
            get { return priorityAxe; }
        }
        /// <summary>
        /// Touch delta position in screen coordinats;
        /// </summary>
        public Vector2 DragDirection
        {
            get { return touchDeltaPosRaw; }
        }
        /// <summary>
        /// Last drag direction.
        /// </summary>
        public Vector2 LastDragDirection
        {
            get { return lastDragDir; }
        }
        /// <summary>
        /// Return touch world position.
        /// </summary>
        public Vector3 WorldPos
        {
            get { return wPos; }
        }

        private Vector2 touchDeltaPosRaw;
        private Vector2 priorityAxe;
        private Vector2 lastDragDir;
        private Vector3 wPos;
        private Vector2 touchPos;

        /// <summary>
        /// Fill touch arguments from touch object;
        /// </summary>
        public void SetTouch(Touch touch)
        {
            touchPos = touch.position;
            wPos = Camera.main.ScreenToWorldPoint(touchPos);
            hits = Physics2D.OverlapPointAll(new Vector2(wPos.x, wPos.y));
            touchDeltaPosRaw = touch.deltaPosition;

            if (touch.phase == TouchPhase.Moved)
            {
                lastDragDir = touchDeltaPosRaw;
                priorityAxe = GetPriorityOneDirAbs(touchDeltaPosRaw);
            }
        }

        /// <summary>
        /// Fill touch arguments.
        /// </summary>
        public void SetTouch(Vector2 position, Vector2 deltaPosition, TouchPhase touchPhase)
        {
            touchPos = position;
            wPos = Camera.main.ScreenToWorldPoint(touchPos);
            hits = Physics2D.OverlapPointAll(new Vector2(wPos.x, wPos.y));
            touchDeltaPosRaw = deltaPosition;

            if (touchPhase == TouchPhase.Moved)
            {
                lastDragDir = touchDeltaPosRaw;
                priorityAxe = GetPriorityOneDirAbs(touchDeltaPosRaw);
            }
        }

        /// <summary>
        /// Return drag icon for firs touched elment or null.
        /// </summary>
        public GameObject GetIconDrag()
        {
            if (firstSelected != null)
            {
                GameObject icon = firstSelected.GetDataIcon();
                return icon;
            }
            else
            {
                return null;
            }

        }

        private Vector2 GetPriorityOneDirAbs(Vector2 sourceDir)
        {

            if (Mathf.Abs(sourceDir.x) > Mathf.Abs(sourceDir.y))
            {
                float x = (sourceDir.x > 0) ? 1 : 1;
                return new Vector2(x, 0f);
            }
            else
            {
                float y = (sourceDir.y > 0) ? 1 : 1;
                return new Vector2(0f, y);
            }
        }
    }

}