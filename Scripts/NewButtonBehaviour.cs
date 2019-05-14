using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class NewButtonBehaviour : MonoBehaviour
{
    public enum ButtonType { RightButton, WrongButton};

    public ButtonType buttonType;

    private Sprite normalSprite;
    private Sprite highlightedSprite;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void InitializeButton(Sprite normalSprite, Sprite highlightedSprite)
    {
        this.normalSprite = normalSprite;
        this.highlightedSprite = highlightedSprite;
    }

    public void ActivateButton()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }

        button.interactable = true;
    }

    public void DesactivateButton()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.interactable = false;
    }

    public void UseNormalSprite()
    {
        button.image.sprite = normalSprite;
    }

    public void UseHighlightedlSprite()
    {
        button.image.sprite = highlightedSprite;
    }

    public void SetFungusParameter(Flowchart flowchart)
    {
        switch(buttonType)
        {
            case ButtonType.RightButton:

                if(flowchart.HasExecutingBlocks())
                {
                    flowchart.SetGameObjectVariable("rightButton", this.gameObject);
                }
                else
                {
                    flowchart.SetGameObjectVariable("rightButton", this.gameObject);
                    flowchart.ExecuteBlock("Right Button"); 
                }

                break;

            case ButtonType.WrongButton:
                flowchart.SetGameObjectVariable("wrongButton", this.gameObject);
                flowchart.ExecuteBlock("Wrong Button");
                break;
        }
    }
}
