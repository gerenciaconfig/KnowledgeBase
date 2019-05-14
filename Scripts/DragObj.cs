using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class DragObj : MonoBehaviour
{
    public Transform gameCanvas;
    public float backVelocity;
    public RectTransform pivot;
    public Transform mainImage;
    public float xLimitDistance;
    public float yLimitDistance;
    public bool isCorrectItem;
    public string fungusCallMessage;

    public static DragStates dragState = DragStates.notDragging;
    public static GameObject draggingObj;

    public enum DragStates
    {
        notDragging,
        gameEnded,
        backing
    }

    public void PointerDrag()
    {
        if (dragState == DragStates.notDragging)
        {
            if (draggingObj == null)
            {
                draggingObj = this.gameObject;
            }

            if (this.gameObject == draggingObj)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCanvas.transform as RectTransform, Input.mousePosition, Camera.main, out pos);
                draggingObj.transform.position = gameCanvas.transform.TransformPoint(pos);
            }
        }
    }

    public void PointerEndDrag()
    {
        CheckCorrectItem();
    }

    IEnumerator BackToDefaultPosition(RectTransform objRect, float velocity, Vector2 anchoredPosition)
    {
        dragState = DragStates.backing;

        while (objRect.anchoredPosition != pivot.anchoredPosition)
        {
            objRect.anchoredPosition = Vector3.MoveTowards(objRect.anchoredPosition, anchoredPosition, velocity * Time.deltaTime);
            yield return null;
        }

        dragState = DragStates.notDragging;
        draggingObj = null;
    }

    void CheckCorrectItem()
    {
        if (isCorrectItem && AvailableDistance())
        {
            dragState = DragStates.notDragging;
            GetComponent<Image>().raycastTarget = false;
            Fungus.Flowchart.BroadcastFungusMessage(fungusCallMessage);
            draggingObj = null;
        }
        else if (isCorrectItem)
        {
            StartCoroutine(BackToDefaultPosition(GetComponent<RectTransform>(), backVelocity, pivot.anchoredPosition));
        }
        else
        {
            StartCoroutine(BackToDefaultPosition(GetComponent<RectTransform>(), backVelocity, pivot.anchoredPosition));
            Fungus.Flowchart.BroadcastFungusMessage(fungusCallMessage);
        }
    }

    public bool AvailableDistance()
    {
        if (Mathf.Abs(transform.position.x - mainImage.transform.position.x) <= xLimitDistance &&
            Mathf.Abs(transform.position.y - mainImage.transform.position.y) <= yLimitDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().raycastTarget = true;
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;
    }
}
