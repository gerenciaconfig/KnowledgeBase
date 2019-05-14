using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnableEvent;
	[SerializeField] private float delay;

    private void OnEnable()
    {
		if (delay == 0)
		{
			onEnableEvent.Invoke();
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(DelayEvent());
		}
    }

	private IEnumerator DelayEvent()
	{
		yield return new WaitForSeconds(delay);
		onEnableEvent.Invoke();
	}

}
