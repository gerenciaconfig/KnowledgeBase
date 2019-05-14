using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class AudioRecorder : MonoBehaviour
{
   
    public AudioClip recording;
    public enum State
    {
        Idle,
        Recording,
        ReadyToPlay
    }
    public State state;
    AudioSource aud;

    public GameObject recordingButton;
    public GameObject readyToPlayButton;
    public string doneRecBlockName;
    public string readyToPlayBlockname;

    [SerializeField]
    private Flowchart flowChart;

    [SerializeField]
    private AudioSource MusicToControl;

    [SerializeField]
    private string RightSound;
    
    

    void Start()
    {
        aud = GetComponent<AudioSource>();
        recordingButton.SetActive(false);
        readyToPlayButton.SetActive(false);
        state = State.Idle;
        StartCoroutine(FindMics());
    }

    void OnEnable()
    {
        recordingButton.SetActive(false);
        readyToPlayButton.SetActive(false);
        state = State.Idle;
        StartCoroutine(FindMics());
    }

    public void ButtonPress()
    {
        switch(state)
        {
            case State.Idle:
                MusicToControl.Pause();
                recordingButton.SetActive(true);
                readyToPlayButton.SetActive(false);
                StartCoroutine(BeginRecording());
            break;

            case State.Recording:
                MusicToControl.UnPause();
                recordingButton.SetActive(false);
                readyToPlayButton.SetActive(true);
                StopRecording();
            break;

            case State.ReadyToPlay:
                GetComponent<Button>().interactable = false;
                StopCoroutine(PlayRecordedAudio(0f));
                StartCoroutine(PlayRecordedAudio(PlayRecordedAudioWaitTime()));
            break;
        }  
    }

    /*public void StopRecording()
    {
        StopCoroutine("BeginRecording");
        Microphone.End("");
        aud.clip = recording;
        state = State.ReadyToPlay;
    }*/

    public void StopRecording()
    {
        //Capture the current clip data
        StopCoroutine("BeginRecording");
        if (aud.clip != null)
        {
            AudioClip recordedClip = aud.clip;
            var position = Microphone.GetPosition("");
            var soundData = new float[recordedClip.samples * recordedClip.channels];
            recordedClip.GetData(soundData, 0);

            //Create shortened array for the data that was used for recording
            var newData = new float[position * recordedClip.channels];


            //Copy the used samples to a new array
            for (int i = 0; i < newData.Length; i++)
            {
                newData[i] = soundData[i];
            }
            Microphone.End("");
            //One does not simply shorten an AudioClip,
            //    so we make a new one with the appropriate length
            AudioClip newClip = AudioClip.Create("NewClip", position, recordedClip.channels, recordedClip.frequency, false);
            newClip.SetData(newData, 0);        //Give it the data from the old clip

            //Replace the old clip
            //AudioClip.Destroy(recording);
            aud.clip = newClip;
            flowChart.ExecuteBlock(readyToPlayBlockname);
            state = State.ReadyToPlay;
        } else
        {
            flowChart.ExecuteBlock(readyToPlayBlockname);
            state = State.ReadyToPlay;
        }
    }

    public void ChangeState(State _state)
    {
        state = _state;
    }

    private float PlayRecordedAudioWaitTime()
    {
        if (aud.clip != null)
        {
            aud.Play();
            return aud.clip.length;
        } else
        {
            return 0.1f;
        }    
    }

    IEnumerator BeginRecording()
    {
        AudioManager.instance.PlaySound(RightSound);
        yield return new WaitForSeconds(1.1f);
        state = State.Recording;
        foreach (string device in Microphone.devices)
        {
            Debug.Log(device);
        }
        if (Microphone.devices.Length > 0)
        {
            recording = Microphone.Start("", false, 190, 44100);
            aud.clip = recording;
            yield return new WaitForSeconds(180);
            state = State.ReadyToPlay;
            recordingButton.SetActive(false);
            readyToPlayButton.SetActive(true);
            StopRecording();
            //aud.Play();
        }
    }

    IEnumerator PlayRecordedAudio(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        flowChart.ExecuteBlock(doneRecBlockName);
    }

    IEnumerator FindMics()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }
    }
}
