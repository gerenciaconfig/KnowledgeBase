using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("GameObject", "MultipleSetActive", "Activates or desactivates all game objcts of a list")]
public class FungusMultipleSetActive : Command
{
    public List<GameObject> gameObjects;

    public bool setActive;

    public override void OnEnter()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(setActive);
        }

        Continue();
    }
}
