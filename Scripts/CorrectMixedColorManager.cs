using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Fungus;

namespace Arcolabs.Disney
{
	public class CorrectMixedColorManager : MonoBehaviour
	{
		[SerializeField]
		private int winGames = 3;
		private int currentGamesWon = 0;

		[SerializeField]
		[OnValueChanged("ChangePotionsColors")]
		private MixedColor mixedColor;

		[SerializeField]
		private Image firstPotion;
		[SerializeField]
		private Image secondPotion;

		[SerializeField]
		private Flowchart gameFlowchart;

		[SerializeField]
		private Image[] randomColorObjects;
		[SerializeField]
		[ReadOnly]
		private List<Color> randomColorOptions;

		[SerializeField]
		private MixedColor[] mixColors;

		private Color defaultColor = new Color(1f, 1f, 1f, .1f);
		private Image image;

		private void Awake()
		{
			image = GetComponent<Image>();
			GetIndividualColors();
		}

		[Button]
		public void GetIndividualColors()
		{
			randomColorOptions = new List<Color>();
			for (int i = 0; i < mixColors.Length; i++)
			{
				bool hasFirst = false;
				bool hasSecond = false;
				for (int k = 0; k < randomColorOptions.Count; k++)
				{
					if (randomColorOptions[k] == mixColors[i].FirstColor)
					{
						hasFirst = true;
					}
					if (randomColorOptions[k] == mixColors[i].SecondColor)
					{
						hasSecond = true;
					}
				}
				if (!hasFirst)
				{
					randomColorOptions.Add(mixColors[i].FirstColor);
				}
				if (!hasSecond)
				{
					randomColorOptions.Add(mixColors[i].SecondColor);
				}
			}
		}

		[ExecuteInEditMode]
		private void ChangePotionsColors()
		{
			firstPotion.color = mixedColor.FirstColor;
			secondPotion.color = mixedColor.SecondColor;
			ResetBigPotion();
			// we change the color after the experiment
			// GetComponent<Image>().color = mixedColor.color;
		}

		public void ResetBigPotion()
		{
			Debug.Log("Resetting to default color");
			image.color = defaultColor;
		}

		public void PlayColorAudio()
		{
			AudioManager.instance.PlaySound(mixedColor.AudioName);
		}

		public void ChangeMixedColor(MixedColor newMixedColor)
		{
			mixedColor = newMixedColor;
			ChangePotionsColors();
		}

		public void ChangeToRandomMixedColor()
		{
			int randomIndex = Random.Range(0, mixColors.Length - 1);
			while (mixColors[randomIndex] == mixedColor)
			{
				randomIndex = Random.Range(0, mixColors.Length - 1);
			}
			mixedColor = mixColors[randomIndex];
			ChangePotionsColors();
			int randomObjectIndex = 0;
			for (int i = 0; i < randomColorOptions.Count; i++)
			{
				if (randomColorOptions[i] != mixedColor.FirstColor && randomColorOptions[i] != mixedColor.SecondColor)
				{
					randomColorObjects[randomObjectIndex++].color = randomColorOptions[i];
				}
			}
		}

		public void PrepareNextGame()
		{
			currentGamesWon++;
			if (currentGamesWon < winGames)
			{
				gameFlowchart.ExecuteBlock("Random Color");
			}
			else
			{
				currentGamesWon = 0;
				gameFlowchart.ExecuteBlock("Continue");
			}
		}

		public void ChangeColor(Image go)
		{
			if (image.color.a < .2f)
			{
				if (go == firstPotion.gameObject)
				{
					StartCoroutine(FadeColor(firstPotion.color));
				}
				else
				{
					StartCoroutine(FadeColor(secondPotion.color));
				}
			}
			else
			{
				StartCoroutine(FadeColor(mixedColor.color));
			}
		}

		public void ChangeColor()
		{
			StopAllCoroutines();
			if (image.color.a < .2f)
			{
				// then it's the first color
				StartCoroutine(FadeColor(firstPotion.color));
			}
			else
			{
				Debug.Log("Second color mix");
				// then it's the second color
				StartCoroutine(FadeColor(mixedColor.color));
			}
		}

		public IEnumerator FadeColor(Color newColor)
		{
			Debug.Log("Changing color");
			Color c = image.color;
			while (c != newColor)
			{
				Debug.Log("Looping");
				c = Color.Lerp(c, newColor, .1f);
				image.color = c;
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
