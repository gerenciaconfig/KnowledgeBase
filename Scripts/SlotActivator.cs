using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotActivator : MonoBehaviour 
{
	public bool haveNext;
	public Image nextSlot;

	public void OnEnable()
	{
		nextSlot.enabled = false;
	}

	public void ActivateNext()
	{
		if(haveNext)
		{
			nextSlot.enabled = true;
		}
	}

}