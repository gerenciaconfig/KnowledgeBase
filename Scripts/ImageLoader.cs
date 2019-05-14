using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public Image imageComponent;

    public void SetImageSprite(string imageName)
    {
        foreach (var item in CurrentStatsInfo.ActivityImagesList)
        {
            if (item.name == imageName)
            {
                imageComponent.sprite = item;
                break;
            }
        }
    }
}
