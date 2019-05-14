using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("UI", "Set Image Sprite", "Sets the image sprite")]
public class FungusSetImageSprite : Command
{
    public Image uiImage;
    public Sprite sprite;

    public override void OnEnter()
    {
        uiImage.sprite = sprite;
        Continue();
    }
}
