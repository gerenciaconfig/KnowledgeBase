using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOrientation : MonoBehaviour
{

    public enum Type
    {
       Portrait,Landscape
    }

    public Type type;

    void Awake()
    {
        switch (type)
        {
            case Type.Portrait:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case Type.Landscape:
                Screen.orientation = ScreenOrientation.Landscape;
                break;
        }
    }

    
}
