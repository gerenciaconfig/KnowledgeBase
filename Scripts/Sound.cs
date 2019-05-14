using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // nome que será dado ao áudio.

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume = 1; // volume do AudioClip.
    [Range(0.1f, 3f)]
    public float pitch = 1; // pitch do AudioClip.

    public bool loop;

    public enum SoundType { Music, SFX, ImageDescription }
    public SoundType soundType;

    [HideInInspector]
    public AudioSource source; // AudioSource referente a cada AudioClip.

}
