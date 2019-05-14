using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

namespace Arcolabs.Disney.Patterns
{
	public class ButtonObserver : MonoBehaviour
	{
		public static event Action<ButtonObserver> OnObserverInteraction;
		[SerializeField]
		private string observerName;
		public string ObserverName { get { return observerName; } }

		private Button button;
		[SerializeField]
		private Flowchart flowchart;

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		public void Notify()
		{
			// OnObserverInteraction?.Invoke(this);
		}

		public void UpdateBlockName(string newBlock)
		{
			// button.onClick.RemoveAllListeners();
			// button.onClick.AddListener(() => { Debug.Log("Calling new block!!!");  flowchart.ExecuteBlock(newBlock); });
			// Debug.Log("Updating block name: " + newBlock);
			observerName = newBlock;
		}

		public void PushButton()
		{
			// button.onClick.Invoke();
			Debug.Log("Calling block: " + observerName);
			flowchart.ExecuteBlock(observerName);
		}
	}
}

