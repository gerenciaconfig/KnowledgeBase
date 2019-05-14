using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragObjMatrizGame : MonoBehaviour
{
    public Transform gameCanvas;
    public RectTransform pivot;

    public float backAnimVelocity;
    public float xLimitDistance;
    public float yLimitDistance;

    public static DragStates dragState = DragStates.notDragging;
    public static GameObject draggingObj;

    Vector3 savedPosition;

    private void OnEnable()
    {
        savedPosition = transform.position;
    }

    public void OnDisable()
    {
        ResetDragObj();
    }

    public enum DragStates
    {
        notDragging,
        gameEnded,
        backing,
        tutorial
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
                this.gameObject.transform.SetAsLastSibling();
            }
        }
    }

    public void PointerEndDrag()
    {
        if (dragState != DragStates.tutorial)
        {
            CheckCorrectItem();
        }     
    }

    IEnumerator BackToDefaultPosition(Transform objTransform, float velocity, Transform targetTransform)
    {
        dragState = DragStates.backing;

        while (objTransform.position != targetTransform.position)
        {
            objTransform.position = Vector3.MoveTowards(objTransform.position, targetTransform.position, velocity * Time.deltaTime);
            yield return null;
        }

        dragState = DragStates.notDragging;
        draggingObj = null;

        MatrizGameLogic.instance.currentGameProperties.colItem.gameObject.transform.SetAsLastSibling();
        MatrizGameLogic.instance.currentGameProperties.lineItem.gameObject.transform.SetAsLastSibling();
    }

    public void CheckCorrectItem()
    {
        if (AvailableDistance() &&
            (this.gameObject == MatrizGameLogic.instance.currentGameProperties.lineItem ||
            this.gameObject == MatrizGameLogic.instance.currentGameProperties.colItem))
        {
            dragState = DragStates.notDragging;
            GetComponent<Image>().raycastTarget = false;
            draggingObj = null;
            this.transform.position = MatrizGameLogic.instance.currentGameProperties.slot.transform.position;
            MatrizGameLogic.instance.MakeProgress();
            AudioManager.instance.PlaySound(MatrizGameLogic.instance.rightSound);
            GetComponent<Animator>().SetTrigger("popDown");
        }
        else
        {
            AudioManager.instance.StopAllSounds();
            if (MatrizGameLogic.instance.buzzSound != "")
            {
                AudioManager.instance.PlaySound(MatrizGameLogic.instance.buzzSound);
            }
            AudioManager.instance.PlayRandomFailSound();
            GetComponent<Animator>().SetTrigger("popDown");
            StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
        }

        Debug.Log("Distance: " + AvailableDistance() +
            "\n" + this.gameObject + " ---- " + MatrizGameLogic.instance.currentGameProperties.lineItem +
            "\n" + MatrizGameLogic.instance.currentGameProperties.colItem);
    }

    public bool AvailableDistance()
    {
        if (Mathf.Abs(transform.position.x - MatrizGameLogic.instance.currentGameProperties.slot.transform.position.x) <= xLimitDistance &&
            Mathf.Abs(transform.position.y - MatrizGameLogic.instance.currentGameProperties.slot.transform.position.y) <= yLimitDistance)
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
