using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Game", "Game Ended", "")]
public class FungusGameEnded : Command
{
    public Game game;
    public bool succeeded;

    public override void OnEnter()
    {
        game.AddVictory(succeeded);
        Continue();
    }

}
