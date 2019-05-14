using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectProfileImage : MonoBehaviour
{
    public int imageId;
    public Image thisSelection;

    public static Image currentSelection;

    private void OnEnable()
    {
        CreateProfile.choosenImageId = 0;

        if (imageId == 0)
        {
            currentSelection = thisSelection;
            currentSelection.enabled = true;
        }
    }

    public void SelectImage()
    {
        currentSelection.enabled = false;
        currentSelection = thisSelection;
        currentSelection.enabled = true;

        CreateProfile.choosenImageId = imageId;
    }
}
