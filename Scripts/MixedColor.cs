using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney
{
	public enum BaseColor
	{
		Red, Green, Blue, Yellow, White
	}
	[CreateAssetMenu(fileName = "New Mixed Color", menuName = "Arcolabs/New Mixed Color")]
	public class MixedColor : ScriptableObject
	{
		public Color color = Color.black;

		[OnValueChanged("OnFirstColorChanged")]
		public BaseColor firstBaseColor;
		[OnValueChanged("OnSecondColorChanged")]
		public BaseColor secondBaseColor;

		[SerializeField]
		private Color firstColor;
		public Color FirstColor { get { return firstColor; } }

		[SerializeField]
		private Color secondColor;
		public Color SecondColor { get { return secondColor; } }

		[SerializeField]
		private string audioName;
		public string AudioName { get { return audioName; } }

		private void OnFirstColorChanged()
		{
			firstColor = GetColorFromBaseColor(firstBaseColor);
			Debug.Log("Changed first color to: " + firstColor);
		}

		private void OnSecondColorChanged()
		{
			secondColor = GetColorFromBaseColor(secondBaseColor);
			Debug.Log("Changed second color to: " + secondColor);
		}

		private Color GetColorFromBaseColor(BaseColor c)
		{
			if (c == BaseColor.Red)
			{
				return new Color(236 / 255f, 28 / 255f, 36 / 255f); // Color.red;
			}
			else if (c == BaseColor.Green)
			{
				return new Color(34 / 255f, 180 / 255f, 115 / 255f); // Color.green;
			}
			else if (c == BaseColor.Blue)
			{
				return new Color(41 / 255f, 170 / 255f, 225 / 255f); // Color.blue;
			}
			else if (c == BaseColor.White)
			{
				return Color.white;
			}
			else if (c == BaseColor.Yellow)
			{
				return new Color(251 / 255f, 237 / 255f, 33 / 255f);  // Color.yellow;
			}
			return Color.black;
		}
	}

}
