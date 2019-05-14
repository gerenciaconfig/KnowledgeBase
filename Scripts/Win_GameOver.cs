using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_GameOver : MonoBehaviour
{
	void Awake()
	{
	}

	void Start()
	{
		UIRoot.Show(gameObject);
	}

	public void BackMainMenu()
	{
		UIRoot.CloseAll();
		UIRoot.Load(WindowName.Win_MainMenu);
	}

	public void Replay()
	{
		UIRoot.Close(gameObject);
		EventDispatcher.SendEvent(EventName.BoardStartGame, GamePuzzle.image);
	}

	public void NextPic()
	{
		UIRoot.Close(gameObject);
		GamePuzzle.image = Content.currentGame.GetNextPic(GamePuzzle.image);
		EventDispatcher.SendEvent(EventName.BoardStartGame, GamePuzzle.image);
	}
	
	public void BackGameMenu()
	{
		EventDispatcher.SendEvent(EventName.GameSelect, GamePuzzle.gameID);
	}
}
