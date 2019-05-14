using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class Card : MovingProperties
{
    MemoryManager memoryManager;

    private int cardId;

    private Vector3 originalPos;

    private string cardSound;

    private Animator animator;

    private SpriteRenderer[] spriteRenderers;

    private bool disabled;

    public float transitionTime;

	void Awake()
	{
        SavePos();

        memoryManager = GameObject.FindGameObjectWithTag("Game").GetComponent<MemoryManager>();
        animator = GetComponent<Animator>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

	public int GetCardId()
	{
		return cardId;
	}

	public void SetCardId(int cardId)
	{
		this.cardId = cardId;
	}

    //Reseta as variáveis da carta
    public void Reset()
    {
        disabled = false;
        animator.SetTrigger("Reset");
    }

    //Saves its original position for returning to it later in the game
    public void SavePos()
    {
        originalPos = transform.position;
    }

   

    //Calls the corroutine that moves it to an specific position
    public void GoToPos(Vector3 pos, Quaternion quaternion)
    {
        StartCoroutine(MoveAndRotateTo(transitionTime, pos, quaternion));
    }

    //Calls the corroutine that moves it to the original position
    public void ReturnToOriginalPos()
    {
        StartCoroutine(MoveAndRotateTo(transitionTime, originalPos, Quaternion.identity));
    }

	public void SelectCard()
	{
		memoryManager.SelectCard(this);
	}

    

    public IEnumerator FlipAndMoveTo(Vector3 pos, Quaternion quaternion)
    {
        if(disabled)
        {
            Flip();
            yield return new WaitForSeconds(0.3f);
        }
        StartCoroutine(MoveAndRotateTo(transitionTime, pos, quaternion));
    }

    public void SetCardSound(string soundName)
    {
        cardSound = soundName;
    }

    public void PlayCardSound()
    {
        AudioManager.instance.PlaySound(cardSound);
    }

    public void OnMouseDown()
    {
        if(!disabled && memoryManager.SelectCard(this))
        {
            Flip();
        }
    }

    public void Flip()
    {
        animator.SetTrigger("Flip");
    }

    public void SetBackSprite(Sprite newSprite)
    {
        spriteRenderers[0].sprite = newSprite;
    }

    public void SetFrontSprite(Sprite newSprite)
    {
        spriteRenderers[1].sprite = newSprite;
    }

    public void DisableCard()
    {
        disabled = true;
    }

    public void EnableCard()
    {
        disabled = false;
    }

    public bool GetDisabled()
    {
        return disabled;
    }

    public void PlayCardFlipSound()
    {
        AudioManager.instance.PlaySound("Card Flip");
    }
}