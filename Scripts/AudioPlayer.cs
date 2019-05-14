using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class AudioPlayer : MonoBehaviour 
{
    public enum GameState {Playing, Ending};

	public GameState gameState;

	public AudioManager am;
	public Win_Board wb;
	public string audioName;

	public Image finalImg;

	public static bool startGame = false;
	public static bool endGame = false;

	public static bool changeBtn = false;

	//public GameObject btns;

	public GameObject closeGame;
	public GameObject closeSelect;

	//public PuzzleSize ps;

	public Flowchart flow;

	public Flowchart menuFlow;

	public GameObject gridSelection;

	public ParticleManager particle;

	public GameObject boardChild;

	void Update()
	{
		/*if(startGame)
		{
			Debug.Log("aqui");
			am.StopAllSounds();
			PlayVoice();
			startGame = false;
		}

		if(endGame)
		{
			AudioManager.instance.StopAllSounds();
			AudioManager.instance.PlaySound("Congrats");
			particle.StartParticlesFromBeginning();
			menuFlow.ExecuteBlock("Start Play Again");
			endGame = false;	
		}*/

		if(changeBtn)
		{
			ChangeButton();
			changeBtn = false;
		}
	}

	public void PlayVoice()
	{
		flow.ExecuteBlock("Game");
		closeSelect.SetActive(false);
		closeGame.SetActive(true);
		//btns.SetActive(false);
		am.PlayAudioDescription(audioName);
	}

	public void PlayButtonAudio()
	{
		am.PlaySound("Button Click");
	}

	public void Reset()
	{
		//ps.ResetSize();
		endGame = false;
		changeBtn = false;
		am.StopAllSounds();
		gridSelection.SetActive(true);
		wb.BackGameMenu();
		DeleteBoard();
	}

	public void ChangeImage()
	{
		finalImg.GetComponent<Image>().sprite = GamePuzzle.image;
	}

	public void StopSounds()
	{
		am.StopAllSounds();
	}

	public void DeleteBoard()
	{
		boardChild = GameObject.Find("Board Puzzle(Clone)");
		Destroy(boardChild);
	}

	public void ChangeButton()
	{
		flow.ExecuteBlock("ChangeButton");
	}

	public void ChangeGameState()
	{
		switch(gameState)
		{
			case GameState.Playing:
			am.StopAllSounds();
			PlayVoice();
			startGame = false;
			break;

			case GameState.Ending:
			AudioManager.instance.StopAllSounds();
			AudioManager.instance.PlaySound("Congrats");
			particle.StartParticlesFromBeginning();
			menuFlow.ExecuteBlock("Start Play Again");
			endGame = false;
			break;
		}
	}
}