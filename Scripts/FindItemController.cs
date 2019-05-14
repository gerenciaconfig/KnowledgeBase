using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

namespace Arcolabs.Disney.Patterns
{
	[System.Serializable]
	class SelectableObject
	{
		public Vector3 position;
		public Sprite sprite;
		public string signal;
	}

	public class FindItemController : GameController
	{
		[SerializeField]
		[OnValueChanged("ToggleItems")]
		private bool startHidden;

		// [SerializeField]
		private SelectableObject[] items;

		[SerializeField]
		[ReadOnly]
		private GameObject[] observers;

		[Button]
		private void GetChildButtonObjects()
		{
			Transform childrenContainer = GetComponent<PointAndClickController>().ItemsContainer.transform;
			int count = childrenContainer.childCount;
			GetComponent<PointAndClickController>().VictoryCount = count;

			observers = new GameObject[count];
			for (int i = 0; i < count; i++)
			{
				observers[i] = childrenContainer.GetChild(i).gameObject;
			}
		}

		public void ResetHints()
		{
			foreach (GameObject go in observers)
			{
				go.GetComponent<PointAndClickObserver>().ResetHint();
			}
		}

		private void ToggleItems()
		{
			for (int i = 0; i < observers.Length; i++)
			{
				observers[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, startHidden ? 0f : 1f);
			}
		}

		// [Button("Generate Objects!")]
		private void GenerateObjects()
		{
			if (observers != null)
			{
				// Destroy old objects
				for (int i = observers.Length - 1; i >= 0; i--)
				{
					DestroyImmediate(observers[i]);
				}
			}
			
			// Create new objects
			observers = new GameObject[items.Length];
			for (int i = 0; i < observers.Length; i++)
			{
				observers[i] = Instantiate(new GameObject(), itemsContainer.transform);
				observers[i].name = "Item " + i;
				observers[i].AddComponent(typeof(Image));
				observers[i].AddComponent(typeof(PointAndClickObserver));

				observers[i].transform.position = items[i].position;
				observers[i].GetComponent<Image>().sprite = items[i].sprite;
				observers[i].GetComponent<PointAndClickObserver>().SetObserverName(items[i].signal);

				SnapAnchors(observers[i]);
			}
		}
	}
}
