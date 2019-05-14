using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformGoToPosition : MonoBehaviour
{
	private RectTransform rect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	public void ChangePosition(RectTransform to)
	{
		rect.position = to.position;
	}
}
