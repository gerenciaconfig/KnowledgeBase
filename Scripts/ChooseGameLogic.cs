using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameLogic : MonoBehaviour
{
    public static ChooseGameLogic instance;

    [HideInInspector]
    private Sprite chooseImage;
    [HideInInspector]
    private Color chooseImageBaseColor;
    [HideInInspector]
    private Sprite chooseSoundSprite;

    public Image cardImage;
    public Image soundCardImage;    
    public List<Text> finalTexts;

    public Sprite ChooseImage
    {
        get
        {
            return chooseImage;
        }
        set
        {
            chooseImage = value;
            cardImage.sprite = value;
        }
    }

    public Color ChooseImageBaseColor
    {
        get
        {
            return chooseImageBaseColor;
        }

        set
        {
            chooseImageBaseColor = value;

            foreach (var item in finalTexts)
            {
                item.color = value;
            }
        }
    }

    public Sprite ChooseSoundSprite
    {
        get
        {
            return chooseSoundSprite;
        }

        set
        {
            chooseSoundSprite = value;
            soundCardImage.sprite = value;
        }
    }

    private void OnEnable()
    {
        instance = this;
    }
}
