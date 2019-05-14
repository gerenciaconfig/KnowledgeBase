using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Sirenix.OdinInspector;

public class XylophoneNotesManager : SerializedMonoBehaviour
{
    public List<XylophoneNote> xylophoneNotesPressets;
    public List<XylophoneNoteBehaviour> xylophoneNotesGameObj;

    [Range(1, 9)]
    public int notesToSort;

    public float waitBetweenNotes;

    //Índice referente a nota que deve ser tocada da sequência correta
    private int currentSequenceNote;

    private int startingIndex;

    public Game game;

    private IEnumerator currentCorroutine;

    private bool showInProgress;

    public bool shuffleNotes = true;

    public void OnEnable()
    {
        ResetNotes();
    }

    public void ResetNotes()
    {
        currentSequenceNote = 0;

        startingIndex = 0;

        SortNotes();

        HideAllGlows();
    }

    public void SortNotes()
    {
        TurnOffNotes();

        for (int i = 0; i < notesToSort; i++)
        {
            xylophoneNotesGameObj[i].gameObject.SetActive(true);

            if (shuffleNotes)
            {
                int randomNoteIndex = Random.Range(0, xylophoneNotesPressets.Count);

                xylophoneNotesGameObj[i].SetXylophoneNote(xylophoneNotesPressets[randomNoteIndex]);
            }
            else
            {
                xylophoneNotesGameObj[i].ResetNote();
            }
            
        }
    }

    private void TurnOffNotes()
    {
        for (int i = 0; i < xylophoneNotesGameObj.Count; i++)
        {
            xylophoneNotesGameObj[i].gameObject.SetActive(false);
        }
    }

    public void ShowNotesFirstTime()
    {
        currentCorroutine = ShowNotes();

        StartCoroutine(currentCorroutine);
    }

    public IEnumerator ShowNotes()
    {
        showInProgress = true;

        for (int i = startingIndex; i < notesToSort; i++)
        {
            yield return new WaitForSeconds(waitBetweenNotes);

            xylophoneNotesGameObj[i].ShowNote();
        }

        if(currentSequenceNote < notesToSort)
        {
            xylophoneNotesGameObj[currentSequenceNote].GlowNote();
        }

        showInProgress = false;
    }

    public void RepeatNotes()
    {
        startingIndex = currentSequenceNote;
        StartCoroutine(HideAndShowNotes());
    }

    public void HideAllNotes(int startingIndex = 0)
    {
        for (int i = startingIndex; i < notesToSort; i++)
        {
            xylophoneNotesGameObj[i].HideNote();
        }
    }

    public IEnumerator HideAndShowNotes()
    {
        if(showInProgress)
        {
            yield break;
        }

        showInProgress = true;
        HideAllNotes(startingIndex);

        yield return new WaitForSeconds(0.3f);

        currentCorroutine = ShowNotes();

        StartCoroutine(currentCorroutine);
    }

    public void HideAllGlows()
    {
        for (int i = startingIndex; i < notesToSort; i++)
        {
            xylophoneNotesGameObj[i].SetPlayedNote(false);
        }
    }


    public void StackNote(string noteName)
    {
        if(currentSequenceNote < notesToSort && noteName == xylophoneNotesGameObj[currentSequenceNote].GetXylophoneNote().noteSoundName)
        {
            xylophoneNotesGameObj[currentSequenceNote].SetPlayedNote(true);

            currentSequenceNote++;

            if(currentSequenceNote == notesToSort)
            {
                game.AddVictory(true);
            }
        }
        else
        {
            currentSequenceNote = 0;

            startingIndex = currentSequenceNote;

            HideAllGlows();
        }
    }
}
