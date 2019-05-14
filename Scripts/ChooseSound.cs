using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSound : MonoBehaviour
{
    public bool startSelected;
    public Image borderImage;
    public Image mainImage;
    public Sprite soundSprite;
    public string soundIdentifier;
    //public Flowchart flowchart;

    public void OnEnable()
    {
        if (startSelected)
        {
            ButtonSelectionWithBorder.instance.rightBtn.interactable = false;
        }
    }

    public void SelectSound()
    {
        ButtonSelectionWithBorder.instance.rightBtn.interactable = true;
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlaySound(soundIdentifier);

        ChooseGameLogic.instance.ChooseSoundSprite = soundSprite;

        ButtonSelectionWithBorder.instance.currentSelection = new KeyValuePair<Image, Image>(borderImage, mainImage);
        ButtonSelectionWithBorder.instance.SelectButton();
    }
}