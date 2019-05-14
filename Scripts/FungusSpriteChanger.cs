using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("UI", "Button Sprite Change", "Changes the button sprite to the selected one")]
public class FungusSpriteChanger : Command
{
    public enum ChangeTo {NormalSprite, HighlightedSprite };

    public ChangeTo changeTo;
    public ButtonBehaviour buttonBehaviour;

    public override void OnEnter()
    {
        switch (changeTo)
        {
            case ChangeTo.NormalSprite:
                buttonBehaviour.SetNormalSprite();
                break;

            case ChangeTo.HighlightedSprite:
                buttonBehaviour.SetHighlightedSprite();
                break;
        }
        Continue();
    }
}
