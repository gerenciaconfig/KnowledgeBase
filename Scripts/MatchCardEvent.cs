using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// The block will execute when the user clicks on the target UI button object.
/// </summary>
[EventHandlerInfo("Game",
                    "Match Card",
                    "The block will execute when the user match target card")]

public class MatchCardEvent : EventHandler 
{
	public static GameObject matchCard;

    public static MatchCardEvent instance;

    private void Start()
    {
        instance = this;
    }

    public static void Match(GameObject card)
    {
        GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        gp.SetParameter(card);
        instance.ExecuteBlock();
    }
}