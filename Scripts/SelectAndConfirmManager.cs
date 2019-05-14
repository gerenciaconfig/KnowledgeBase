using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class SelectAndConfirmManager : MonoBehaviour 
{
	public Game game;
	public GameObject selectedBtn;
	public GameObject firstSelected;

	public string wrongAudioName;


	public void CheckAnswer()
	{
		if(selectedBtn.GetComponent<SelectAndConfirmBtn>().buttonType == SelectAndConfirmBtn.ButtonType.RightButton)
		{
			AudioManager.instance.PlaySound("Right Click");
			game.AddVictory(true);
		}
		else
		{
			AudioManager.instance.PlaySound(wrongAudioName);
		}
	}

	public void CheckSelected()
	{
		if(firstSelected == null)
		{
			firstSelected = selectedBtn;
		}

		if(firstSelected != selectedBtn)
		{
			firstSelected.GetComponent<SelectAndConfirmBtn>().selectedImg.enabled = false;
			firstSelected.GetComponent<SelectAndConfirmBtn>().selected = false;
			firstSelected = selectedBtn;
		}
	}

	public void ResetAll()
	{
		firstSelected.GetComponent<SelectAndConfirmBtn>().selectedImg.enabled = false;
		firstSelected.GetComponent<SelectAndConfirmBtn>().selected = false;
		selectedBtn.GetComponent<SelectAndConfirmBtn>().selectedImg.enabled = false;
		selectedBtn.GetComponent<SelectAndConfirmBtn>().selected = false;

		firstSelected = null;
		selectedBtn = null;
	}
}
