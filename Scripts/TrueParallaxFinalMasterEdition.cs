using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueParallaxFinalMasterEdition : MonoBehaviour
{
    public bool freezeX;

    public bool freezeY;

    /// <summary>
    /// Max offset relative to the default position.
    /// </summary>
    public Vector3 MaxOffset = new Vector3(0.5f, 0.5f, 0);

    /// <summary>
    /// Whether gyro is enabled.
    /// </summary>
    public bool IsGyroEnabled { get; private set; }

    /// <summary>
    /// Use gyro if available.
    /// </summary>
    public static bool UseGyroscope = true;

    /// <summary>
    /// Value to smooth a gyro/accelerometer data.
    /// </summary>
    private const float _lerpFactor = 0.4f;

    private Vector3 _defaultAcceleration;


    public List<Transform> layerList = new List<Transform>();

    private List<Vector3> maxLayerPositionList = new List<Vector3>();
    private List<Vector3> minLayerPositionList = new List<Vector3>();
    private List<Vector3> defLayerPositionList = new List<Vector3>();

    public List<float> layerSpeedList = new List<float>();


    void Start()
    {
        if (UseGyroscope && SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            IsGyroEnabled = true;
        }

        _defaultAcceleration = IsGyroEnabled ? Input.gyro.gravity : Input.acceleration;


        for(int i = 0; i < layerList.Count; i++)
        {
            maxLayerPositionList.Add(Vector3.zero);
            minLayerPositionList.Add(Vector3.zero);
            defLayerPositionList.Add(Vector3.zero);
            maxLayerPositionList[i] = layerList[i].position + MaxOffset;
            minLayerPositionList[i] = layerList[i].position - MaxOffset;
            defLayerPositionList[i] = layerList[i].position;
        }

    }

    void Update()
    {

            var acceleration = IsGyroEnabled ? Input.gyro.gravity : Input.acceleration;
            
            for(int j = 0; j < layerList.Count; j++)
            {   
                var offsetLayer = new Vector3(layerSpeedList[j] * (_defaultAcceleration.x - acceleration.x),layerSpeedList[j] * (_defaultAcceleration.y - acceleration.y),0);
                var positionLayer = defLayerPositionList[j] - offsetLayer;
                var lerpLayerPosition = Vector3.Lerp(layerList[j].position,positionLayer,_lerpFactor);
                var newLayerPosition = new Vector3(Mathf.Clamp(lerpLayerPosition.x,minLayerPositionList[j].x,maxLayerPositionList[j].x),
                                       Mathf.Clamp(lerpLayerPosition.y,minLayerPositionList[j].y,maxLayerPositionList[j].y), lerpLayerPosition.z);

                layerList[j].position = newLayerPosition;

                if(freezeX)
                {
                    newLayerPosition.x = layerList[j].position.x;
                }

                 if(freezeY)
                {
                    newLayerPosition.y = layerList[j].position.y;
                }    
            }
    }

#if !(UNITY_ANDROID || UNITY_IPHONE)
	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 300, 20), "TrueParallax only works on iOS and Android platforms.");
	}
	
#endif

    void OnDisable()
    {
        for(int l = 0; l < layerList.Count; l++)
        { 
            transform.position = defLayerPositionList[l];
        }
    }
}
