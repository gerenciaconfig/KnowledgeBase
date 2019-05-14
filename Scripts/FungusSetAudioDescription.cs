using System.Collections;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Set Image Description", "Sets the image description")]
public class FungusSetAudioDescription : Command
{
	public string audioName;

	public override void OnEnter()
	{
		AudioManager.instance.SetAudioDescription(audioName);
		Continue();
	}
}
