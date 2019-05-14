using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Home { 
    public class HomeIlhasHelper : MonoBehaviour
    {
        public enum PreviousHomeFocus {IVulcan, ITech, IBeach, IForest, IGlacius, ISearch}

        public enum CameraType { Orthographic, Perspective }

        public static PreviousHomeFocus previousHomeFocus;

        public CameraType cameraType;

        private float newCameraSize;

        public float multiplier;

        float originalCameraSize;


        public GameObject objIVulcan;
        public GameObject objITech;
        public GameObject objIBeach;
        public GameObject objIForest;
        public GameObject objIGlacius;

        public static bool firstAcess = true;
        public static bool reEnter = true;
        public static Transform previousCamTransform;

        Camera cam;    

        private void Awake()
        {
            EventDispatcher.Add(EventName.GameSelectImage, ToggleGameCamera);

            cam = Camera.main;

            float aspectMultiplier = multiplier * (1 + (16f / 9 - cam.aspect));

            if (cameraType == CameraType.Orthographic)
            {
                originalCameraSize = cam.orthographicSize;
            }
            else
            {
                originalCameraSize = cam.fieldOfView;
            }

            newCameraSize = originalCameraSize * aspectMultiplier;
        
        }

        public void ToggleGameCamera()
        {
            //print(cam.aspect);
            if (cam.aspect < 1.7f)
            {
                if (cameraType == CameraType.Orthographic)
                {
                    cam.orthographicSize = newCameraSize;
                }
                else
                {
                    cam.fieldOfView = newCameraSize;

                }
            }
        }

        private void Start()
        {
            ToggleGameCamera();
            CheckPreviusState();
        }

        private void CheckPreviusState()
        {
            if (firstAcess)
            {               
                return;
            }
            else if (reEnter)
            {
                return;
            }
            else
            {
                HudManager.BackFromActivity();
                Camera.main.transform.position = new Vector3(0, 0, -10);
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicVolumeChecker>().CheckVolume();
                switch (previousHomeFocus)
                {
                    case PreviousHomeFocus.IBeach:
                        objIBeach.SetActive(true);
                        break;
                    case PreviousHomeFocus.IForest:
                        objIForest.SetActive(true);
                        break;
                    case PreviousHomeFocus.IGlacius:
                        objIGlacius.SetActive(true);
                        break;
                    case PreviousHomeFocus.ITech:
                        objITech.SetActive(true);
                        break;
                    case PreviousHomeFocus.IVulcan:
                        objIVulcan.SetActive(true);               
                        break;
                    case PreviousHomeFocus.ISearch:

                        break;
                }
            }
        }

        public void DisableActivitysCollider()
        {
            GameObject worlds = HudManager.worlds;
            if (worlds != null && worlds.gameObject.activeSelf)
            {
                GameObject[] activityButtons = GameObject.FindGameObjectsWithTag("ActivityButton");

                foreach (GameObject go in activityButtons)
                {
                    go.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

        public void EnableActivitysCollider()
        {
            GameObject worlds = HudManager.worlds;
            if (worlds != null && worlds.gameObject.activeSelf)
            {
                GameObject[] activityButtons = GameObject.FindGameObjectsWithTag("ActivityButton");

                foreach (GameObject go in activityButtons)
                {
                    go.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
    }
}
