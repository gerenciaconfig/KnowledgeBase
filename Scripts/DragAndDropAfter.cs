using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropAfter : DragAndDropLogic
{

    private void OnEnable()
    {
        instance = this;
        ResetGameProperties();
        CheckAnim();

        StartCoroutine(PivotChange());               
    }

    IEnumerator PivotChange()
    {
        yield return new WaitForSeconds(2);

        foreach (var item in dragObjects)
        {
            item.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }

        foreach (var item in dragPivots)
        {
            item.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }
    }

    public void PivotReturn()
    {
        foreach (var item in dragObjects)
        {
            item.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.09f);
        }

        foreach (var item in dragPivots)
        {
            item.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.09f);
        }
    }
}
