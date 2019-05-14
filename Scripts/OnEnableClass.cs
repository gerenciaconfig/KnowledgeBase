using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableClass : MonoBehaviour {
    public UnityEvent OnEnableThis;

    private void OnEnable()
    {
        OnEnableThis.Invoke();
    }
}
