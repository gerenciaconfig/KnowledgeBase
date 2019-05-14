using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(AudioManager))]
public class AddAudiosToManager : MonoBehaviour
{
    public List<AudioClip> audiosToAdd;
    public string charactersToRemove = "";
    //public bool clearManagerBeforeAdd = false;

    [Button]
    public void AddToManager()
    {
        AudioManager aManager = GetComponent<AudioManager>();

        aManager.soundList = new Sound[audiosToAdd.Count];

        for (int i = 0; i < audiosToAdd.Count; i++)
        {
            Sound sd = new Sound();
            sd.audioClip = audiosToAdd[i];
            sd.soundType = Sound.SoundType.ImageDescription;

            sd.name = audiosToAdd[i].name.Remove(0, charactersToRemove.ToCharArray().Length);

            aManager.soundList[i] = sd;
        }
    }

}
