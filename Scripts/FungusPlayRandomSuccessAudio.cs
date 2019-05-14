using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Play Random Success Sound Arco", "Plays a ramdom success sound from the Audio Manager sound bank")]
public class FungusPlayRandomSuccessAudio : Command
{
 public bool waitUntilFinished;

    public override void OnEnter()
    {
        if (waitUntilFinished)
        {
            StartCoroutine(WaitAudio(AudioManager.instance.PlayRandomSuccessSound()));
        }
        else
        {
            AudioManager.instance.PlayRandomSuccessSound();
            Continue();
        }
    }

    IEnumerator WaitAudio(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Continue();
    }
}