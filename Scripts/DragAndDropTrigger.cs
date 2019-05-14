using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class DragAndDropTrigger : EventTrigger
{
    private DragAndDropObj dragObj;
    private DragAndDropFree dragObjF;

    //[SerializeField] private bool dragNDropFree;

    void Awake()
    {
        dragObj = GetComponent<DragAndDropObj>();
        if (dragObj.GetType() == typeof(DragAndDropFree))
            dragObjF = (DragAndDropFree)dragObj;

    }

    public override void OnDrag(PointerEventData data)
    {
        dragObj.PointerDrag();
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if(dragObjF)
            dragObjF.PointerEndDrag();
        else
            dragObj.PointerEndDrag();
    }

}
