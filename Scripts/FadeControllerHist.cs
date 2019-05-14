using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeControllerHist : MonoBehaviour
{
    public UnityEvent OnFade;

    public void InvokeOnFadeEvent()
    {
        OnFade.Invoke();
    }

    public void DeactivateFadeImage()
    {
        this.gameObject.SetActive(false);
    }
}
