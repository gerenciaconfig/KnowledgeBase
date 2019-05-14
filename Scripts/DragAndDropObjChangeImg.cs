using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropObjChangeImg : DragAndDropObj
{
    public GameObject levelReference;
    public bool refAnim;

    public bool hasAudioName = false;
    public string rightSoundName;
    public string wrongSoundName;

    public List<GameObject> slotList = new List<GameObject>();

    public override void CheckCorrectItem()
    {
        foreach (var slotobj in slotList)
        {
            if (Mathf.Abs(transform.position.x - slotobj.transform.position.x) <= xLimitDistance &&
                Mathf.Abs(transform.position.y - slotobj.transform.position.y) <= yLimitDistance && slotobj.GetComponent<DragAndDropSlot>().checkedSlot == false)
            {
                if (slotList.Contains(slotobj) )
                {
                    dragState = DragStates.notDragging;
                    GetComponent<Image>().raycastTarget = false;
                    draggingObj = null;
                    
                    if (refPos)
                    {
                        refObj.transform.position = this.transform.position;
                    }

                    this.transform.position = slotobj.transform.position;
                    GetComponent<Image>().enabled = false;
                    slotobj.GetComponent<DragAndDropSlot>().checkedSlot = true;

                    if (slotImage == null)
                    {
                        slotobj.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                        slotobj.GetComponent<Image>().enabled = true;}
                    }
               
                    if(hasAudioName)
                    {
                        DragAndDropLogic.instance.MakeProgress();
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlaySound(DragAndDropLogic.instance.rightSound);
                        AudioManager.instance.PlaySound(rightSoundName);
                    }
                    else
                    {
                        DragAndDropLogic.instance.MakeProgress();
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlaySound(DragAndDropLogic.instance.rightSound);
                    }
            }
                
            else
            {
                if(playFailSound)
                {
                    if(hasAudioName)
                    {
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlaySound(wrongSoundName);
                    }
                    else
                    {
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlayRandomFailSound();
                    }
                }
                //GetComponent<Animator>().SetTrigger("popDown");
                StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
            }
        }
    }

    public override void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().raycastTarget = true;
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;

        foreach (GameObject slotobj in slotList)
        {
           slotobj.GetComponent<Image>().enabled = false;
           slotobj.GetComponent<DragAndDropSlot>().checkedSlot = false;
        }
    }
}
