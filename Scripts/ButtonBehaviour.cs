using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite highlightedSprite;

    private Button btn;

    private Image image;

    public bool interacted;

    private void Start()
    {
        
        
    }

    void Awake()
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
        ResetButton();
    }

    public void SetNormalSprite()
    {
        if(normalSprite != null)
        {
            image.sprite = normalSprite;
        }
        
    }

    public void SetHighlightedSprite()
    {
        if(highlightedSprite != null)
        {
            image.sprite = highlightedSprite;
        }
       
    }

    public void InteractedBtn()
    {
        interacted = true;
        DeactivateButton();
    }

    public void Desinteract()
    {
        interacted = false;
        ActivateButton();
    }

    public void DeactivateButton()
    {
        btn.interactable = false;
    }

    public void ActivateButton()
    {
        if(!interacted)
        {
            btn.interactable = true;
        }
    } 

    public void ResetButton()
    {
        SetNormalSprite();
        interacted = false;
        btn.interactable = true;
    }    
}
