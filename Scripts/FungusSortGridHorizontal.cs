using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

[CommandInfo("UI", "Sort Horizontal Grid Order", "")]
public class FungusSortGridHorizontal : Command
{
    public HorizontalLayoutGroup grid;

    public override void OnEnter()
    {
        List<Transform> children = new List<Transform>();

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

        Continue();
    }
}
