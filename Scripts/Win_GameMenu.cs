using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_GameMenu : MonoBehaviour
{
	public static int resID;	// ID current size (0 = 2x2, 1=3x3 ...)

	public Transform rootImages;	
	public UIImageItem prefImage;
    public Button backButton;
    [Space(10)]
    public Text labelTitle;
	public Text labelSizeX;
	public Text labelSizeY;

    private GameObject gridSelection;

    private Sprite selectedImage;


    void Awake()
	{
		// display the current size
		labelSizeX.text = GamePuzzle.piecesX.ToString();
		labelSizeY.text = GamePuzzle.piecesY.ToString();

		// subscribe to image selection Event
		EventDispatcher.Add(EventName.GameSelectImage, GameSelectImage);

		gridSelection = GameObject.Find("Grid Selection");
	}

    private void OnEnable()
    {
        gridSelection.SetActive(false);
    }

    void Start()
	{
		UIRoot.Show(gameObject);
        backButton.onClick.AddListener(DeactivateGameMenu);
	}

	// set game data when loading windows
	void SetData(object[] args)
	{
		GameType game = (GameType)args [0];

		labelTitle.text = game.title;	// название

		// картинки
		foreach (Sprite image in game.images)
		{
			UIImageItem item = Lib.AddObject<UIImageItem>(prefImage, rootImages);
			item.SetData(image);
		}
	}

	// button MainMenu
	public void BackMainMenu()
	{
		UIRoot.Close(gameObject);
		UIRoot.Load(WindowName.Win_MainMenu);
	}

	// button Prev X
	public void PrevSizeX()
	{
		GamePuzzle.piecesX--;
		if (GamePuzzle.piecesX < 2)
			GamePuzzle.piecesX = 2;

		labelSizeX.text = GamePuzzle.piecesX.ToString();
	}

	// button Next X
	public void NextSizeX()
	{
		GamePuzzle.piecesX++;
		if (GamePuzzle.piecesX > 18)
			GamePuzzle.piecesX = 18;
		
		labelSizeX.text = GamePuzzle.piecesX.ToString();
	}

	// button Prev Y
	public void PrevSizeY()
	{
		GamePuzzle.piecesY--;
		if (GamePuzzle.piecesY < 2)
			GamePuzzle.piecesY = 2;
		
		labelSizeY.text = GamePuzzle.piecesY.ToString();
	}
	
	// button Next Y
	public void NextSizeY()
	{
		GamePuzzle.piecesY++;
		if (GamePuzzle.piecesY > 18)
			GamePuzzle.piecesY = 18;
		
		labelSizeY.text = GamePuzzle.piecesY.ToString();
	}
	
	// Event selection of pictures
	void GameSelectImage(object[] args)
	{
        selectedImage = (Sprite)args [0];

        EnableGridSelection();
	}

    void EnableGridSelection()
    {
        gridSelection.SetActive(true);
		AudioPlayer.changeBtn = true;
		AudioManager.instance.StopAllSounds();
		AudioManager.instance.PlaySound("Pergunta 2");
    }
    
     public void DeactivateGameMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void StartPuzzle()
    {
		gridSelection.SetActive(false);
        // load the game with the selected picture
        EventDispatcher.SendEvent(EventName.BoardStartGame, selectedImage);
    }
}