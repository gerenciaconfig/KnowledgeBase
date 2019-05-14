using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropPositionChanger : MonoBehaviour
{
    public GameObject parObjetct;

    public GameObject pivot1;
    public GameObject pivot2;

    public GameObject localPivot1;
    public GameObject localPivot2;


    public void ChangePosition()
    {
        if(this.parObjetct.transform.position == pivot1.transform.position)
        {
            this.transform.position = localPivot1.transform.position;
            this.GetComponent<DragAndDropObj>().pivot.transform.position = localPivot1.transform.position;
            this.GetComponent<DragAndDropObj>().savedPosition = localPivot1.transform.position;
        }
        else if(this.parObjetct.transform.position == pivot2.transform.position)
        {
            this.transform.position = localPivot2.transform.position;
            this.GetComponent<DragAndDropObj>().pivot.transform.position = localPivot2.transform.position;
            this.GetComponent<DragAndDropObj>().savedPosition = localPivot2.transform.position;
        }
    }
}