using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePuzzle : MonoBehaviour
{
	public static int gameID;		// The current game ID
	public static Sprite image;		// current picture
	public static int piecesX;		// the number of pieces on the side
	public static int piecesY;		// the number of pieces on the side
    

    [Range(2,18)]
    public int puzzleSizeX = 6;
    [Range(2, 18)]
    public int puzzleSizeY = 4;

    public Content content;			// object games content
	public Transform rootScene;		// stage root for loading games
    //public GameObject congratsScreen;



	void Awake()
	{
        GamePuzzle.piecesX = puzzleSizeX;
        GamePuzzle.piecesY = puzzleSizeY;

        // subscription events
        EventDispatcher.Add(EventName.GameSelect, GameSelect);
		EventDispatcher.Add(EventName.BoardStartGame, BoardStartGame);
		EventDispatcher.Add(EventName.GameOver, GameOver);
	}

	void Start ()
	{
        // load MainMenu window
        //UIRoot.Load (WindowName.Win_MainMenu);
        

        EventDispatcher.SendEvent(EventName.GameSelect, 1); // value = ID game
    }

    void GameSelect(object[] args)
	{
		// select ID game
		gameID = (int)args [0];
	}

	void BoardStartGame(object[] args)
	{
		// load game
		image = (Sprite)args [0];	// picture

		Lib.RemoveObjects(rootScene);
		// load the game from the content list
		Board board = Lib.AddObject<Board>(content.games [gameID].board, rootScene);
		board.SendMessage("SetData", SendMessageOptions.DontRequireReceiver);

		// load the game interface window
		UIRoot.CloseAll();
		UIRoot.Load(WindowName.Win_Board);
	}

	void GameOver()
	{
        // GameOver show window
        //UIRoot.Load(WindowName.Win_GameOver);

		/*if(congratsScreen != null)
	        congratsScreen.SetActive(true);        
		else 
			Debug.LogAssertion("congratsScreen is null");*/
	}

	public void ChangePuzzleSize(int sizeX, int sizeY) {

		puzzleSizeX = sizeX;
		puzzleSizeY = sizeY;
		
		GamePuzzle.piecesX = puzzleSizeX;
		GamePuzzle.piecesY = puzzleSizeY;
	}


}
