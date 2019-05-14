using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Mkey
{
    public class SoundMasterController : MonoBehaviour
    {

        [Space(8, order = 0)]
        [Header("Default Sound Settings", order = 1)]
        private bool soundOn = true;
        private bool musicOn = true;
        private float volume = 1.0f;
        public static SoundMasterController Instance;
        [Space(8, order = 0)]
        [Header("Audio Sources", order = 1)]
        public AudioSource aSclick;
        public AudioSource aSbkg;
        public AudioSource aSloop;

        [Space(8, order = 0)]
        [Header("AudioClips", order = 1)]
        public AudioClip menuClick;
        public AudioClip menuPopup;
        public AudioClip menuCheck;
        public AudioClip screenChange;
        public AudioClip bkgMusic;
        public AudioClip shoot;

        WaitForEndOfFrame wff;
        WaitForSeconds wfs0_1;

        public BubblesPlayer player
        {
            get { return BubblesPlayer.Instance; }
        }

        private string saveNameSound = "mk_soundon";
        private string saveNameMusic = "mk_musicon";
        private string saveNameVolume = "mk_volume";

        public bool SoundOn
        {
            get
            {
                return soundOn;
            }
            set
            {
                soundOn = value;
                if (player.SaveData) PlayerPrefs.SetInt(saveNameSound, (soundOn) ? 1 : 0);
            }
        }

        public bool MusicOn
        {
            get
            {
                return musicOn;
            }
            set
            {
                bool upd = (musicOn != value);
                musicOn = value;
                if (player.SaveData) PlayerPrefs.SetInt(saveNameMusic, (musicOn) ? 1 : 0);
                if (upd) UpdateLevelBkgMusik();
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = Mathf.Clamp(value, 0, 1);
                if (player.SaveData) PlayerPrefs.SetFloat(saveNameVolume, volume);
                ApplyVolume();
            }
        }

        public List<AudioSource> tempAudioSources;
        private int audioSoucesMaxCount = 5;

        void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            wff = new WaitForEndOfFrame();
            wfs0_1 = new WaitForSeconds(0.1f);
        }

        void Start()
        {
            if (player.SaveData)
            {
                if (!PlayerPrefs.HasKey(saveNameSound))
                {
                    PlayerPrefs.SetInt(saveNameSound, (soundOn) ? 1 : 0);
                }
                soundOn = (PlayerPrefs.GetInt(saveNameSound) > 0) ? true : false;

                if (!PlayerPrefs.HasKey(saveNameMusic))
                {
                    PlayerPrefs.SetInt(saveNameMusic, (musicOn) ? 1 : 0);
                }
                musicOn = (PlayerPrefs.GetInt(saveNameMusic) > 0) ? true : false;

                if (!PlayerPrefs.HasKey(saveNameVolume))
                {
                    PlayerPrefs.SetFloat(saveNameVolume, 1.0f);
                }
                volume = PlayerPrefs.GetFloat(saveNameVolume);
            }

            tempAudioSources = new List<AudioSource>();
            PlayBkgMusik(musicOn);
            ApplyVolume();
        }

        #region play sounds

        public void SoundPlayClipAtPos(float playDelay, AudioClip aC, Action callBack, Vector3 pos, float volumeK)
        {
            StartCoroutine(PlayClipAtPoint(playDelay, aC, callBack, pos, volumeK));
        }

        public void SoundPlayClick(float playDelay, Action callBack)
        {
            StartCoroutine(PlayClip(playDelay, menuClick, callBack));
        }

        public void SoundPlayPopUp(float playDelay, Action callBack)
        {
            StartCoroutine(PlayClip(playDelay, menuPopup, callBack));
        }

        public void SoundPlayCheck(float playDelay, Action callBack)
        {
            StartCoroutine(PlayClip(playDelay, menuCheck, callBack));
        }

        public void SoundPlayScreenChange(float playDelay, Action callBack)
        {
            StartCoroutine(PlayClip(playDelay, screenChange, callBack));
        }

        public void SoundPlayShoot(float playDelay, Action callBack)
        {
            StartCoroutine(PlayClip(playDelay, shoot, callBack));
        }



        IEnumerator PlayClip(float playDelay, AudioClip aC, Action callBack)
        {
            if (soundOn)
            {
                if (!aSclick) aSclick = GetComponent<AudioSource>();
                float delay = 0f;
                while (delay < playDelay)
                {
                    delay += Time.deltaTime;
                    yield return wff;
                }

                if (aSclick && aC)
                {
                    aSclick.clip = aC;
                    aSclick.Play();
                }

                while (aSclick.isPlaying)
                    yield return wff;
                if (callBack != null)
                {
                    callBack();
                }
            }
        }

        IEnumerator PlayLoopClip(float playDelay, bool loop, AudioClip aC, Action callBack)
        {
            if (soundOn)
            {
                if (!aSloop) aSloop = GetComponent<AudioSource>();
                float delay = 0f;
                while (delay < playDelay)
                {
                    delay += Time.deltaTime;
                    yield return wff;
                }

                if (aSloop && aC)
                {
                    aSloop.clip = aC;
                    aSloop.loop = loop;
                    aSloop.Play();
                }
                while (aSloop.isPlaying)
                    yield return wff;
                if (callBack != null)
                {
                    callBack();
                }
            }
        }

        public void StopLoopClip()
        {
            if (aSloop)
            {
                aSloop.Stop();
            }
        }

        public void PlayBkgMusik(bool play)
        {
            if (play && aSbkg && !aSbkg.isPlaying)
            {
                aSbkg.volume = 0;
                aSbkg.Play();
                SimpleTween.Value(gameObject, 0.0f, 0.45f, 3.5f).SetOnUpdate((float val) => { aSbkg.volume = val; }).
                    AddCompleteCallBack(() => { });
            }

            else if (!play && aSbkg)
            {
                SimpleTween.Value(gameObject, 0.45f, 0.0f, 2f).SetOnUpdate((float val) => { aSbkg.volume = val; }).
                      AddCompleteCallBack(() => { aSbkg.Stop(); });

            }
        }

        IEnumerator PlayClipAtPoint(float playDelay, AudioClip aC, Action callBack, Vector3 pos, float volumeK)
        {
            if (soundOn && tempAudioSources.Count < audioSoucesMaxCount)
            {
                AudioSource aSt = CreateASAtPos(pos);
                tempAudioSources.Add(aSt);
                aSt.volume = Volume * volumeK;

                float delay = 0f;
                while (delay < playDelay)
                {
                    delay += Time.deltaTime;
                    yield return wff;
                }

                if (aC)
                {
                    aSt.clip = aC;
                    aSt.Play();
                }

                while (aSt && aSt.isPlaying)
                    yield return wff;

                tempAudioSources.Remove(aSt);
                if (aSt) Destroy(aSt.gameObject);
                if (callBack != null)
                {
                    callBack();
                }
            }
        }

        private AudioSource CreateASAtPos(Vector3 pos)
        {
            GameObject aS = new GameObject();
            aS.transform.position = pos;
            return aS.AddComponent<AudioSource>();
        }

        public void UpdateLevelBkgMusik()
        {
            bool play = musicOn;

            AudioClip nClip = bkgMusic;
            if (nClip != aSbkg.clip)
            {
                aSbkg.Stop();
                aSbkg.Play();
                aSbkg.clip = nClip;
            }

            SimpleTween.Cancel(gameObject, true);

            if (play && aSbkg && !aSbkg.isPlaying)
            {
                aSbkg.volume = 0;
                aSbkg.Play();
                SimpleTween.Value(gameObject, 0.0f, volume, 3.5f).SetOnUpdate((float val) => { aSbkg.volume = val; }).
                       AddCompleteCallBack(() => { });
            }

            else if (!play && aSbkg && aSbkg.isPlaying)
            {
                SimpleTween.Value(gameObject, volume, 0.0f, 2f).SetOnUpdate((float val) => { aSbkg.volume = val; }).
                      AddCompleteCallBack(() => { aSbkg.Stop(); });
            }
        }

        #endregion play sounds

        private void ApplyVolume()
        {
            if (aSclick)
            {
                aSclick.volume = Volume;
            }

            if (aSbkg)
            {
                aSbkg.volume = Volume;
            }

            if (aSloop)
            {
                aSloop.volume = Volume;
            }

            if (tempAudioSources.Count > 0)
            {
                tempAudioSources.ForEach((ast) => { ast.volume = Volume; });
            }
        }

        private void OnDestroy()
        {
            SimpleTween.Cancel(gameObject, false);
        }
    }
}