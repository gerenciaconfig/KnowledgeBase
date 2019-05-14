using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Transform", "Set Scale", "")]
public class SetScale : Command
{
    public Transform transform;
    public Vector3 target;

    public override void OnEnter()
    {
        transform.localScale = target;

        Continue();
    }
}
