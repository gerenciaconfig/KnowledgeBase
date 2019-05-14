using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropFree : DragAndDropObj {

    public Game gameReference;

    public bool mustBePlaced;
    public bool doNotRemoveVictory = false;

    bool isInScene;

    public float scaleMultiplier = 1;

    public string audioName;
    public bool playAudio;
    public bool placed = false;

    public Flowchart flowChart;

    private void OnEnable()
    {
        savedPosition = transform.position;
        pivot.transform.position = savedPosition;
        transform.localScale = Vector3.one;
        isInScene = false;
        placed = false;
    }

    public new void OnDisable()
    {
        ResetDragObj();
    }

    public new void PointerEndDrag()
    {
        if (dragState != DragStates.tutorial)
        {
            CheckCorrectItem();
        }
    }

    public new void CheckCorrectItem()
    {
        if (slot != null && mustBePlaced)
        {
            if (Mathf.Abs(transform.position.x - slot.transform.position.x) <= xLimitDistance &&
            Mathf.Abs(transform.position.y - slot.transform.position.y) <= yLimitDistance)
            {
                if (isInScene == false)
                {
                    if (!holdProgress)
                    {
                        gameReference.AddVictory(true);
                    }
                    isInScene = true;
                    if (playAudio)
                    {
                        AudioManager.instance.StopAllSounds();
                        AudioManager.instance.PlaySound(audioName);
                    }
                }

                transform.localScale = Vector3.one * scaleMultiplier;

                dragState = DragStates.notDragging;
                draggingObj = null;
                placed = true;
            }
            else
            {
                if (isInScene == true)
                {
                    if (!doNotRemoveVictory)
                    {
                        gameReference.RemoveVictory(true);
                    }
                    isInScene = false;
                }
                else
                {
                   // AudioManager.instance.StopAllSounds();
                    //AudioManager.instance.PlaySound(audioName);
                }
                
                transform.localScale = Vector3.one;
                StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
                placed = false;
            }
        }
        else
        {
            if (playAudio)
            {
                AudioManager.instance.StopAllSounds();
                AudioManager.instance.PlaySound(audioName);
            }

            transform.localScale = Vector3.one;
            
            StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
        }
    }

    public new void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().raycastTarget = true;
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;
    }

    public void FadeObj()
    {
        GetComponent<Animator>().SetTrigger("fadeOut");
    }
}
