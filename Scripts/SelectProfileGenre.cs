using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectProfileGenre : MonoBehaviour
{
    private void OnEnable()
    {
        CreateProfile.genre = "F";
    }

    public void SelectGenre(string genre)
    {
        CreateProfile.genre = genre;
    }
}
