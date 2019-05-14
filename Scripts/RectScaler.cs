using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class RectScaler : MonoBehaviour
{

    public RectTransform rectTransformFrom;
    public RectTransform rectTransformTo;

    public float time;

    private Rect rectFrom;
    private Rect rectTo;

    Vector2 originalAnchorMax;
    Vector2 originalAnchorMin;

    // Use this for initialization
    void Start ()
    {
        originalAnchorMax = rectTransformFrom.anchorMax;
        originalAnchorMin = rectTransformFrom.anchorMin;

        rectFrom = rectTransformFrom.rect;
        rectTo = rectTransformTo.rect;

        StartCoroutine(ScaleRect(time));
    }

    public void ResetToStartRect()
    {
        rectTransformFrom.anchorMax = originalAnchorMax;
        rectTransformFrom.anchorMin = originalAnchorMin;
    }

    public IEnumerator ScaleRect(float seconds)
    {
        float elapsedTime = 0;

        Vector2 anchorMaxOriginal = rectTransformFrom.anchorMax;
        Vector2 anchorMinOriginal = rectTransformFrom.anchorMin;

        while (elapsedTime < seconds)
        {
            rectTransformFrom.anchorMax = Vector2.Lerp(anchorMaxOriginal, rectTransformTo.anchorMax, (elapsedTime / seconds));
            rectTransformFrom.anchorMin = Vector2.Lerp(anchorMinOriginal, rectTransformTo.anchorMin, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
        ResetToStartRect();
    }
}
