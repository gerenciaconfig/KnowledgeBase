using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("UI", "Fade All Images", "Changes the all images alpha, child from a transform, according to the selected value")]
public class FadeAllImages : Command
{
    public enum Type { Lerp, Set };

    public Type type;

    [Range(0, 1)]
    public float targetAlpha;

    public Transform target;

    public float seconds;

    public bool waitUntilFinished;

    private Image[] images;

    public override void OnEnter()
    {
        images = target.GetComponentsInChildren<Image>();

        switch (type)
        {
            case Type.Lerp:
                StartCoroutine(ChangeAlpha(seconds));
                break;

            case Type.Set:
                for (int i = 0; i < images.Length; i++)
                {
                    Color targetColor = images[i].color;
                    targetColor.a = targetAlpha;
                    images[i].color = targetColor;
                }
                break;
        }



        if (!waitUntilFinished)
        {
            Continue();
        }
    }

    public IEnumerator ChangeAlpha(float seconds)
    {
        float elapsedTime = 0;

        
        Color[] originalColors = new Color[images.Length];


        Color[] targetColors = new Color[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            originalColors[i] = images[i].color;
            targetColors[i] = images[i].color;
            targetColors[i].a = targetAlpha;
        }

        while (elapsedTime < seconds)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = Color.Lerp(originalColors[i], targetColors[i], (elapsedTime / seconds));
            }
            
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = targetColors[i];
        }

        if (waitUntilFinished)
        {
            Continue();
        }
    }
}