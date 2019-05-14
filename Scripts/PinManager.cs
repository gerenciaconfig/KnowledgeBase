using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
	private GameObject[] pins;

	private void Awake()
	{
		GetChildPins();
	}

	

	private void GetChildPins()
	{
		int count = transform.childCount;
		pins = new GameObject[count];
		for (int i = count - 1; i >= 0; i--)
		{
			pins[count - i - 1] = transform.GetChild(i).gameObject;
		}
	}

	public void KnockPin(int index)
	{

	}

	public void ResetPins()
	{

	}
}
