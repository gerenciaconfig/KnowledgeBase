using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// The block will execute when the user clicks on the target UI button object.
/// </summary>
[EventHandlerInfo("Game",
                    "Unflip Card 2",
                    "The block will execute when the user unflips target card")]

public class UnflipCardEvent2 : EventHandler
 {
	public static GameObject unflippedCard;

    public static UnflipCardEvent2 instance;

    private void Start()
    {
        instance = this;
    }

    public static void Unflip(GameObject card)
    {
		GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        gp.SetParameter2(card);

        instance.ExecuteBlock();
    }
}