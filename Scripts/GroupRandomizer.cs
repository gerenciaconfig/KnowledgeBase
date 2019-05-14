using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Randomizes the children components
/// </summary>
public class GroupRandomizer : MonoBehaviour
{
	[SerializeField]
	private bool alwaysRandomize = false;

	private void OnValidate()
	{
		if (alwaysRandomize)
		{
			RandomizeNow();
		}
	}

	[Button]
	public void RandomizeNow()
	{
		int count = transform.childCount;
		int i = 0;
		while (i < count)
		{
			int r = Random.Range(0, count - 1);
			transform.GetChild(i).SetSiblingIndex(r);
			i++;
		}
	}
}
