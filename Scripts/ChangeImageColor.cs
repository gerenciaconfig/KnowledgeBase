using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Image", "Change Image Color", "Changes the image color according to the selected value")]
public class ChangeImageColor : Command
{
    public Color targetColor;

    public Image image;

    public float seconds;

    public bool waitUntilFinished;

    public override void OnEnter()
    {
        StartCoroutine(ChangeAlpha(seconds));

        if (!waitUntilFinished)
        {
            Continue();
        }
    }

    public IEnumerator ChangeAlpha(float seconds)
    {
        float elapsedTime = 0;
        Color originalColor = image.color;

        while (elapsedTime < seconds)
        {
            image.color = Color.Lerp(originalColor, targetColor, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (waitUntilFinished)
        {
            Continue();
        }
    }
}