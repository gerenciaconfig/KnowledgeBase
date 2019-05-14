using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Scripting", "Activate Random Game Object", "")]
public class FungusActiveRandomGObj : Command
{
    public List<GameObject> gameObjects;

    public override void OnEnter()
    {
        int aux = Random.Range(0, gameObjects.Count);

        gameObjects[aux].SetActive(true);

        Continue();
    }
}
