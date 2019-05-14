using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SavedClassesManeger : SerializedMonoBehaviour
{
    public static SavedClassesManeger instance;
    //public ActivitiesFranchises activitiesFranchises;
    [OdinSerialize]
    public List<SavedClass> classes;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(UpdateAll());

        //LoadAll();
    }

    private IEnumerator UpdateAll()
    {
        for (int i = 0; i < classes.Count; i++)
        {
            
            CurrentStatsInfo.isDownloading = true;

            classes[i].UpdateClass();

            yield return new WaitUntil(() => CurrentStatsInfo.isDownloading == false);

            ClassSaver.SaveClass(classes[i]);
        }
    }

    public void LoadAll()
    {
        for (int i = 0; i < classes.Count; i++)
        {   
            classes[i] = (SavedClass)ClassSaver.LoadClass(classes[0].GetType().ToString());
        }
    }

    public System.Object GetClass(System.Type classToSearch)
    {
        foreach (var item in classes)
        {
            if (item.GetType() == classToSearch.GetType())
            {
                return item;
            }
        }

        Debug.LogError("GetClass(System.Object classToSearch) -> Classe não encontrada!");
        return null;
    }
}
