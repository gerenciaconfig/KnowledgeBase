using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinats
{

    public static Vector3 WorldToCanvasCenterCenter(Vector3 wPos, Canvas c)
    {
        RectTransform CanvasRect = c.GetComponent<RectTransform>();

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(wPos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((viewportPoint.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((viewportPoint.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }

    public static Vector3 WorldToCanvasBottomLeft(Vector3 wPos, Canvas c)
    {
        RectTransform CanvasRect = c.GetComponent<RectTransform>();

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(wPos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((viewportPoint.x * CanvasRect.sizeDelta.x)),
        ((viewportPoint.y * CanvasRect.sizeDelta.y)));
        return WorldObject_ScreenPosition;
    }

    public static Vector3 CanvasToWorld(GameObject guiObject)
    {
        Vector3 wPos = Camera.main.ScreenToWorldPoint(RectTransformUtility.WorldToScreenPoint(null, guiObject.transform.position)); //http://answers.unity3d.com/questions/826851/how-to-get-screen-position-of-a-recttransform-when.html
        return wPos;
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
    }

    public static Vector2 RectTransformToCanvasSpaceCenterCenter(RectTransform transform, Canvas c)
    {
        RectTransform CanvasRect = c.GetComponent<RectTransform>();
        Vector2 viewportPoint =  Camera.main.ScreenToViewportPoint(new Vector3(transform.position.x, Screen.height - transform.position.y, transform.position.z));
       // Debug.Log("view port" + viewportPoint);
        Vector2 WorldObject_ScreenPosition = new Vector2(
       ((viewportPoint.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
       (-(viewportPoint.y * CanvasRect.sizeDelta.y) + (CanvasRect.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }
}
