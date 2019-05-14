using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo("Audio", "Stop All Sounds", "")]
public class FungusStopAllAudios : Command
{
    public override void OnEnter()
    {
        AudioManager.instance.StopAllSounds();
        Continue();
    }
}
