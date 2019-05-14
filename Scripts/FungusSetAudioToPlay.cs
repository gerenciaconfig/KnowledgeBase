using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Audio", "Set Audio To Play", "Sets the audio to play")]

public class FungusSetAudioToPlay : Command 
{

	public Flowchart flow;
	public string blockName;
	public string audioN;

	public override void OnEnter()
    {
		flow.FindBlock(blockName).GetComponent<FungusPlayImageDescription>().audioName = audioN;
		//audioBtn.GetComponent<FungusPlayImageDescription>().audioName = audioN;
		//Debug.Log("aqui");
		Continue();
	}

}