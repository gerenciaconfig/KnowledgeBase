// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// The block will execute when the user clicks on the target UI button object.
/// </summary>
[EventHandlerInfo("Game",
                    "Card Flipped",
                    "The block will execute when the user flips target card")]
[AddComponentMenu("")]
public class CardFlippedEvent : EventHandler
{
    public static CardFlippedEvent instance;

    private void Start()
    {
        instance = this;
    }

    public static void Flip(GameObject card)
    {
        GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        gp.SetParameter(card);
        instance.ExecuteBlock();
    }
}
