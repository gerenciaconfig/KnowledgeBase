namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class SearchBar : MonoBehaviour
    {
        public enum SearchType { Dropdown, Text, Image };

        public SearchType searchType;

        public TMP_InputField inputField;

        public TMP_Dropdown dropdown;

        public ActivitiesDatabase activitiesDatabase;

        public GameObject activitiesThumbs;

        public Transform buttonsParents;

        private string searchText;

        private List<GameObject> instantiatedThumbs;

        /*
        private void OnEnable()
        {
            ClearSearchResults();
        }*/

        void Awake()
        {
            instantiatedThumbs = new List<GameObject>();

            if (activitiesDatabase == null)
            {
                activitiesDatabase = GameObject.FindGameObjectWithTag("ActivitiesDatabase").GetComponent<ActivitiesDatabase>();
            }

            if (searchType == SearchType.Dropdown)
            {
                dropdown.options.Clear();

                for (int i = 0; i < ActivitiesFranchises.franchises.Count; i++)
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData(ActivitiesFranchises.franchises[i].descricao));
                }
            }
        }

        public void SearchByInput()
        {
            searchText = inputField.text;

            print("search text :" + searchText);
            if (searchText == "")
            {
                ClearSearchResults();
            }
            else
            {
                SearchAllActivities();
            }           
        }

        public void SearchByDropDown()
        {
            searchText = (dropdown.options[dropdown.value].text);
            SearchActivitiesFrachise();
        }

        public void SearchActivityByText(string searchText)
        {
            this.searchText = searchText;
            SearchActivitiesFrachise();
        }


        private List<Activity> SearchAllActivities()
        {
            ClearSearchResults();

            List<Activity> activities = new List<Activity>();
            activities.AddRange(ActivitiesDatabase.serverActivities);

            string[] keywords = searchText.Split(' ');

            for (int i = 0; i < keywords.Length; i++)
            {
                for (int j = (activities.Count - 1); j >= 0; j--)
                {
                    if (!(activities[j].levelDTO.name.ToString()).ToLower().Contains(keywords[i])
                        && !(activities[j].levelDTO.franchise.ToString()).ToLower().Contains(keywords[i].ToLower()))
                    {
                        activities.RemoveAt(j);
                    }
                }
            }

            InstantiateActivitiesButtons(activities);

            return activities;
        }


        private List<Activity> SearchActivitiesFrachise()
        {
            ClearSearchResults();

            List<Activity> activities = new List<Activity>();

            for (int i = 0; i < ActivitiesDatabase.serverActivities.Count; i++)
            {
                if (ActivitiesDatabase.serverActivities[i].levelDTO.franchise.Contains(searchText))
                {
                    activities.Add(ActivitiesDatabase.serverActivities[i]);
                }
            }

            InstantiateActivitiesButtons(activities);

            return activities;
        }

        private void ClearSearchResults()
        {
            foreach (var thumb in instantiatedThumbs)
            {
                Destroy(thumb);
            }

            instantiatedThumbs.Clear();
        }

        private void InstantiateActivitiesButtons(List<Activity> activities)
        {
            GameObject instantiatedThumb;

            for (int i = 0; i < activities.Count; i++)
            {
                instantiatedThumb = Instantiate(activitiesThumbs, buttonsParents);
                instantiatedThumb.GetComponent<ThumbBehaviour>().SetActivity(activities[i]);                
                instantiatedThumb.GetComponent<LoadAssetFileBundle>().SetActivity(activities[i].levelDTO);

                instantiatedThumbs.Add(instantiatedThumb);
            }
        }
    }
}