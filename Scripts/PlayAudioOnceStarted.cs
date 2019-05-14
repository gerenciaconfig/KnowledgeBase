using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnceStarted : MonoBehaviour
{
    private bool playedAudio;

    public string audioName;

	// Use this for initialization
	private void OnEnable ()
    {
		if(!playedAudio)
        {
            playedAudio = true;
            StartCoroutine(PlayButtonHint(AudioManager.instance.PlayAudioDescriptionWaitTime(audioName)));
            
        }
        else
        {
           AudioManager.instance.SetLastAudioDescription(audioName);
        }

        
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
