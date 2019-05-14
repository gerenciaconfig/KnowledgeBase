using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fungus;

public class OnEnableCallBlock : MonoBehaviour
{

    //[SerializeField] private UnityEvent onStartEvent;
    [SerializeField] private BlockReference onStartBlock;
    [SerializeField] private float delay;

    void OnEnable()
    {
        StartCoroutine(DelayTimer());
    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(delay);
        onStartBlock.Execute();
        //onStartEvent.Invoke();
    }

}