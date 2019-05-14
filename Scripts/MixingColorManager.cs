using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Fungus;

namespace Arcolabs.Disney
{
	public class MixingColorManager : MonoBehaviour
	{
		[SerializeField]
		private MixedColor[] mixColors;
		[SerializeField]
		private UnityEvent finishCallback;
		[SerializeField]
		private Flowchart flowchart;
		[SerializeField]
		private int victories = 3;
		private int currentVictories = 0;

		private Color defaultColor = new Color(1f, 1f, 1f, .1f);
		private Image image;

		private List<MixedColor> nextPossibleColors;
		private string colorAudioName;

		private void Awake()
		{
			image = GetComponent<Image>();
			nextPossibleColors = new List<MixedColor>();
		}

		private void OnEnable()
		{
			currentVictories = 0;
		}

		public void CheckFlow(string repeatBlock, string continueBlock)
		{
			currentVictories++;
			if(currentVictories < victories)
			{
				flowchart.ExecuteBlock(repeatBlock);
			}
			else
			{
				currentVictories = 0;
				flowchart.ExecuteBlock(continueBlock);
			}
			
		}

		public void MoveToBlockAccordingToColor(MixedColor color, string successBlock, string failBlock)
		{
			Debug.Log("Comparing colors! " + color.color + " " + image.color);
			if (color.color == image.color)
			{
				flowchart.ExecuteBlock(successBlock);
			}
			else
			{
				flowchart.ExecuteBlock(failBlock);
			}
		}

		public void ChangePotionColor(Image potion)
		{
			if (nextPossibleColors.Count == 0)
			{
				// change the color
				StartCoroutine(FadeColor(potion.color));
				// find the possible second colors and add it to a list
				foreach (MixedColor mixed in mixColors) 
				{
					if (mixed.FirstColor == potion.color || mixed.SecondColor == potion.color)
					{
						nextPossibleColors.Add(mixed);
					}
				}
				Debug.Log("Next Possible colors count: " + nextPossibleColors.Count);
			}
			else
			{
				Debug.Log("Selected potion color: " + potion.color);
				for (int i = 0; i < nextPossibleColors.Count; i++)
				{
					Debug.Log("Next Possible colors count: " + nextPossibleColors[i].FirstColor + " " + nextPossibleColors[i].SecondColor);
					if (nextPossibleColors[i].FirstColor == potion.color ||
						nextPossibleColors[i].SecondColor == potion.color)
					{
						// change the color to this one
						StartCoroutine(FadeColor(nextPossibleColors[i].color));
						colorAudioName = nextPossibleColors[i].AudioName;
						finishCallback.Invoke();
						break;
					}
				}
			}
		}

		public void PlayColorAudio()
		{
			flowchart.SetFloatVariable("narrationTime", AudioManager.instance.PlaySound(colorAudioName));
		}

		public void ResetBigPotion()
		{
			Debug.Log("Resetting big potion to default color");
			image.color = defaultColor;
			Debug.Log("Clearnign the possible colors list");
			nextPossibleColors.Clear();
		}

		public IEnumerator FadeColor(Color newColor)
		{
			Debug.Log("Changing color...");
			Color c = image.color;
			while (c != newColor)
			{
				c = Color.Lerp(c, newColor, .3f);
				image.color = c;
				yield return new WaitForEndOfFrame();
			}
			Debug.Log("Finished changing the color: " + image.color);
		}
	}
}
