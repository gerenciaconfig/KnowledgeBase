using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class MicSoundLimitGetter : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    private AudioSource _audio;

    public Game game;
    public UnityEvent OnBlowComplete;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (!Application.isEditor)
        {
            _audio.clip = Microphone.Start(null, true, 10, 44100);
            _audio.loop = true;
            _audio.mute = false;
            while (!(Microphone.GetPosition(null) > 0)) { }
            _audio.Play();
        }  
    }

    private void OnDisable()
    {
        if (!Application.isEditor)
        {
            Microphone.End(null);
        }
    }

    public float GetMaxAudioPeak(AudioClip clip)
    {
        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
        clip.GetData(waveData, micPosition);

        // Getting a peak on the last 128 samples
        float levelMax = 0;
        for (int i = 0; i < dec; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // use it like:
        return Mathf.Sqrt(levelMax);
    }

    void Update()
    {
        loudness = GetAveragedVolume() * sensitivity;
        if (loudness > 2.5f)
        {
            if (game!=null)
            {
                game.AddVictory(true);
            }
            OnBlowComplete.Invoke();
            this.gameObject.SetActive(false);
        }
    }
    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
}