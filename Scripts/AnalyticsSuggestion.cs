using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.Home;

public class AnalyticsSuggestion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Activity>GetSuggestions()
    {
        List<Activity> suggestions = ActivitiesDatabase.GetLocalActivities(ActivitiesDatabase.serverActivities);

        suggestions = ActivitiesDatabase.GetActivitiesFromGenre(suggestions, CurrentStatsInfo.currentKid.genre);
        return suggestions;
    }
}
