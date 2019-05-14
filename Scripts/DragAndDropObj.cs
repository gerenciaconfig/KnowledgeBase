using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropObj : MonoBehaviour
{
    public Transform gameCanvas;

    [HideInInspector]
    public RectTransform pivot;
    [Space(10)]
    public GameObject slot;
    public GameObject slotImage;

    public bool refPos=false;
    public GameObject refObj;

    [Space(10)]
    public float backAnimVelocity;
    public float xLimitDistance;
    public float yLimitDistance;

    public static DragStates dragState = DragStates.notDragging;
    public static GameObject draggingObj;
    public DragAndDropSlot currentSlot;

    public Vector3 savedPosition;

    public bool playFailSound = true;
    public bool playRightSound = false;
    public bool truePlayNoRightSound = false;
    public bool saveCurrentSlot = false;
    public bool keepImage = false;
    public bool keepRaycast = false;
    public bool holdProgress = false;
    public bool clearCurrentSlotImage = false;
    public bool resetScaleOnSendBack = false;
    public float dragScaleMultiplier = 1;

    [HideInInspector]
    public ActivityAnalytics activityAnalytics;

    private void OnEnable()
    {
        savedPosition = transform.position;

        try
        {
            activityAnalytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<ActivityAnalytics>();
        }
        catch
        {

        }
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
            if (currentSlot != null)
            {
                currentSlot.checkedSlot = false; //Descheca o slot atual quando o drag é removido dele
                if (clearCurrentSlotImage)
                {
                    currentSlot.ResetSlot();
                }
                currentSlot.receivedSlot = null;
                currentSlot = null;
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

    public IEnumerator BackToDefaultPosition(Transform objTransform, float velocity, Transform targetTransform)
    {
        dragState = DragStates.backing;
        if (resetScaleOnSendBack)
        {
            transform.localScale = Vector3.one;
        }

        while (objTransform.position != targetTransform.position)
        {
            objTransform.position = Vector3.MoveTowards(objTransform.position, targetTransform.position, velocity * Time.deltaTime);
            yield return null;
        }

        dragState = DragStates.notDragging;
        draggingObj = null;
    }

    public virtual void CheckCorrectItem()
    {
        if (Mathf.Abs(transform.position.x - slot.transform.position.x) <= xLimitDistance &&
            Mathf.Abs(transform.position.y - slot.transform.position.y) <= yLimitDistance)
        {
            dragState = DragStates.notDragging;
            if (slot.GetComponent<DragAndDropSlot>() != null)
            {
                slot.GetComponent<DragAndDropSlot>().checkedSlot = true;
                slot.GetComponent<DragAndDropSlot>().OnCheckedSlot.Invoke();
            }
            GetComponent<Image>().raycastTarget = false;
            draggingObj = null;
            if (refPos)
            {
                refObj.transform.position = this.transform.position;
            }            
            this.transform.position = slot.transform.position;
            GetComponent<Image>().enabled = false;
            if (slotImage != null)
            {
                slotImage.SetActive(true);
            }  
            
            if(playRightSound)
            {
                if (!holdProgress)
                {
                    DragAndDropLogic.instance.MakeProgress();
                }
                if (!truePlayNoRightSound)
                {
                    AudioManager.instance.StopAllSounds();
                    AudioManager.instance.PlaySound(DragAndDropLogic.instance.rightSound);
                    AudioManager.instance.PlayRandomSuccessSound();
                }
                
            }
            else
            {
                if (!holdProgress)
                {
                    DragAndDropLogic.instance.MakeProgress();
                }
                if (!truePlayNoRightSound)
                {
                    AudioManager.instance.StopAllSounds();
                    AudioManager.instance.PlaySound(DragAndDropLogic.instance.rightSound);
                }
            }
            if (activityAnalytics != null)
            {
                activityAnalytics.AddRight();
            }
        }
        else
        {
            if(playFailSound)
            {
                AudioManager.instance.StopAllSounds();
                AudioManager.instance.PlayRandomFailSound();
            }
            //GetComponent<Animator>().SetTrigger("popDown");
            StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
            if (activityAnalytics != null)
            {
                activityAnalytics.AddWrong();
            }
        }
    }
    
    public virtual void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().enabled = true;
        if (slotImage != null)
        {
            slotImage.SetActive(false);
        }
        GetComponent<Image>().raycastTarget = true;
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;
    }

    public void ClearCurrentSlot()
    {
        currentSlot = null;
    }
}
