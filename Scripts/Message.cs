using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Message : MonoBehaviour
{
    public static Message instance;
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;

    private void OnEnable()
    {
        #region Singleton Statement
        if (instance == null)
        {
            instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public void Show(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }

    internal void Show(object eRROR_INTERNET_NO)
    {
        throw new NotImplementedException();
    }
}
