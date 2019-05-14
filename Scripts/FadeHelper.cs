using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.Home;

public class FadeHelper : MonoBehaviour
{
    private static InteractCollider interactCollider; 

    public void FadeFinished()
    {
        interactCollider.PlayTransition();
    }

    public void FadeStarted()
    {
        interactCollider.playingTransition = true;
    }

    public static void SetInteractCollider(InteractCollider ic)
    {
        interactCollider = ic;
    }
}
