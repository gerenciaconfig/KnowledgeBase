using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney.Patterns
{
	public class PointAndClickButtonGrouper : MonoBehaviour
	{
		[SerializeField]
		private GameObject buttonPrefab;

		[SerializeField]
		[ReadOnly]
		private PointAndClickObserver[] options;
		private int correctIndex = 0;
		private int correctOptionValue = 1;

		private static int[] randomIncrements = { -2, -1, 1, 2 };

		[Button]
		private void GetButtons()
		{
			int count = transform.childCount;

			options = new PointAndClickObserver[count];
			for (int i = 0; i < count; i++)
			{
				options[i] = transform.GetChild(i).GetComponent<PointAndClickObserver>();
				Debug.Log(options[i].ObserverName);
				if (options[i].ObserverName == "CorrectItem")
				{
					correctIndex = i;
					int.TryParse(options[i].GetComponentInChildren<Text>().text, out correctOptionValue);
					Debug.Log("CorrectItem index é " + correctIndex + " e o valor é " + correctOptionValue);
				}
			}
		}

		private void OnEnable()
		{
			//for (int i = 0; i < options.Length; i++)
			//{
			//	if (i != correctIndex)
			//	{
			//		int randomValue;
			//		if (correctOptionValue == 1)
			//		{
			//			randomValue = correctOptionValue + randomIncrements[Random.Range(2, 3)];
			//		}
			//		else if (correctOptionValue == 2)
			//		{
			//			randomValue = correctOptionValue + randomIncrements[Random.Range(1, 3)];
			//		}
			//		else
			//		{
			//			randomValue = correctOptionValue + randomIncrements[Random.Range(0, 3)];
			//		}
			//		options[i].GetComponentInChildren<Text>().text = randomValue.ToString();
			//	}
			//}
		}

		private void OnValidate()
		{
		
		}
	}
}
