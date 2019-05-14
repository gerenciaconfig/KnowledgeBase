using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Game", "Memory Game Movements", "")]
public class FungusMemoryGameCardController : Command
{
    public enum Action {MoveFirstTime, MoveToDeck, MoveToTable };

    public Action action;
    public GridLayoutGroup grid;

    List<Card> cards = new List<Card>();

    public Transform deckGameObject;

    public float waitTime;

    public override void OnEnter()
    {
        foreach (Card card in grid.GetComponentsInChildren<Card>())
        {
            cards.Add(card);
        }

        switch (action)
        {
            case Action.MoveFirstTime:
                StartCoroutine(MoveFirstTime());
                break;

            case Action.MoveToDeck:
                StartCoroutine(MoveToDeck());
                break;

            case Action.MoveToTable:
                StartCoroutine(MoveToTable());
                break;
        }

        Continue();
    }

    public IEnumerator MoveFirstTime()
    {
        grid.enabled = false;
        for (int i = 0; i < cards.Count; i++)
        {
            //cards[i].SetPos(deckGameObject.position);
            yield return new WaitForSeconds(waitTime);
        }
    }


    public IEnumerator MoveToDeck()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<Image>().enabled = true;
           // cards[i].GoToPos(deckGameObject.position);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public IEnumerator MoveToTable()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].ReturnToOriginalPos();
            yield return new WaitForSeconds(waitTime);
        }
    }

}
