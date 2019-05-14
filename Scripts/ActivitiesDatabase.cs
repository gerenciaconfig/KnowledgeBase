namespace Arcolabs.Home
{
    using System.Collections;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using System.Net;
    using UnityEngine.UI;
    using Newtonsoft.Json;
    using System.Text;

    public class ActivitiesDatabase : SerializedMonoBehaviour
    {
        public static List<Activity> serverActivities;

        void Start()
        {
            StartCoroutine(PopulateActivities());
        }

        IEnumerator PopulateActivities()
        {
            yield return new WaitWhile(() => ServerActivitiesList.serverActivities == null);

            serverActivities = new List<Activity>();

            for (int i = 0; i < ServerActivitiesList.serverActivities.Count; i++)
            {
                Activity activity = new Activity(ServerActivitiesList.serverActivities[i]);
                serverActivities.Add(activity);
            }
        }

        public static List<Activity> GetLocalActivities(List<Activity> activities)
        {
            List<Activity> result = new List<Activity>();

            for (int i = 0; i < activities.Count; i++)
            {
                if(activities[i].downloaded)
                {
                    result.Add(activities[i]);
                }
            }

            return result;
        }

        public static List<Activity> GetActivitiesFromGenre(List<Activity> activities, string genre)
        {
            List<Activity> result = new List<Activity>();

            for (int i = 0; i < activities.Count; i++)
            {
                if (activities[i].levelDTO.genre == genre)
                {
                    if(activities[i].levelDTO.pathAndroid != null)
                    {
                        result.Add(activities[i]);
                    }  
                }
            }

            return result;
        }

        public static List<Activity> GetActivitiesFromArea(List<Activity> activities, string area)
        {
            List<Activity> result = new List<Activity>();

            for (int i = 0; i < activities.Count; i++)
            {
                if(activities[i].levelDTO.areaknowlagecode == area && (activities[i].levelDTO.pathAndroid != null && activities[i].levelDTO.pathAndroid != ""))
                {
                    result.Add(activities[i]);
                }
            }
            return result;
        }
    }
}