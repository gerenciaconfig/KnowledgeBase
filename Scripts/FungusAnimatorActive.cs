using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Animation", "Animator Controller", "")]
public class FungusAnimatorActive : Command
{
    public GameObject objAnim;
    public bool setAnimator;

    public override void OnEnter()
    {
        objAnim.GetComponent<Animator>().enabled = setAnimator;

        Continue();
    }
}
