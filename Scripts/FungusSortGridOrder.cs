using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("UI", "Sort Grid Order", "")]
public class FungusSortGridOrder : Command
{
    public GridLayoutGroup grid;

    public override void OnEnter()
    {
        

        StartCoroutine(Sort());
        

        Continue();
    }

    private IEnumerator Sort()
    {
        

        List<Transform> children = new List<Transform>();

        grid.enabled = true;

        if (grid.gameObject.activeSelf)
        {
            foreach (Transform child in grid.transform)
            {
                children.Add(child);
            }

            for (int i = 0; i < 15; i++)
            {
                int aux = Random.Range(0, children.Count);
                children[aux].SetSiblingIndex(0);
            }
        }

        yield return new WaitForEndOfFrame();

        grid.enabled = false;

    }
}
