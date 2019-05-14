using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Game", "Stop All Games", "Stop All Games")]
[System.Serializable]
public class FungusStopAllGames : Command
{
    [SerializeField]
    public Game game;

    public override void OnEnter()
    {
        game.stopGame();

        Continue();
    }
}
