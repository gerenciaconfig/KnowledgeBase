using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class MemoryManager : MonoBehaviour
{
    private Card card1;
    private Card card2;

    public float waitTimeCardFlip;

    public float waitTimeCardEnter;

    public Game game;

    private int piecesCount;

    public Sprite backSprite;

    private ImageAndSoundObject lastSelectedSprite;

    public List<ImageAndSoundObject> sprites = new List<ImageAndSoundObject>();

    public List<GameObject> grids;

    public Transform deck;

    public MovingProperties camera;

    private List<int> availableSprites = new List<int>();

    private List<int> availableCards = new List<int>();

    private List<Card> cards = new List<Card>();

    public Flowchart targetFlowchart;

    public Button plusButton;

    public Button minusButton;

    private Animator deckAnimator;

    public Vector3 deckPositionOut;
    public Vector3 deckPositionIn;
    public Vector3 deckPositionIn2;

    private ActivityAnalytics activityAnalytics;

    private void Start()
    {
        try
        {
            activityAnalytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<ActivityAnalytics>();
        }
        catch
        {

        }
        Camera.main.GetComponent<PuzzleCameraFix>().ToggleGameCamera();
    }

    private void OnEnable()
    {
        camera.SetPosAndRotation(deckPositionOut, Quaternion.identity);

        //Desativa todos os grids
        for (int i = 0; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }

        piecesCount = 0;

        foreach (SpriteRenderer sr in deck.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sprite = backSprite;
        }

        ResetGame(true);
    }

    public bool SelectCard(Card card)
    {
        if (card1 == null)
        {
            card1 = card;
        }
        else if (card2 == null)
        {
            card2 = card;

            if (card1.GetCardId() == card2.GetCardId())
            {
                StartCoroutine(MatchCards());
            }
            else
            {
                StartCoroutine(UnflipCard());
            }
        }
        else
        {
            return false;
        }

        card.DisableCard();
        return true;
    }

    private IEnumerator MatchCards()
    {
        game.AddVictory(true);
        if (activityAnalytics!=null)
        {
            activityAnalytics.AddRight();
        }
        yield return new WaitForSeconds(waitTimeCardFlip);
        card1.PlayCardSound();
        ResetCards();
    }

    private IEnumerator UnflipCard()
    {
        yield return new WaitForSeconds(waitTimeCardFlip);
        //AudioManager.instance.PlayRandomFailSound();
        if (activityAnalytics != null)
        {
            activityAnalytics.AddWrong();
        }

        yield return new WaitForSeconds(0.5f);
        card1.Flip();
        card1.EnableCard();
        card2.Flip();
        card2.EnableCard();
        ResetCards();
    }

    //Turns the refferences to the cards back to null
    public void ResetCards()
    {
        card1 = null;
        card2 = null;
    }

    //Aumenta a quantidade de peças do tabuleiroe e reseta o jogo (já com a nova quantidade de peças)
    public void IncreasePieces()
    {
        if (piecesCount < 2)
        {
            piecesCount++;
            ResetGame(false); 
        }
    }

    //Diminui a quantidade de peças do tabuleiro e reseta o jogo (já com a nova quantidade de peças)
    public void DecreasePieces()
    {
        if (piecesCount > 0)
        {
            piecesCount--;
            ResetGame(false);
        }
    }

    private void DisableButtons()
    {
        minusButton.interactable = false;
        plusButton.interactable = false;
    }

    public void ReactivateButtons()
    {
        minusButton.interactable = true;
        plusButton.interactable = true;

        if (piecesCount == 0)
        {
            minusButton.interactable = false;
        }
        else if (piecesCount == 2)
        {
            plusButton.interactable = false;
        }

    }

    //Percorre toda a lista de cartas e sorteia os sprites, sendo que, para cada carta par, a carta ímpar em seguida, possui o mesmo sprite (sendo seu par)
    public void SortNewSprites()
    {
        int lastIndex = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            availableCards.Add(i);
        }


        for (int i = 0; i < cards.Count; i++)
        {
            //Sorteia um número dentre as cartas restantes
            lastIndex = Random.Range(0, availableCards.Count);

            //Se o índice for ímpar, é a primeira carta do par
            if (i % 2 == 0)
            {
                cards[availableCards[lastIndex]].SetFrontSprite(SortSprite().highlightedImage);
                cards[availableCards[lastIndex]].SetCardSound(lastSelectedSprite.soundName);
            }
            //Se o índice for par, é a primeira carta do par
            else
            {
                cards[availableCards[lastIndex]].SetFrontSprite(lastSelectedSprite.highlightedImage);
                cards[availableCards[lastIndex]].SetCardSound(lastSelectedSprite.soundName);
            }

            cards[availableCards[lastIndex]].Reset();
            cards[availableCards[lastIndex]].SetCardId((int)i / 2);
            cards[availableCards[lastIndex]].SetBackSprite(backSprite);
            availableCards.RemoveAt(lastIndex);
        }
    }

    //Sorteia um sprite da lista de sprites disponíveis
    private ImageAndSoundObject SortSprite()
    {
        int index = Random.Range(0, availableSprites.Count);
        lastSelectedSprite = sprites[availableSprites[index]];

        availableSprites.RemoveAt(index);

        return lastSelectedSprite;
    }

    //Reseta a lista de sprites disponíveis
    private void ResetAvailableSprites()
    {
        availableSprites.Clear();

        for (int i = 0; i < sprites.Count; i++)
        {
            availableSprites.Add(i);
        }
    }

    private void ResetGame(bool firstTime)
    {
        DisableButtons();

        ResetCards();

        game.ResetGame();
        
        ResetAvailableSprites();

        //Seta a quantidade necessárias de pares que é preciso encontrar
        switch (piecesCount)
        {
            case 0:
                game.SetVictoryToWin(3);
                break;

            case 1:
                game.SetVictoryToWin(4);
                break;

            case 2:
                game.SetVictoryToWin(6);
                break;
        }

        StartCoroutine(SetTable(firstTime));
    }

    //Monta as cartas na mesa, além de sortear os sprites e ids de cada uma
    private IEnumerator SetTable(bool firtsTime)
    {
        if (!firtsTime)
        {
            yield return StartCoroutine(camera.MoveAndRotateTo(0.3f, deckPositionOut, Quaternion.identity));
            yield return new WaitForSeconds(0.1f);

            yield return StartCoroutine(MoveToDeck());
            yield return new WaitForSeconds(0.1f);
        }

        cards.Clear();

        //Pega as cartas do grid atual
        foreach (Card card in grids[piecesCount].GetComponentsInChildren<Card>())
        {
            cards.Add(card);
        }

        //Desativa todos os grids
        for (int i = 0; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }

        //Ativa o grid atual
        grids[piecesCount].gameObject.SetActive(true);


        //Sorteia sprites novos para todas as cartas do grid atual
        SortNewSprites();

        //Move as peças para a posição do deck
        SetToDeckPosition();

        if(firtsTime)
        {
            yield return StartCoroutine(camera.MoveAndRotateTo(0.3f, deckPositionOut, Quaternion.identity));
            yield return new WaitForSeconds(0.1f);
        }

        //Move as peças pela primeira vez, setando as posições na mesa
        yield return StartCoroutine(MoveToTable());

        ReactivateButtons();

        if(piecesCount == 0)
        {
            StartCoroutine(camera.MoveAndRotateTo(0.3f, deckPositionIn, Quaternion.identity));
        }
        else
        {
            StartCoroutine(camera.MoveAndRotateTo(0.3f, deckPositionIn2, Quaternion.identity));
        }
        
    }



    public void SetToDeckPosition()
    {
        //grids[piecesCount].enabled = false;
        for (int i = 0; i < cards.Count; i++)
        {
            if(i < 5)
            {
                cards[i].SetPosAndRotation(new Vector3(deck.position.x, deck.position.y + (i + 1) * 0.1f, deck.position.z), deck.rotation);
            }
            else
            {
                cards[i].SetPosAndRotation(new Vector3(deck.position.x, deck.position.y + 5 * 0.1f, deck.position.z), deck.rotation);
            }
            
        }
    }


    public IEnumerator MoveToDeck()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (i < 5)
            {
                StartCoroutine(cards[i].FlipAndMoveTo(new Vector3(deck.position.x, deck.position.y + (i + 1) * 0.1f, deck.position.z), deck.rotation));
            }
            else
            {
                StartCoroutine(cards[i].FlipAndMoveTo(new Vector3(deck.position.x, deck.position.y + 5 * 0.1f, deck.position.z), deck.rotation));
            }
            
            yield return new WaitForSeconds(waitTimeCardEnter);
        }
    }

    //Move cada carta para sua posição inicial
    public IEnumerator MoveToTable()
    {
        yield return new WaitForSeconds(0.25f);

        for (int i = cards.Count - 1; i >= 0; i--)
        {
            cards[i].ReturnToOriginalPos();
            yield return new WaitForSeconds(waitTimeCardEnter);
        }
    }
}