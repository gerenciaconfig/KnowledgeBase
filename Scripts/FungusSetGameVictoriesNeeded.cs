using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Game", "Set Victories to win", "Sets to number of victories needed to go to next game")]
public class FungusSetGameVictoriesNeeded : Command 
{
	public Game game;
	public int victoriesToWin;
	public override void OnEnter()
    {
        game.SetVictoryToWin(victoriesToWin);
        Continue();
    }
}