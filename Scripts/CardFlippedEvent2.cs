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
                    "Card 2 Flipped",
                    "The block will execute when the user flips the second card")]
[AddComponentMenu("")]
public class CardFlippedEvent2 : EventHandler
{
    public static CardFlippedEvent2 instance;

    private void Start()
    {
        instance = this;
    }

    public static void Flip(GameObject card)
    {
        GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        gp.SetParameter2(card);
        instance.ExecuteBlock();
    }
}
