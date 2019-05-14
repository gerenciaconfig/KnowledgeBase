using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// The block will execute when the user clicks on the target UI button object.
/// </summary>
[EventHandlerInfo("Game",
                    "Match Card 2",
                    "The block will execute when the user match target card")]

public class MatchCardEvent2 : EventHandler 
{
    public static MatchCardEvent2 instance;

    private void Start()
    {
        instance = this;
    }

    public static void Match(GameObject card)
    {
        GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        gp.SetParameter2(card);
        instance.ExecuteBlock();
    }
}