namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MusicVolumeButton : MonoBehaviour
    {
        MusicVolumeChecker musicVo;


        private void Awake()
        {
            musicVo = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicVolumeChecker>();
        }

        public void ButtonClicked()
        {
            if (PlayerPrefs.GetInt("MusicOff") == 0)
            {
                PlayerPrefs.SetInt("MusicOff", 1);
            }
            else
            {
                PlayerPrefs.SetInt("MusicOff", 0);
            }

            musicVo.CheckVolume();
        }
    }
}