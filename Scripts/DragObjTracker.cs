using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DragObjTracker : MonoBehaviour
{
    [SerializeField]
    private List<DragAndDropFree> notPlacedDragList = new List<DragAndDropFree>();

    [SerializeField]
    private List<DragAndDropFree> placedDragList = new List<DragAndDropFree>();

    [SerializeField]
    private Flowchart flowChart;
    [SerializeField]
    private string ifAnyPlacedBlockName;
    [SerializeField]
    private string ifNonePlacedBlockName;

    private void OnEnable()
    {
        ResetList();
    }

    public void CheckDrags()
    {
        /*foreach (DragAndDropFree item in notPlacedDragList)
        {
            if (item.placed == true)
            {
                if (!placedDragList.Contains(item))
                {
                    placedDragList.Add(item);
                }
                if (notPlacedDragList.Contains(item))
                {
                    notPlacedDragList.Remove(item);
                }
            } else
            {
                if (!notPlacedDragList.Contains(item))
                {
                    notPlacedDragList.Add(item);
                }
                if (placedDragList.Contains(item))
                {
                    placedDragList.Remove(item);
                }
            }
        }*/
        for (int i = notPlacedDragList.Count - 1; i >= 0; i--)
        {
            if (notPlacedDragList[i].placed == true)
            {
                if (!placedDragList.Contains(notPlacedDragList[i]))
                {
                    placedDragList.Add(notPlacedDragList[i]);
                }
                notPlacedDragList.RemoveAt(i);
            } else
            {
                if (placedDragList.Contains(notPlacedDragList[i]))
                {
                    placedDragList.RemoveAt(i);
                }
            }
        }

        if (placedDragList.Count > 0)
        {
            flowChart.ExecuteBlock(ifAnyPlacedBlockName);
        } else
        {
            flowChart.ExecuteBlock(ifNonePlacedBlockName);
        }
    }

    public void ResetList()
    {
        if (placedDragList.Count > 0)
        {
            for (int i = 0; i < placedDragList.Count; i++)
            {
                if (!notPlacedDragList.Contains(placedDragList[i]))
                {
                    placedDragList[i].placed = false;
                    notPlacedDragList.Add(placedDragList[i]);
                }   
            }
            placedDragList.Clear();
        }    
    }

    public void FadeNotPlacedDrags()
    {
        foreach (DragAndDropFree item in notPlacedDragList)
        {
            item.FadeObj();
        }
    }

    public void FadePlacedDrags()
    {
        foreach (DragAndDropFree item in placedDragList)
        {
            item.FadeObj();
        }
    }
}
