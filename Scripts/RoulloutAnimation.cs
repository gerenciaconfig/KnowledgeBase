using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoulloutAnimation : MonoBehaviour
{

	[SerializeField] private RolloutObject objectToRoll;

	private LayoutGroup layout;

	public void RollObject()
	{	
		BookPage.currentPage.ResetTextElement();

		if ((layout = transform.GetChild(0).GetComponent<LayoutGroup>()) != null)
			layout.enabled = false;

		StartCoroutine(RolloutEndMenu.GoToTargetPosition(objectToRoll, 1));
	}

}
