using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    private Toggle helpToggle;
    public GameObject helpBaloon;


    void Start()
    {
        helpToggle = this.gameObject.GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, and output the state
        helpToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(helpToggle);
        });
        
    }

    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn == true)
        {
            helpBaloon.SetActive(true);
        }
        else
        {
            helpBaloon.SetActive(false);
        }

    }


}
