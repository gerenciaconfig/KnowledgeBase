using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;

    public Flowchart fadeFlowchart;
    public string openVar;
    public string closeVar;

    private void Awake()
    {
        instance = this;
    }

    public enum FadeTypes
    {
        fadeOpenClose,
        fadeOpen,
        fadeClose
    }

    public void SetOpen(GameObject obj)
    {
        fadeFlowchart.SetGameObjectVariable(openVar, obj);
    }

    public void SetClose(GameObject obj)
    {
        fadeFlowchart.SetGameObjectVariable(closeVar, obj);
    }

    public void FadeScreen(FadeTypes type)
    {
        fadeFlowchart.ExecuteBlock(type.ToString());
    }
}
