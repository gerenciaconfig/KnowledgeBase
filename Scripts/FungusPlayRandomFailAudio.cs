using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Play Random Fail Sound Arco", "Plays a ramdom fail sound from the Audio Manager sound bank")]
public class FungusPlayRandomFailAudio : Command
{
    public bool waitUntilFinished;

    public override void OnEnter()
    {
        if (waitUntilFinished)
        {
            StartCoroutine(WaitAudio(AudioManager.instance.PlayRandomFailSound()));
        }
        else
        {
            AudioManager.instance.PlayRandomFailSound();
            Continue();
        }
    }

    IEnumerator WaitAudio(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Continue();
    }
}
