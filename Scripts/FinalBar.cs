using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arcolabs.Home;

public class FinalBar : MonoBehaviour
{
    public GameObject activityIconPrefab;

    AnalyticsSuggestion analyticsSuggestion;

    public Transform buttonsParent;

    public int activitiesToShow;

    private void Awake()
    {
        analyticsSuggestion = GameObject.FindGameObjectWithTag("Analytics").GetComponent<AnalyticsSuggestion>();
    }

    private void OnEnable()
    {
        List<Activity> activitiesOptions = analyticsSuggestion.GetSuggestions();

        GameObject instantiatedIcon;

        for (int i = 0; i < activitiesToShow; i++)
        {
            int index = Random.Range(0, activitiesOptions.Count);

            instantiatedIcon = Instantiate(activityIconPrefab, buttonsParent);

            instantiatedIcon.GetComponent<ActivityButton>().SetActivity(activitiesOptions[index]);

            activitiesOptions.RemoveAt(index);
        }
    }
}
