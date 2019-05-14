using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialFill : MonoBehaviour {

    private Image radial;

    public float totalTime;

	// Use this for initialization
	void Awake ()
    {
        radial = GetComponent<Image>();
	}

    private void OnEnable()
    {
        StartCoroutine(Fill(totalTime));
    }

    public IEnumerator Fill(float seconds)
    {
        radial.fillAmount = 0;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            radial.fillAmount = Mathf.Lerp(0,1,(elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        /*
        if (waitUntilFinished)
        {
            Continue();
        }*/
    }
}
