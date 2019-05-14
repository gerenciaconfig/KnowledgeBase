using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayEducaAttributes
{


    private static string currentActivity;
    private static string childID;

    public static void SetCurrentActivity(string id)
    {
        currentActivity = id;
    }

    public static void SetChildID(string id)
    {
        childID = id;
    }

    public static string GetCurrentActivity()
    {
        return currentActivity;
    }

    public static string GetchildID()
    {
        return childID;
    }
}

