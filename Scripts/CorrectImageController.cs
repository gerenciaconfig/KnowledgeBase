using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney.Patterns
{
	public enum CorrectOptionType
	{
		Image, Text
	}

	public class CorrectImageController : GameController
	{
		[SerializeField]
		[OnValueChanged("OnItemTypeChanged")]
		private CorrectOptionType itemType;

		[SerializeField]
		[ShowIf("itemType", CorrectOptionType.Text)]
		private string[] textOptions;

		[SerializeField]
		[ShowIf("itemType", CorrectOptionType.Image)]
		private Sprite[] imageOptions;

		private void OnItemTypeChanged()
		{
			if (itemType == CorrectOptionType.Image)
			{
				textOptions = null;
			}
			else if (itemType == CorrectOptionType.Text)
			{
				imageOptions = null;
			}
		}
	}

}
