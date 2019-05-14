using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arcolabs.Home.Activity;

namespace Arcolabs.Home { 
    public class InteractCollider : MonoBehaviour
    {
    
        private Vector3 screenPoint;
        private Vector3 offset;
        Vector3 curScreenPoint;
        Vector3 curPosition;
        private Camera cam;

        public GameObject World;

        private float clickTime;

        
        public Transform targetEnd;
        /*
        public float lerpTimeIncrement = 0.005f;
        public float lerpTimeStart=0.02f;
        public float lerpTimeCurrent;
        */
        public float seconds = 0.5f;

        public float deepness;
        public bool playingTransition = false;

        private Vector3 target;

        private void Start()
        {
            cam = Camera.main;
        }

        private void OnMouseUp()
        {
            if (World == null)
            {
                return;
            }
            else if ((Time.time - clickTime) < 0.15f)
            {
                /*World.SetActive(true);
                SetLastAreaFocus(World);
                HudManager.backButton.SetActive(true);*/

                cam.GetComponent<IntroHelper>().DisableColliders();
                target = new Vector3(targetEnd.position.x, targetEnd.position.y, deepness);
                StartCoroutine(Transitioning());
            }
        }

        void OnMouseDrag()
        {
            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }

        void OnMouseDown()
        {
            screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            clickTime = Time.time;
        }

        private void SetLastAreaFocus(GameObject area)
        {
            switch (area.name)
            {
                case "VulcanWorld":
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IVulcan;
                    break;
                case "BeachWorld":
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IBeach;
                    break;
                case "GeloWorld":
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IGlacius;
                    break;
                case "TechWorld":
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.ITech;
                    break;
                case "ForestWorld":
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IForest;
                    break;
                default:

                    break;
            }
        }
                
        private IEnumerator Transitioning()
        {
            float stateTime = 0f;
            FadeHelper.SetInteractCollider(this);
            HudManager.canvasFade.GetComponent<Animator>().SetTrigger("startFade");
            while (stateTime < seconds)
            {
                HudManager.host.gameObject.SetActive(false);
                cam.GetComponent<CameraParallax>().enabled = false;
                cam.transform.position = Vector3.Lerp(cam.transform.position, target, (stateTime / seconds));
                //lerpTimeCurrent += lerpTimeIncrement;
                stateTime += Time.deltaTime;
                //Debug.Log("lerpTimeCurrent Increment: " + lerpTimeCurrent);
                yield return new WaitForEndOfFrame();            }
            
            //lerpTimeCurrent = lerpTimeStart;
        }

        public void PlayTransition()
        {
            HudManager.GoToIsland();
            World.SetActive(true);
            SetLastAreaFocus(World);
            HudManager.backButton.SetActive(true);            
        }

    }
}
