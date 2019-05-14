using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColorToChange : MonoBehaviour
{
    public ColorChanger colorChanger;
    public Color actualColor;
    
    public void SetMainColor()
    {
        colorChanger.mainColor = this.actualColor;
        colorChanger.mainColor.a = 1f;
        colorChanger.ChangeColor();
    }
}
