using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fungus;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    //public ButtonSpriteChanger musicMuteButton;
    //public ButtonSpriteChanger bfxMuteButton;

    public Sound[] soundList;

    public Sound[] failSoundList;

    public Sound[] successSoundList;

    public Sound[] noteScaleList;

    private string lastAudioDescription;

    private int currNoteIndex;

    CommonAudios commonAudios;

    private void Awake()
    {//Singleton statement.
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/

        AudioManager.instance = this;

        

        AudioManagerSetup();
    }

    private void Start()
    {
        VerifySoundPrefs(Sound.SoundType.Music, true);
        VerifySoundPrefs(Sound.SoundType.SFX, true);
    }

    public void SetLastAudioDescription(string newAudioName)
    {
        lastAudioDescription = newAudioName;
    }

    void AudioManagerSetup()
    {
        GameObject commonAudioGO = GameObject.FindGameObjectWithTag("Common Audios");
        
        

        if (commonAudioGO != null)
        {
            commonAudios = commonAudioGO.GetComponent<CommonAudios>();

            if (failSoundList.Length == 0)
            {
                failSoundList = commonAudios.failSoundList;
            }

            if(successSoundList.Length == 0)
            {
                successSoundList = commonAudios.successSoundList;
            }
        }


        foreach (Sound sound in instance.soundList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
        }

        foreach (Sound sound in instance.failSoundList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
        }

        foreach (Sound sound in instance.noteScaleList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
            sound.source.priority = 0;
        }

        if (instance.successSoundList.Length > 0)
        {
            foreach (Sound sound in instance.successSoundList)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.audioClip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = false;
            }
        }

    }

    public AudioSource GetAudioSource(string _soundName)
    {
        Sound sound;

        sound = Array.Find(instance.soundList, _sound => _sound.name == _soundName);

        if (sound == null)
        {
            Debug.LogWarning(_soundName + " não foi encontrado na lista de sons!");
            return null;
        }

        return sound.source;
    }

    public AudioSource GetFailAudioSource(string _soundName)
    {
        Sound sound;

        sound = Array.Find(instance.failSoundList, _sound => _sound.name == _soundName);

        if (sound == null)
        {
            Debug.LogWarning(_soundName + " não foi encontrado na lista de sons!");
            return null;
        }

        return sound.source;
    }

    public AudioSource GetSuccessAudioSource(string _soundName)
    {
        Sound sound;

        sound = Array.Find(instance.successSoundList, _sound => _sound.name == _soundName);

        if (sound == null)
        {
            Debug.LogWarning(_soundName + " não foi encontrado na lista de sons!");
            return null;
        }

        return sound.source;
    }

    public AudioSource GetScaleAudioSource(string _soundName)
    {
        Sound sound;

        sound = Array.Find(instance.noteScaleList, _sound => _sound.name == _soundName);

        if (sound == null)
        {
            Debug.LogWarning(_soundName + " não foi encontrado na lista de sons!");
            return null;
        }

        return sound.source;
    }

    public float PlaySound(string _soundName)
    {
        try
        {
            AudioSource audioSource = GetAudioSource(_soundName);
            audioSource.Play();
            return audioSource.clip.length;
        }
        catch (System.Exception)
        {
            Debug.LogError(_soundName + " não pode ser tocado pois não esta contido na lista de sons!");
            return 0;
        }
    }

    public void PlaySoundEvent(string _soundName)
    {
        PlaySound(_soundName);
    }

    public void PlayRandomFailSoundEvent()
    {
        PlayRandomFailSound();
    }

    public void PlayRandomSucessSoundEvent()
    {
        PlayRandomSuccessSound();
    }

    public float PlayRandomFailSound()
    {
        try
        {
            int aux = UnityEngine.Random.Range(0, failSoundList.Length);
            AudioSource audioSource = GetFailAudioSource(failSoundList[aux].name);
            audioSource.Play();
            return audioSource.clip.length;
        }
        catch (System.Exception)
        {
            Debug.LogError("Não existe som de erro!");
            return 0;
        }
    }

    public float PlayScaleSound()
    {
        try
        {
            AudioSource audioSource = GetScaleAudioSource(noteScaleList[currNoteIndex].name);
            audioSource.Play();
            currNoteIndex++;

            if (currNoteIndex >= noteScaleList.Length)
            {
                currNoteIndex = 0;
            }

            return audioSource.clip.length;
        }
        catch (System.Exception)
        {
            Debug.LogError("Não existe som!");
            return 0;
        }
    }

    public float PlayRandomSuccessSound()
    {
        try
        {
            int aux = UnityEngine.Random.Range(0, successSoundList.Length);
            AudioSource audioSource = GetSuccessAudioSource(successSoundList[aux].name);
            audioSource.Play();
            return audioSource.clip.length;
        }
        catch (System.Exception)
        {
            Debug.LogError("Não existe som de erro!");
            return 0;
        }
    }

    public void ResetNoteIndex()
    {
        currNoteIndex = 0;
    }

    public void PlaySound(Sound sound)
    {
        if (sound.source == null)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        sound.source.Play();
    }

    public void SetAudioDescription(string soundName)
    {
        lastAudioDescription = soundName;
        Debug.Log(lastAudioDescription);
    }

    public void PlayAudioDescription(string soundName)
    {
        SetAudioDescription(soundName);

        try
        {
            AudioSource audioSource = GetAudioSource(soundName);
            audioSource.Play();
        }
        catch (System.Exception)
        {
            Debug.LogError(soundName + " não pode ser tocado pois não esta contido na lista de sons!");
        }
    }

    public float PlayAudioDescriptionWaitTime(string soundName)
    {
        SetAudioDescription(soundName);

        try
        {
            AudioSource audioSource = GetAudioSource(soundName);
            audioSource.Play();
            return audioSource.clip.length;
        }
        catch (System.Exception)
        {
            Debug.LogError(soundName + " não pode ser tocado pois não esta contido na lista de sons!");
            return 0;
        }
    }

    public float PlayLastAudioDescription()
    {
        return PlaySound(lastAudioDescription);
    }

    public void MuteSoundByType(Sound.SoundType type)//True for Music and False for BFX
    {
        bool isMuted = false;

        foreach (Sound s in instance.soundList)
        {
            if (s.soundType == type)
            {
                s.source.mute = !s.source.mute;
                isMuted = s.source.mute;
            }
        }

        //Salva em PlayerPrefs qual tipo de som foi mutado.
        if (type == Sound.SoundType.Music)
        {
            PlayerPrefs.SetInt("MusicMuted", Convert.ToInt32(isMuted));
        }

        if (type == Sound.SoundType.SFX)
        {
            PlayerPrefs.SetInt("SfxMuted", Convert.ToInt32(isMuted));
        }
    }

    public void VerifySoundPrefs(Sound.SoundType type, bool isInitializing)
    {
        bool isMuted = false;

        if (type == Sound.SoundType.Music)
        {
            isMuted = Convert.ToBoolean(PlayerPrefs.GetInt("MusicMuted"));
        }

        if (type == Sound.SoundType.SFX)
        {
            isMuted = Convert.ToBoolean(PlayerPrefs.GetInt("SfxMuted"));
        }

        if (isInitializing)
        {
            if (isMuted)
            {
                MuteSoundByType(type);
            }
        }
    }


    public void StopAllSounds()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }
}

[CommandInfo("Audio", "ResetNoteIndex", "Bla bla bla")]
public class ResetNoteIndex : Command
{
    public override void OnEnter()
    {
        AudioManager.instance.ResetNoteIndex();
        Continue();
    }
}