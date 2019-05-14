using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {
    public static VideoController instance;
    public VideoPlayer player;
    public AudioSource audioSource;
    [Space(10)]
    public GameObject screenPanel;
    public Slider progressBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }  
    }

    private void OnEnable()
    {
        player.prepareCompleted += OpenVideo;
        player.loopPointReached += CloseVideoWhenFinished;       
    }

    private void OnDisable()
    {
        player.prepareCompleted -= OpenVideo;
        player.loopPointReached -= CloseVideoWhenFinished;
    }

    private void Update()
    {
        VideoProgressBar();
    }

    public void SelectVideo(VideoClip video)
    {
        player.clip = video;
        
        player.audioOutputMode = VideoAudioOutputMode.AudioSource;
        player.SetTargetAudioSource(0, audioSource);

        player.Prepare();
    }

    public void OpenVideo(VideoPlayer source)
    {
        screenPanel.SetActive(true);
        player.Play();
    }

    public void CloseVideo()
    {
        player.Stop();
        player.clip = null;
        screenPanel.SetActive(false);
    }

    public void CloseVideoWhenFinished(VideoPlayer source)
    {
        CloseVideo();
    }

    public void VideoProgressBar()
    {
        if (progressBar == null)
        {
            return;
        }
        progressBar.minValue = 0;

        if ((player.frameCount / player.frameRate) >= 0)
        {
            progressBar.maxValue = (player.frameCount / player.frameRate);
        }

        progressBar.value = (float) player.time;
    }

    public void Seek()
    {
        float nTime = progressBar.value;

        if (!player.isPrepared || !player.canSetTime)
        {
            return;
        }

        nTime = Mathf.Clamp(nTime, 0 , 1);
        player.time = nTime * (player.frameCount / player.frameRate);
    }


    public void StepFoward(float stepPercent)
    {
        player.time = player.time + ( stepPercent * (player.frameCount / player.frameRate));
    }

    public void StepBackward(float stepPercent)
    {
        player.time = player.time - (stepPercent * (player.frameCount / player.frameRate));
    }

    public void PauseVideo()
    {
        player.Pause();
    }
}
