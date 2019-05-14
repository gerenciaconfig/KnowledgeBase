using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Audio", "Set Lerp Volume", "")]
public class FungusSetAudioVolumeArco : Command
{
    public enum Type { Lerp, Set };

    public Type type;

    [Range(0, 1)]
    public float targetVolume;

    public bool waitUntilFinished;

    public AudioSource audioSource;

    public float seconds;

    public override void OnEnter()
    {
        switch (type)
        {
            case Type.Lerp:
                StartCoroutine(ChangeVolume(seconds));
                break;

            case Type.Set:
                audioSource.volume = targetVolume;
                break;
        }

        if (!waitUntilFinished || type == Type.Set)
        {
            Continue();
        }
    }

    public IEnumerator ChangeVolume(float seconds)
    {
        float elapsedTime = 0;

        float originalVolume = audioSource.volume;

        while (elapsedTime < seconds)
        {
            audioSource.volume = Mathf.Lerp(originalVolume, targetVolume, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (waitUntilFinished)
        {
            Continue();
        }
    }
}