using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class PotionBehaviour : MonoBehaviour
{
    Animator animator;

    public Image glow;

    public int totalSelections;

    private int remaningSelections;

    private Button button;

    public Color nativeColor;

    public Color selectedColor;

	// Use this for initialization
	void Awake ()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        DesactivatePotion();
    }

    public void DisableGlow()
    {
        animator.SetTrigger("Reset");
    }

    public void GlowPotion()
    {
        remaningSelections--;

        button.interactable = false;

        glow.color = selectedColor;

        animator.SetTrigger("Glow");
    }

    public void ShowGlow()
    {
        glow.color = nativeColor;
        animator.SetTrigger("Show");
    }

    public void ActivatePotion()
    {
        button.interactable = true;
    }

    public void DesactivatePotion()
    {
        button.interactable = false;
    }

    public void ResetPotion()
    {
        DisableGlow();

        glow.color = nativeColor;

        remaningSelections = totalSelections;
    }
}
