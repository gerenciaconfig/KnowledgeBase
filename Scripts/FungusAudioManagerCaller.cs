 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo ("Audio", "Play Sound Arco", "Plays Sounds using Arco's Audio Manager")]
public class FungusAudioManagerCaller : Command
{
    public string audioName;

    public bool waitUntilFinished;

    public override void OnEnter()
    {
        if(waitUntilFinished)
        {
            StartCoroutine(WaitAudio(AudioManager.instance.PlaySound(audioName)));
        }
        else
        {
            AudioManager.instance.PlaySound(audioName);
            Continue();
        }
    }

    IEnumerator WaitAudio(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Continue();
    }
}
