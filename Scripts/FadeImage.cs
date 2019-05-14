using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Image", "Fade Image", "Changes the image alpha according to the selected value")]
public class FadeImage : Command
{
    public enum Type {Lerp, Set };

    public Type type;

    [Range(0, 1)]
    public float targetAlpha;

    public Image image;

    public float seconds;

    public bool waitUntilFinished;

    public override void OnEnter()
    {
        switch(type)
        {
            case Type.Lerp:
                StartCoroutine(ChangeAlpha(seconds));
                break;

            case Type.Set:
                Color targetColor = image.color;
                targetColor.a = targetAlpha;
                image.color = targetColor;
                break;
        }

        

        if(!waitUntilFinished)
        {
            Continue();
        }
    }

    public IEnumerator ChangeAlpha(float seconds)
    {
        float elapsedTime = 0;
        Color originalColor = image.color;
        Color targetColor = image.color;
        targetColor.a = targetAlpha;

        while (elapsedTime < seconds)
        {
            image.color = Color.Lerp(originalColor, targetColor, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        image.color = targetColor;

        if (waitUntilFinished)
        {
            Continue();
        }
    }
}