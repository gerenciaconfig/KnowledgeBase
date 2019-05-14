using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Xylophone : SerializedMonoBehaviour
{
    public XylophoneNotesManager currentNotesManager;

    public List<Image> notes;

    private bool hasParticles;

    public NoteParticleInstantiator particleInstantiator;

    public Dictionary<string, Color> noteColorReference;

    public Dictionary<string, XylophoneNoteProperties> noteToPropertiesReference;

    public bool emitsSounds = true;

    public void SetNotesManager(XylophoneNotesManager notesManager)
    {
        currentNotesManager = notesManager;
    }

    public void PlayNote(string noteName)
    {
        if(noteName == "Pause")
        {
            return;
        }

        //Toca animação da nota
        noteToPropertiesReference[noteName].anim.SetTrigger("Click");

        //Toca som da nota
        if (emitsSounds) AudioManager.instance.PlaySound(noteName);

        //Se o Notes Manager for diferente de nulo, informa a nota tocada
        if(currentNotesManager != null)
        {
            currentNotesManager.StackNote(noteName);
        }

        //Se for para instanciar partículas, faz a chamada do Particle Instantiator
        if(hasParticles)
        {
            particleInstantiator.InstantiateParticleSortMaterial(noteToPropertiesReference[noteName].noteColor);
        }
    }

    public void ShowNotes()
    {
        currentNotesManager.ShowNotesFirstTime();
    }

    public void RepeatNoteSequence()
    {
        currentNotesManager.RepeatNotes();
    }

    public void SetInteractble(bool value)
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].raycastTarget = value;
        }
    }

    public void SetHasParticles(bool value)
    {
        hasParticles = value;
    }

    public void SetEmitsSounds(bool value)
    {
        emitsSounds = value;
    }
}
