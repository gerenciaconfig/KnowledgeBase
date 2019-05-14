using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;
using UnityEngine.Events;

public class DragAndDropSlot : MonoBehaviour 
{
	public bool checkedSlot = false;
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Color defaultColor;
    public GameObject receivedSlot;
    public bool fadeOnReset = false;
    public bool setColorOnReset = false;
    public bool reactivateDragRaycast = false;
    public bool resetScaleOnSendBack = false;
    public bool playResetBlock = false;
    public Flowchart flowChart;
    public string onResetBlock;

    public void ResetSlot()
    {
        if (defaultSprite != null)
        {
            GetComponent<Image>().sprite = defaultSprite;
        }
        if (defaultColor != null && setColorOnReset)
        {
            GetComponent<Image>().color = defaultColor;
        }
        if (fadeOnReset)
        {
            GetComponent<Image>().enabled = false;
        }
        if (playResetBlock)
        {
            flowChart.ExecuteBlock(onResetBlock);
        }
    }

    public void AlterCheckSlot(bool _value) //Metodo maneiro pra chamar no unity events
    {
        checkedSlot = _value;
    }

    public void DisappearDraggable() //Metodo que faz o draggable que acertou desaparecer imediatamente após o acerto, geralmente chamado no OnCheckedSlot;
    {
        receivedSlot.GetComponent<Image>().enabled = false;
    }

    public void SetFungusParameter(Flowchart flowchart) //Setta uma variavel do flowchart chamada currentDrag como o received slot para utiliza-lo em alguma coisa em algum bloco
    {
        if (receivedSlot != null)
        {
            flowchart.SetGameObjectVariable("currentDrag", this.receivedSlot);
        }
    }

    public void SendReceivedSlotBack()
    {
        if (receivedSlot != null)
        {
            if (resetScaleOnSendBack)
            {
                receivedSlot.GetComponent<DragAndDropObj>().resetScaleOnSendBack = true;
            } else
            {
                receivedSlot.GetComponent<DragAndDropObj>().resetScaleOnSendBack = false;
            }
            receivedSlot.GetComponent<DragAndDropObj>().StartCoroutine(receivedSlot.GetComponent<DragAndDropObj>().BackToDefaultPosition(receivedSlot.transform, receivedSlot.GetComponent<DragAndDropObj>().backAnimVelocity, receivedSlot.GetComponent<DragAndDropObj>().pivot.transform));
            receivedSlot.GetComponent<DragAndDropObj>().currentSlot = null;
            checkedSlot = false;
            receivedSlot = null;
        } 
    }


    public void ChangeKeepScale(bool value)
    {
        resetScaleOnSendBack = value;
    }

    public void SetReceivedSlotPivot()
    {
        if (receivedSlot != null)
        {
            if (resetScaleOnSendBack)
            {
                receivedSlot.GetComponent<DragAndDropObj>().resetScaleOnSendBack = true;
            }
            else
            {
                receivedSlot.GetComponent<DragAndDropObj>().resetScaleOnSendBack = false;
            }
            receivedSlot.GetComponent<DragAndDropObj>().pivot = GetComponent<RectTransform>();
            if (reactivateDragRaycast)
            {
                receivedSlot.GetComponent<Image>().raycastTarget = true;
            }
        }
    }

    public void ClearReceivedSlot() //Geralmente usado para assegurar que a referência a um drag seja perdida
    {
        receivedSlot = null;
    }

    public void PasteColorOnReceivedSlot() //faz o draggavell contido receber a defaultColor deste slot
    {
        if (receivedSlot != null)
        {
            receivedSlot.GetComponent<Image>().color = defaultColor;
        }
    }

    public void SetReceivedSlotPosition()
    {
        if (receivedSlot != null)
        {
            receivedSlot.GetComponent<RectTransform>().SetPositionAndRotation(GetComponent<RectTransform>().position, GetComponent<RectTransform>().localRotation);
            if (reactivateDragRaycast)
            {
                receivedSlot.GetComponent<Image>().raycastTarget = true;
            }
        }
    }

    public UnityEvent OnCheckedSlot;
}