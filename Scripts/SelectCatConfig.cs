using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Game", "Select Cat Configuration", "Bla bla bla")]
public class SelectCatConfig : Command 
{
	public Sprite backgroundSprite;

    public GameObject backgroundGObj;

    [Space]

    [SerializeField]

    [Space]

    public Image background;
    public Button catButton;
    public Button catButton1;
    public Button catButton2;

    public override void OnEnter()
    {
        //background.sprite = backgroundSprite;
        backgroundGObj.SetActive(true);

         Continue();
    }
}