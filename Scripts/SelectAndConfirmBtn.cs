using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAndConfirmBtn : NewButtonBehaviour 
{
	//public enum ButtonType { RightButton, WrongButton};
    //public ButtonType buttonType;
	public GameObject manager;

	public Image selectedImg;

	public bool selected = false;

	public void SelectButton()
	{
		this.selected = true;
		manager.GetComponent<SelectAndConfirmManager>().selectedBtn = this.gameObject;
		manager.GetComponent<SelectAndConfirmManager>().CheckSelected();
	}
}