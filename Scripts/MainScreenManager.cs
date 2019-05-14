using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Disney
{
	public enum Screens
	{
		Menu, Game, PlayAgain
	}

	public class MainScreenManager : MonoBehaviour
	{
		public static MainScreenManager instance;
		[SerializeField]
		private GameObject mainMenu;
		[SerializeField]
		private GameObject mainGame;
		[SerializeField]
		private GameObject playAgain;

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
			
		}

		public void SwitchScreens(Screens s) 
		{
			mainMenu.SetActive(s == Screens.Menu);
			mainGame.SetActive(s == Screens.Game);
			playAgain.SetActive(s == Screens.PlayAgain);
		}
	}
}
