using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Color mainColor;
    public List <GameObject> colorObj = new List <GameObject>();

    public void ChangeColor()
    {
        foreach (GameObject obj in colorObj)
        {
            obj.GetComponent<Image>().color = mainColor;
        }
    }
}
