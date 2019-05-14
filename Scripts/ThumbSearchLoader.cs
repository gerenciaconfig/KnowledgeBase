using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbSearchLoader : MonoBehaviour
{
    public GameObject ThumbPrefab;
    public Transform thumbGridTransform;

    private void OnEnable()
    {
        FillThumbGrid();
    }

    public void FillThumbGrid()
    {
        foreach (var item in ServerActivitiesList.serverActivities)
        {
            LoadAssetFileBundle thumb = Instantiate(ThumbPrefab.GetComponent<LoadAssetFileBundle>(), thumbGridTransform);

            thumb.SetActivity(item);
        }
    }
}


