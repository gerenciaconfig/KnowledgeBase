using UnityEngine;
using UnityEngine.UI;

public class DragAndDropConfirm : DragAndDropObj
{ 
    public Button confirmButton;

    Vector3 savedPosition;

    private void OnEnable()
    {
        savedPosition = transform.position;
        pivot.transform.position = savedPosition;
    }

    public new void CheckCorrectItem()
    {
        if (slot != null)
        {
            if (Mathf.Abs(transform.position.x - slot.transform.position.x) <= xLimitDistance &&
            Mathf.Abs(transform.position.y - slot.transform.position.y) <= yLimitDistance)
            {
                dragState = DragStates.notDragging;
                if (confirmButton != null)
                {
                    confirmButton.interactable = true;
                }
                draggingObj = null;
            }
            else
            {
                AudioManager.instance.StopAllSounds();
                AudioManager.instance.PlayRandomFailSound();
                if (confirmButton != null)
                {
                    confirmButton.interactable = false;
                }

                StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
            }
        }
        else
        {
            AudioManager.instance.StopAllSounds();
            AudioManager.instance.PlayRandomFailSound();
            if (confirmButton != null)
            {
                confirmButton.interactable = false;
            }

            StartCoroutine(BackToDefaultPosition(this.transform, backAnimVelocity, pivot.transform));
        }
    }

    public new void ResetDragObj()
    {
        draggingObj = null;
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().raycastTarget = true;
        if (confirmButton != null)
        {
            confirmButton.interactable = false;
        }
        dragState = DragStates.notDragging;
        this.transform.position = pivot.transform.position;
    }
}
