using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtil : MonoBehaviour {

    public Animator Anim;

    public void DisableAnimator()
    {
        Anim.enabled = false;
    }
    public void EnableAnimator()
    {
        Anim.enabled = true;
    }
}
