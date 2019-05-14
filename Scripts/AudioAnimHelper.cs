using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Arcolabs.Home
{
    public class AudioAnimHelper : MonoBehaviour
    {

        public Sound[] soundList;

        private void Start()
        {
            AudioManagerSetup();
        }        

        public void PlaySound(string _soundName)
        {
            try
            {
                GetAudioSource(_soundName).Play();
            }
            catch (System.Exception)
            {
                Debug.LogError(_soundName + " não pode ser tocado pois não esta contido na lista de sons!");
            }
        }

        public AudioSource GetAudioSource(string _soundName)
        {
            Sound sound;

            sound = Array.Find(soundList, _sound => _sound.soundName == _soundName);

            if (sound == null)
            {
                Debug.LogWarning(_soundName + " não foi encontrado na lista de sons!");
                return null;
            }

            return sound.source;
        }

        void AudioManagerSetup()
        {
            foreach (Sound sound in soundList)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.audioClip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

    }   
    

}

