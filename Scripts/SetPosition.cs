using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Transform", "SetPosition", "")]
public class SetPosition : Command
{
    public Transform objectTransform;
    public Vector2 newPosition;

    public override void OnEnter()
    {
        objectTransform.position = newPosition;

        Continue();
    }
    
}
