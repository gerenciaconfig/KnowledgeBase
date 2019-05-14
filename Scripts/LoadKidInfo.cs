using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadKidInfo : MonoBehaviour
{
    public Image kidImage;
    public TextMeshProUGUI kidName;
    public PerfilImageScriptable perfilImageDB;

    private void OnEnable()
    {
        try
        {
            if (kidImage != null)
            {
                kidImage.sprite = perfilImageDB.perfilImageList[(int)CurrentStatsInfo.currentKid.imageId];
            }

            if (kidName != null)
            {
                kidName.text = CurrentStatsInfo.currentKid.name;
            }
        }
        catch (System.Exception)
        {
            //throw;
        }

    }
}
