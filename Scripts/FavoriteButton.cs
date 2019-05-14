using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FavoriteButton : MonoBehaviour
{
    public Franchise favoriteFranchise;

    [Space(10)]
    public Image favImage;
    public Image selection;
    [Space(10)]
    public bool isSelected;

    public void SelectImage()
    {
        isSelected = !isSelected;

        selection.enabled = isSelected;

        if (isSelected)
        {
            FavoriteController.favoriteFranchiseIdList.Add(favoriteFranchise.id);
        }
        else
        {
            FavoriteController.favoriteFranchiseIdList.Remove(favoriteFranchise.id);
        }
    }
}
