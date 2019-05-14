using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FAQHelper : MonoBehaviour
{
    private Toggle helpToggle;
    public TextMeshProUGUI respostaObj;
    public string respostaText;
    public Scrollbar scrollbar;

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
            scrollbar.value = 1;
            respostaObj.text = respostaText;
        }
        else
        {
        }

    }
}
