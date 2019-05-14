using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.General;

namespace Arcolabs.Disney.Patterns
{
	public class PointAndClickScreenManager : MonoBehaviour
	{
		ScreenTransition screenTransition;

		public void Start()
		{
			screenTransition = GetComponent<ScreenTransition>();
			// StartCoroutine(screenTransition.Transition(transform.GetChild(0).gameObject, transform.GetChild(0).gameObject));
			
		}
	}
}
