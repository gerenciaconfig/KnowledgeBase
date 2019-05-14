using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathRenderJake : MonoBehaviour
{
    private LineRenderer lRender;

    private void Awake()
    {
        lRender = GetComponentInParent<LineRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(SetBobPosition());
        //transform.position = lRender.GetPosition(0);
        //lRender.startWidth = 1;
        //lRender.endWidth = 1;
    }

    private IEnumerator SetBobPosition()
    {
        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(0);

        transform.position = lRender.GetPosition(0);
    }

}
