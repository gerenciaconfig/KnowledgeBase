using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Transform", "ResetTransform", "")]
public class ResetTransform : Command {
    public Transform pivotTransform;
    public Transform ObjectTransform;

    public override void OnEnter()
    {
        ObjectTransform.position = pivotTransform.position;
        Continue();
    }
}
