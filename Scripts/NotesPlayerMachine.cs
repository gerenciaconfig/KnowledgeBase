using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesPlayerMachine : MonoBehaviour
{
    public enum Instrument { Xylophone};

    public Instrument instrument;

    public Xylophone xylophone;

    [Range(1, 200)]
    public float bpm;

    public List<NoteSequence> noteSequences;

    private IEnumerator currentCorroutine;

    //Seconds per beat
    private float spb;

    void Awake()
    {
        spb = (1 / (bpm / 60))/2;
    }


    public void PlayNotes()
    {
        StopNotes();

        xylophone.SetNotesManager(null);

        currentCorroutine = Play();
        StartCoroutine(currentCorroutine);
    }

    public void StopNotes()
    {
        if(currentCorroutine != null)
        {
            StopCoroutine(currentCorroutine);
        } 
    }

    private IEnumerator Play()
    {
        for (int i = 0; i < noteSequences.Count; i++)
        {
            for (int j = 0; j < noteSequences[i].repeatTimes; j++)
            {
                for (int k = 0; k < noteSequences[i].notesList.Count; k++)
                {
                    xylophone.PlayNote((noteSequences[i].notesList[k]).ToString());

                    yield return new WaitForSeconds(spb);
                }
            }
        }
    }
}
