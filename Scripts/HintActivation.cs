using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintActivation : MonoBehaviour
{
    public float timeToShowHint;
    public float timeActive;

    public GameObject hintGameObject;

    private IEnumerator corroutine;

    public float timer = 0;


    private void OnEnable()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        timer = 0;
        hintGameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if ((int)timer == timeToShowHint)
        {
            hintGameObject.SetActive(true);
        }
        else if (timer > (timeToShowHint + timeActive))
        {
            ResetTimer();
        }
    }
}
