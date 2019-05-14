using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Disney.Patterns
{	
	public class TracingInformation : MonoBehaviour
	{
		[SerializeField]
		private bool patetaOnTheLeft = true;
		public bool PatetaOnTheLeft { get { return patetaOnTheLeft; } }
		[SerializeField]
		private bool lastTracing = false;
		public bool IsLastTracing { get { return lastTracing;  } }

		public static event Action<TracingInformation> OnObserverInteraction;

		[SerializeField]
		private string introDescription = "";
		public string IntroAudioDescription { get { return introDescription; } }
		// [SerializeField]
		// private string outroDescription = "";
		// public string OutroAudioDescription { get { return outroDescription; } }

		private void Start()
		{
			introDescription = gameObject.name;
		}

		private void OnEnable()
		{
			//OnObserverInteraction?.Invoke(this);
		}
	}
}
