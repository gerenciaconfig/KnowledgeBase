using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelResults
{
    public string dateEnd;
    public string dateStart;
    public int hits;
    public Int64 levelID;
    public int misses;
    public Int64 personID;

    public LevelResults(DateTime dateEnd, DateTime dateStart, int hits, Int64 levelID, int misses, Int64 personID)
    {
        
        this.dateEnd = dateEnd.ToLocalTime().ToString();
        this.dateStart = dateStart.ToLocalTime().ToString();
        this.hits = hits;
        this.levelID = levelID;
        this.misses = misses;
        this.personID = personID;
    }
}
