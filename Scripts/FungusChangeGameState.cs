using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Game", "Change Game State", "")]
public class FungusChangeGameState : SetGame
{
    [HideInInspector]
    public Block succededBlock;
    [HideInInspector]
    public Block failedBlock;

    [Tooltip("Flowchart which contains the block to execute. If none is specified then the current Flowchart is used.")]
    [SerializeField]
    [HideInInspector]
    protected Flowchart targetFlowchart;

    public override void OnEnter()
    {
        game.OnGameEnd.AddListener(OnGameEnd);
        game.StopGame.AddListener(StopSetGame);
    }

    protected IEnumerator CallBlock(Block block)
    {
        this.StopParentBlock();

        yield return new WaitForEndOfFrame();
        if (block != null)
        {
            block.StartExecution();
        }
    }

    protected void OnGameEnd()
    {
        game.OnGameEnd.RemoveListener(OnGameEnd);
        Flowchart flowchart = GetFlowchart();
        Block targetBlock;

        if (game.GetSucess())
        {
            targetBlock = succededBlock;
        }
        else
        {
            targetBlock = failedBlock;
        }

        if (flowchart.SelectedBlock == ParentBlock)
        {
            flowchart.SelectedBlock = targetBlock;
        }
        flowchart.StartCoroutine(CallBlock(targetBlock));
    }

    public override void GetConnectedBlocks(ref List<Block> connectedBlocks)
    {
        if (succededBlock != null)
        {
            connectedBlocks.Add(succededBlock);
        }
        if (failedBlock != null)
        {
            connectedBlocks.Add(failedBlock);
        }
    }


    public override string GetSummary()
    {
        return "Start Game";
    }

    public override Color GetButtonColor()
    {
        return new Color32(184, 210, 235, 255);
    }

    public void StopSetGame()
    {
        game.OnGameEnd.RemoveListener(OnGameEnd);
        game.StopGame.RemoveListener(StopSetGame);
        this.StopParentBlock();
    }
}
