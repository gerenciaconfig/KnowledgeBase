using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraFix : MonoBehaviour
{
    public enum CameraType { Orthographic, Perspective}

    public CameraType cameraType;

    private float newCameraSize;

    public float multiplier;

    float originalCameraSize;

    Camera cam;


    private void Awake()
    {
        EventDispatcher.Add(EventName.GameSelectImage, ToggleGameCamera);

        cam = GetComponent<Camera>();

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
        print(cam.aspect);
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
}
