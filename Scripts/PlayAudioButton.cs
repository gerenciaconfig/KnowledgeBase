using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioButton : MonoBehaviour 
{

	public AudioManager audio;

	public string soundName;

	public void PlayButtonAudio()
	{
		audio.PlaySound(soundName);
	}
}