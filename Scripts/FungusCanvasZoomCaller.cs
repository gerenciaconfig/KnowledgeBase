using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("UI", "Canvas Zoom", "Zooms the target UI object to the target pos and scale")]
public class FungusCanvasZoomCaller : Command
{
    public enum ZoomType { Lerp, Set};
    public enum ZoomBreed { Local, Global};
    public ZoomType zoomType;
    public ZoomBreed zoomBreed;


    public RectTransform target;

    public RectTransform targetTransform;

    public Vector3 targetScale;

    public Vector2 targetPos;

    public Quaternion targetRotation;

    public float seconds;

    public bool waitUntilFinished;

    public bool adjustScale = true;

    public override void OnEnter()
    {




        switch (zoomType)
        {
            case ZoomType.Lerp:
                switch (zoomBreed)
                {
                    case ZoomBreed.Local:
                        if (targetTransform == null)
                        {
                            StartCoroutine(CanvasZoom.LerpScale(seconds, targetScale, targetPos, target));
                        }
                        else
                        {
                            StartCoroutine(CanvasZoom.LerpScale(seconds, targetTransform.localScale, targetTransform.position, target));
                        }
                        break;

                    case ZoomBreed.Global:
                        if (targetTransform == null)
                        {
                            StartCoroutine(CanvasZoom.LerpScale(seconds, targetScale, targetPos, target));
                        }
                        else
                        {
                            StartCoroutine(CanvasZoom.LerpScaleGlobal(adjustScale, seconds, targetTransform.localScale, targetTransform.position, targetTransform.rotation, target));
                        }
                        break;
                }
               
                break;

            case ZoomType.Set:

                switch (zoomBreed)
                {
                    case ZoomBreed.Local:
                        if (targetTransform == null)
                        {
                            CanvasZoom.SetScale(targetScale, targetPos, target);
                        }
                        else
                        {
                            CanvasZoom.SetScale(targetTransform.localScale, targetTransform.anchoredPosition, target);
                        }
                        break;

                    case ZoomBreed.Global:
                        if (targetTransform == null)
                        {
                            CanvasZoom.SetScaleGlobal(targetScale, targetPos, Quaternion.identity, target);
                        }
                        else
                        {
                            CanvasZoom.SetScaleGlobal(targetTransform.localScale, targetTransform.position, targetTransform.rotation, target);
                        }
                        break;
                }

                if (targetTransform == null)
                {
                    CanvasZoom.SetScale(targetScale, targetPos, target);
                }
                else
                {
                    CanvasZoom.SetScale(targetTransform.localScale, targetTransform.anchoredPosition, target);
                }
                break;
        }
        

        if(waitUntilFinished)
        {
            StartCoroutine(WaitAndContinue(seconds));
        }
        else
        {
            Continue();
        }
    }

    private IEnumerator WaitAndContinue(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Continue();
    }
    
}
