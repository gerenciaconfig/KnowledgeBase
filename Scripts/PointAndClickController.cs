using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// <summary>
/// 
/// </summary>
namespace Arcolabs.Disney.Patterns
{
	public enum PointAndClickType
	{
		None, FindItem, CorrectOption
	}

	public class PointAndClickController : MonoBehaviour
	{
		[SerializeField]
		[OnValueChanged("OnContainerGameObjectChanged")]
		private GameObject itemsContainer;
		public GameObject ItemsContainer { get { return itemsContainer; } }

		[SerializeField]
		[ShowIf("itemsContainer", typeof(GameObject))]
		[OnValueChanged("OnScreenTypeChanged")]
		private PointAndClickType screenType;
		public PointAndClickType Type { get { return screenType; } }

		[SerializeField]
		private string audioIdentifier;
		public string AudioIdentifier { get { return audioIdentifier; } }

		// [SerializeField]
		private string audioDescriptionIn;
		// public string AudioDescriptionIn { get { return audioDescriptionIn; } }
		// [SerializeField]
		private string audioDescriptionOut;
		// public string AudioDescriptionOut { get { return audioDescriptionOut; } }

		[SerializeField]
		[ReadOnly]
		private int currentVictories = 0;
		public int CurrentVictories { get { return currentVictories; } }

		[SerializeField]
		[ReadOnly]
		private int itemsForVictory = 1;
		public int VictoryCount
		{
			set
			{
				itemsForVictory = Mathf.Clamp(value, 1, 20);
			}
			get
			{
				return itemsForVictory;
			}
		}

		public bool Finished { get { return currentVictories >= VictoryCount; } }

		private GameObject item;
		private PointAndClickObserver observer;

		[Button]
		private void SelectFirstChild()
		{
			itemsContainer = transform.GetChild(0).gameObject;
			OnContainerGameObjectChanged();
		}

		private void OnContainerGameObjectChanged()
		{
			screenType = PointAndClickType.None;
			OnScreenTypeChanged();
		}

		private void OnScreenTypeChanged()
		{
			if (screenType == PointAndClickType.None)
			{
				DestroyImmediate(GetComponent<CorrectImageController>());
				DestroyImmediate(GetComponent<FindItemController>());
			}
			else if (screenType == PointAndClickType.FindItem)
			{
				DestroyImmediate(GetComponent<CorrectImageController>());
				gameObject.AddComponent(typeof(FindItemController));
				GetComponent<FindItemController>().ItemsContainer = itemsContainer;
			}
			else if (screenType == PointAndClickType.CorrectOption)
			{
				DestroyImmediate(GetComponent<FindItemController>());
				gameObject.AddComponent(typeof(CorrectImageController));
			}
		}

		public void RequestResetHints()
		{
			Debug.Log("Requesting to stop hints!");
			GetComponent<FindItemController>().ResetHints();
		}

		public void AddVictory()
		{
			Debug.Log("Called!");
			currentVictories++;
		}

		private void OnEnable()
		{
			ResetScreen();
		}

		public void ResetScreen()
		{
			// make all items transparent
			// reset victory count
			currentVictories = 0;
		}
	}
}
