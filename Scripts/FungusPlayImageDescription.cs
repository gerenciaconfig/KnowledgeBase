using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Play Image Description", "Plays the image description")]
public class FungusPlayImageDescription : Command
{

    public string audioName;

    public bool waitUntilFinished;

    public override void OnEnter()
    {
        if(waitUntilFinished)
        {
            StartCoroutine(WaitAudio(AudioManager.instance.PlayAudioDescriptionWaitTime(audioName)));
        }
        else
        {
            AudioManager.instance.PlayAudioDescription(audioName);
            Continue();
        }
        
    }

    IEnumerator WaitAudio(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Continue();
    }
}
