using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ImageAndSoundObject
{
    [Header("Sprite")]
    public Sprite image;

    [Header("Highlighted Sprite")]
    public Sprite highlightedImage;

    [Space]
    [Header("Sound")]
    public string soundName;
}
