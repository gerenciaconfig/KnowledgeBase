using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseImage : MonoBehaviour
{
    public static Image currentBorderSelection;

    public Image charImage;
    public Sprite cardSprite;
    public Color imageBaseColor;

    private void OnEnable()
    {
        ButtonSelection.instance.rightBtn.interactable = false;
    }

    public void SelectImage()
    {
        ButtonSelection.instance.rightBtn.interactable = true;

        ChooseGameLogic.instance.ChooseImage = cardSprite;
        ChooseGameLogic.instance.ChooseImageBaseColor = imageBaseColor;

        ButtonSelection.currentSelection = charImage;
        ButtonSelection.instance.SelectButton();
    }
}
