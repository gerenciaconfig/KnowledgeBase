using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Mute Audio", "Mute all image descriptions audio")]
public class FungusMuteAudio : Command
{   
    public AudioManager am;

    public override void OnEnter()
    {   
        foreach (Sound soundobj in am.soundList)
        {
            if(soundobj.soundType == Sound.SoundType.ImageDescription)
            {
                if(am.GetAudioSource(soundobj.name.ToString()).mute == false)
                {
                    am.GetAudioSource(soundobj.name.ToString()).mute = true;
                }
                else
                {
                    am.GetAudioSource(soundobj.name.ToString()).mute = false;
                }
            }
        }
        
        Continue(); 
    }
}
