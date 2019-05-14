using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasZoom : MonoBehaviour
{
   public static IEnumerator LerpScale(float seconds, Vector3 targetScale, Vector2 targetPos, RectTransform target)
   {
       float elapsedTime = 0;

       //Ajusta a escala para outras proporções diferentes de 16:9
       if (targetScale.x != 1)
       {
           targetScale *= Camera.main.aspect / (1.78f);
       }

       Vector3 originalScale = target.localScale;
       Vector2 originalPos = target.anchoredPosition;

       while (elapsedTime < seconds)
       {
           target.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / seconds));
           target.anchoredPosition = Vector2.Lerp(originalPos, targetPos, (elapsedTime / seconds));

           elapsedTime += Time.deltaTime;
           yield return new WaitForEndOfFrame();
       }

        target.localScale = targetScale;
        target.anchoredPosition = targetPos;
    }

    public static IEnumerator LerpScaleGlobal(bool adjustScale, float seconds, Vector3 targetScale, Vector3 targetPos, Quaternion targetRotation, RectTransform target)
    {
        float elapsedTime = 0;

        //Ajusta a escala para outras proporções diferentes de 16:9
        if (adjustScale && targetScale.x != 1)
        {
            targetScale *= Camera.main.aspect / (1.78f);
        }

        Vector3 originalScale = target.localScale;
        Vector3 originalPos = target.position;
        Quaternion originalRotation = target.rotation;

        while (elapsedTime < seconds)
        {
            target.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / seconds));
            target.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / seconds));
            target.rotation = Quaternion.Lerp(originalRotation, targetRotation, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        target.localScale = targetScale;
        target.position = targetPos;
        target.rotation = targetRotation;
    }


    public static IEnumerator LerpScale(float seconds, Vector3 targetScale, RectTransform target)
   {
       float elapsedTime = 0;

       //Ajusta a escala para outras proporções diferentes de 16:9
       if (targetScale.x != 1)
       {
           targetScale *= Camera.main.aspect / (1.78f);
       }

       Vector3 originalScale = target.localScale;

       while (elapsedTime < seconds)
       {
           target.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / seconds));

           elapsedTime += Time.deltaTime;
           yield return new WaitForEndOfFrame();
       }
   }

   public static void SetScale(Vector3 targetScale, Vector2 targetPos, RectTransform target)
   {
       target.localScale = AdjustScaleToScreenProportion(targetScale);
       target.anchoredPosition = targetPos;
   }

    public static void SetScaleGlobal(Vector3 targetScale, Vector3 targetPos, Quaternion targetRotation, RectTransform target)
    {
        target.localScale = AdjustScaleToScreenProportion(targetScale);
        target.position = targetPos;
        target.rotation = targetRotation;

        target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, 0);
    }

    //Ajusta a escala para outras proporções diferentes de 16:9
    private static Vector3 AdjustScaleToScreenProportion(Vector3 scale)
   {
       if (scale.x != 1)
       {
           scale *= Camera.main.aspect / (1.78f);
       }

       return scale;
   }
}