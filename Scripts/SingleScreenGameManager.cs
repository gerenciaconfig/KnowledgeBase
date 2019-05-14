using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcolabs.Disney
{
	public class SingleScreenGameManager : MonoBehaviour
	{
		// current screen
		// do something
		// maybe nothing happens (even so something happens)
		// if something happens
		//	do something else
		//	change screen
		//  is there more game? repeat everything
		//  else change screen
		[SerializeField]
		private Image portrait;
		[SerializeField]
		private Transform optionsContainer;
		private Image[] options;
		[SerializeField]
		private PinManager pinManager;
		[SerializeField]
		private BowlingLevelInformation[] levels;

		private int currentLevelIndex = 1;

		private void Start()
		{
			Debug.Log("Starting off...");
			GetOptionsImages();
			Patterns.ButtonObserver.OnObserverInteraction += GetNotification;
			UpdateLevelInformation();
		}

		private void UpdateLevelInformation()
		{
			Debug.Log("Updating the level");
			// portrait.sprite = levels[currentLevelIndex].portrait;
			for (int i = 0; i < options.Length; i++)
			{
				options[i].sprite = levels[currentLevelIndex].options[i];
			}
		}

		private void GetNotification(Patterns.ButtonObserver observer)
		{
			Debug.Log("Got notified by observer with name: " + observer.ObserverName);
			// TODO turn this into string comparison to avoid the TryParse?
			int correctOption;
			int.TryParse(observer.ObserverName, out correctOption);
			if (correctOption == levels[currentLevelIndex].correctOption)
			{
				// Correct! On to the next screen
				currentLevelIndex++;
				if (currentLevelIndex < levels.Length)
				{
					UpdateLevelInformation();
				}
				else
				{
					// turn this off and turn the play again on
					MainScreenManager.instance.SwitchScreens(Screens.PlayAgain);
				}
			}
			else
			{
				// Wrong! Tell how dumb he is (in a polite way)
			}
		}

		private void GetOptionsImages()
		{
			options = new Image[optionsContainer.childCount];
			for (int i = 0; i < optionsContainer.childCount; i++)
			{
				options[i] = optionsContainer.GetChild(i).GetComponent<Image>();
			}
		}
	}
}
