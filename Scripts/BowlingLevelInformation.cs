using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney
{
	[CreateAssetMenu(fileName = "New BowlingLevelInformation", menuName = "Arcolabs/Disney/New Bowling Level Information")]
	public class BowlingLevelInformation : ScriptableObject
	{
		public Sprite[] options;

		BowlingAssetInformation.GameAsset portrait;

		[OnValueChanged("PinsRange")]
		public int pinsDown = 2;

		[OnValueChanged("CorrectOptionRange")]
		public int correctOption = 0;
		[ReadOnly]
		[SerializeField]
		private string optionName;

		public string audioDescription;

		public void PinsRange()
		{
			pinsDown = Mathf.Clamp(pinsDown, 2, 12);
		}



		public void CorrectOptionRange()
		{
			correctOption = Mathf.Clamp(correctOption, 0, options.Length - 1);
			optionName = options[correctOption].name;
		}

		
	}
}
