using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XylophoneNoteBehaviour : MonoBehaviour
{
    public XylophoneNote note;

    Image noteImage;

    Animator anim;

    private bool playedNote;

    public void Awake()
    {
        noteImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    public void SetXylophoneNote(XylophoneNote xylophoneNote)
    {
        note = xylophoneNote;
        //playedNote = false;
        ResetNote();
    }

    public void ResetNote()
    {
        noteImage.color = note.noteColor;

        anim.SetTrigger("HideNow");
    }

    public void ShowNote()
    {
        anim.SetTrigger("Show");
    }

    public void HideNote()
    {
        anim.SetTrigger("Hide");
    }

    public void GlowNote()
    {
        anim.SetTrigger("Glow");
    }

    public void HideGlow()
    {
        anim.SetTrigger("HideGlow");
    }

    public void PlayNoteSound()
    {
        AudioManager.instance.PlaySound(note.noteSoundName);
    }

    public XylophoneNote GetXylophoneNote()
    {
        return note;
    }

    public void SetPlayedNote(bool value)
    {
        playedNote = value;

        anim.SetBool("PlayedNote", playedNote);
    }
}
