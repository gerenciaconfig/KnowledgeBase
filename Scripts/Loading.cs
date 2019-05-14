using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static Loading instance;
    public GameObject telaLoading;

    private void Awake()
    {
        instance = this;
    }

    public void StartLoading()
    {
        telaLoading.gameObject.SetActive(true);
    }

    public void StopLoading()
    {
        telaLoading.gameObject.SetActive(false);
    }
}
