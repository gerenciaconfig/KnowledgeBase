using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteSequence
{
    public enum notes { Do, Re, Mi, Fa, Sol, La, Si, Do2, Pause}

    public List<notes> notesList;

    [Range(1, 10)]
    public int repeatTimes = 1;
}
