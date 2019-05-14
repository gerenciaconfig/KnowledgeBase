namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MusicVolumeChecker : MonoBehaviour
    {
        AudioSource audioSource;

        public bool playOnAwake = true;

        void Awake()
        {            
            audioSource = GetComponent<AudioSource>();

            if (playOnAwake)
            {
                CheckVolume();
            }
            //COMANDO PARA SETAR O VOLUME DA MÚSICA
            /*
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume *= PlayerPrefs.GetFloat("MusicVolume");*/
        }

        public void CheckVolume()
        {
            audioSource = GetComponent<AudioSource>();
            if (PlayerPrefs.GetInt("MusicOff") == 1)
            {
                StopAudio();
            }
            else
            {
                PlayAudioSource();
            }
        }

        public void PlayAudioSource()
        {            
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        public void StopAudio()
        {
            audioSource.Stop();
        }
    }
}
