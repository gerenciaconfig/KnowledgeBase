using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class DragAndDropObjList : DragAndDropObj
{
    public RectTransform pivotReturn;

    public int pointsToGive;//maximo de vezes que o objeto tem que ser arrastado para os slots
    public int pointsGiven;//numero de vezes que o objeto ja foi arrastado para os slots corretos

    public Flowchart flow;
    public string blockName;
    public Image alphaImg;

    public List<GameObject> slotList = new List<GameObject>();
    //public List<GameObject> slotImageList = new List<GameObject>();

    public bool setSlotImage = false;
    public bool changeScale = false;
    public bool changeSlotScale = false;

    public float coroutineDelay = 0.0f;
    public float scaleMultiplier = 1;

    public override void CheckCorrectItem()
    {
        foreach (var slotobj in slotList)
        {
            if (Mathf.Abs(transform.position.x - slotobj.transform.position.x) <= xLimitDistance &&
                Mathf.Abs(transform.position.y - slotobj.transform.position.y) <= yLimitDistance)
            {
                if (slotList.Contains(slotobj) && slotobj.GetComponent<DragAndDropSlot>().checkedSlot == false)
                {
                    if (playRightSound)
                    {
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlaySound(DragAndDropLogic.instance.rightSound);
                    }
                    if (GetComponent<Animator>() != null)
                    {
                        GetComponent<Animator>().enabled = false;
                    }
                    if (blockName != "")
                    {
                        flow.ExecuteBlock(blockName);
                    }
                    slotobj.GetComponent<DragAndDropSlot>().checkedSlot = true;
                    if (!setSlotImage)
                    {
                        if (changeSlotScale)
                        {
                            slotobj.transform.localScale = this.transform.localScale;
                        }
                        StartCoroutine(AfterAnim()); //coroutine de quando o slot não recebe a imagem do item
                    } else
                    {
                        slotobj.GetComponentInChildren<Image>().sprite = this.GetComponent<Image>().sprite;
                        slotobj.GetComponentInChildren<Image>().color = this.GetComponent<Image>().color;
                        slotobj.GetComponentInChildren<Image>().enabled = true;
                        if (changeSlotScale)
                        {
                            slotobj.transform.localScale = this.transform.localScale;
                        }
                        StartCoroutine(ChangeSlotImage());
                    }
                    slotobj.GetComponent<DragAndDropSlot>().receivedSlot = this.gameObject;
                    if (saveCurrentSlot)
                    {
                        currentSlot = slotobj.GetComponent<DragAndDropSlot>();
                    }
                    slotobj.GetComponent<DragAndDropSlot>().OnCheckedSlot.Invoke();
                    //StartCoroutine(AfterAnim());
                    //AfterAnim();
                    if (activityAnalytics != null)
                    {
                        activityAnalytics.AddRight();
                    }
                    return;
                }
            }
        }

        if (playFailSound)
        {
            AudioManager.instance.StopAllSounds();
            AudioManager.instance.PlayRandomFailSound();
        }
        StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
    }

    public override void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().raycastTarget = true;
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;
        if (this.alphaImg != null)
        {
            this.alphaImg.GetComponent<Image>().enabled = false;
        }
        pointsGiven = 0;

        foreach (GameObject slot in slotList)
        {
            slot.GetComponent<DragAndDropSlot>().checkedSlot = false;
            slot.GetComponent<DragAndDropSlot>().ResetSlot();
        }
    }

    public void CheckSlots()
    {
        if (this.pointsGiven < pointsToGive)
        {
            draggingObj = null;
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().raycastTarget = true;
            dragState = DragStates.notDragging;
            this.transform.position = pivot.transform.position;
            this.pointsGiven += 1;
        }

        if (this.pointsGiven >= pointsToGive)
        {
            if (!keepImage)
            {
                this.GetComponent<Image>().enabled = false;
            }
            if (changeScale)
            {
                transform.localScale = Vector3.one * dragScaleMultiplier;
            }
            if (alphaImg != null)
            {
                this.alphaImg.GetComponent<Image>().enabled = true;
            }
            if (!keepRaycast)
            {
                GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    public IEnumerator AfterAnim()
    {

        if (!keepRaycast)
        {
            GetComponent<Image>().raycastTarget = false;
        }
        yield return new WaitForSeconds(coroutineDelay);
        dragState = DragStates.notDragging;
        //GetComponent<Image>().raycastTarget = false;
        draggingObj = null;
        if (!keepImage)
        {
            GetComponent<Image>().enabled = false;
        }
        if (!holdProgress)
        {
            DragAndDropLogic.instance.MakeProgress();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = true;
        }
        CheckSlots();
        //yield return null;
    }

    public IEnumerator ChangeSlotImage()
    {
        if (!keepRaycast)
        {
            GetComponent<Image>().raycastTarget = false;
        }
        if (coroutineDelay > 0f)
        {
            yield return new WaitForSeconds(coroutineDelay);
        }
        dragState = DragStates.notDragging;
        draggingObj = null;
        if (!keepImage)
        {
            GetComponent<Image>().enabled = false;
        }
        if (!holdProgress)
        {
            DragAndDropLogic.instance.MakeProgress();
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = true;
        }
        //slotImage.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        //slotImage.GetComponent<Image>().enabled = true;
        CheckSlots();
        yield return null;
    }

    public void setPivotReturn()
    {
        pivot = pivotReturn;
    }
}