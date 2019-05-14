using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Game", "Play Card Sound", "")]

public class FungusPlayCardSound : Command
{
    public Card card;

    public override void OnEnter()
    {
        card.PlayCardSound();
        Continue();
    }
}
