using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DragImage
{
    public DragBehaviour.DropType dropType;
    public Sprite image;
    public Sprite settedSprite;
    public bool mustSelect;
    public string wrongSound;
    public string rightSound;
    //public bool isSortedAudio;
}
