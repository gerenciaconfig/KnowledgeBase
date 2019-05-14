using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEducaAttributesSetter : MonoBehaviour
{
    public string currentGame;
    public string childID;

    // Start is called before the first frame update
    void Awake()
    {
        PlayEducaAttributes.SetCurrentActivity(currentGame);
        PlayEducaAttributes.SetChildID(childID);
    }
}
