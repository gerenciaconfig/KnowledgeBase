using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Play Last Image Description", "Plays the last Desctiption that was played in Arco's Audio Manager")]
public class FungusPlayLastImageDescription : Command
{
    public override void OnEnter()
    {
        AudioManager.instance.PlayLastAudioDescription();
        //StartCoroutine(PlayButtonHint(AudioManager.instance.PlayLastAudioDescription()));
        Continue();
    }

    private IEnumerator PlayButtonHint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Animator anim = GameObject.FindWithTag("PlayButton").gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("shine");
        }



    }
}
