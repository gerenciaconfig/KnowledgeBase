using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioManagerChanger : MonoBehaviour
{
    public GameObject[] audioManagers;

    int currentSelected = 0;

    public TextMeshProUGUI textMesh;

	// Use this for initialization
	void Start ()
    {
        DesactivateAll();
        audioManagers[currentSelected].SetActive(true);
    }
	
	public void ChangeSelected()
    {
        currentSelected++;

        if (currentSelected == audioManagers.Length)
        {
            currentSelected = 0;
        }

        DesactivateAll();
        audioManagers[currentSelected].SetActive(true);
        textMesh.text = currentSelected.ToString();
    }

    private void DesactivateAll()
    {
        for (int i = 0; i < audioManagers.Length; i++)
        {
            audioManagers[i].SetActive(false);
        }
    }
}
