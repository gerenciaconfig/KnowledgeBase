using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("UI", "Set Raycast Target", "")]
public class FungusSetRaycastTarget : Command
{
    public GameObject parent;

    private Image[] images;

    public bool value;

    public override void OnEnter()
    {
        images = parent.GetComponentsInChildren<Image>();

        for (int i = 0; i < images.Length; i++)
        {
            images[i].raycastTarget = value;
        }

        Continue();
    }
}
