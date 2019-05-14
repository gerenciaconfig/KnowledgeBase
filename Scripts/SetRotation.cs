using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Transform", "SetRotation", "")]
public class SetRotation : Command
{

    public Transform objectTransform;
    public Quaternion newRotation;

    public override void OnEnter()
    {
        objectTransform.rotation = newRotation;

        Continue();
    }
}
