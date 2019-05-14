using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component takes the first object and multiplies it
/// according to the amount value.
/// 
/// Best used with the Horizontal or Vertical LayoutGroup components.
/// </summary>
public class GridItemMultiplier : MonoBehaviour
{
	[SerializeField]
	[Range(1, 15)]
	private int items = 1;

	private void Awake()
	{
		items = transform.childCount;
		// UpdateChildren();
	}

	private void UpdateChildren()
	{
		int count = transform.childCount;
		if (items < count)
		{
			for (int i = count - 1; i >= items; i--)
			{
				// TODO encontrar forma alternativa de fazer essa troca de objetos
				//Debug.Log("Destroying object of index: " + i);
				//UnityEditor.EditorApplication.delayCall += () =>
				//{
				//	DestroyImmediate(transform.GetChild(i).gameObject);
				//};
			}
		}
		else if (items > count)
		{
			for (int i = count; i < items; i++)
			{
				Debug.Log("Instantiating!");
				Instantiate(transform.GetChild(0), transform);
			}
		}
	}

	public void OnValidate()
	{
		items = Mathf.Clamp(items, 1, 15);
		// UpdateChildren();
	}
}
