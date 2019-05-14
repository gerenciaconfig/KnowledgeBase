using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusGlobalWaitForSeconds : Command
{
    public string variableName;

    public override void OnEnter()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(PlayerPrefs.GetFloat(variableName));
        Continue();
    }
}