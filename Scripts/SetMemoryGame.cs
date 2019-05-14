using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System.Linq;

[CommandInfo("Game", "Memory Game Configuration", "")]
public class SetMemoryGame : SetGame
{
    public MemoryManager memoryManager;

    public Sprite backSprite;

    public List<ImageAndSoundObject> sprites = new List<ImageAndSoundObject>();

    private List<Transform> dockingAnchors = new List<Transform>();

    public GameObject cardPrefab;
    
	public enum GameType{six = 0 ,eight = 1,twelve = 2};

    public GameType gameType;

    public GridLayoutGroup grid;

    private Card instatiatedCard;

    private List<int> availableSprites;

    private ImageAndSoundObject lastSelectedSprite;

    public Transform deckGameObject;

    private List<Card> cards = new List<Card>();

    private List<Card> cards2 = new List<Card>();

    private void Awake()
    {
        availableSprites = new List<int>();

        
    }

    private void OnEnable()
    {
        
    }

    private void ResetGame()
    {
        game.ResetGame();

        availableSprites.Clear();

        //Aapga as referências de cartas do manager, para quando começar um jogo já não haver carta selecionada
        memoryManager.ResetCards();

        for (int i = 0; i < sprites.Count; i++)
        {
            availableSprites.Add(i);
        }
    }


    public override void OnEnter()
    {
        ResetGame();

        foreach (Card card in grid.GetComponentsInChildren<Card>())
        {
            cards2.Add(card);
        }
        SortNewSprites();
        /*
        if (cards.Count == 0)
        {
            //InstantiateCards();
           // UpdateGrid();
            SortNewSprites();
            //StartCoroutine(MoveFirstTime());
        }
        else
        {
            //UpdateGrid();
            SortNewSprites();
            //StartCoroutine(MoveToCenter());
        }*/


        Continue();
    }

    public void InstantiateCards()
    {
        int maxPairs = 12;

        for (int i = 0; i < maxPairs; i++)
        {
            instatiatedCard = (Instantiate(cardPrefab, grid.transform)).GetComponent<Card>();
            instatiatedCard.SetCardId((int)i / 2);
            //instatiatedCard.normalSprite = backSprite;
            //instatiatedCard.SetNormalSprite();

            cards.Add(instatiatedCard);
        }
    }

    public void SortNewSprites()
    {
        
        for (int i = 0; i < cards2.Count; i++)
        {
            print("ue");
            if (i % 2 == 0)
            {
               // cards2[i].highlightedSprite = SortSprite().highlightedImage;
               // cards2[i].SetCardSound(lastSelectedSprite.soundName);
            }
            else
            {
               // cards2[i].highlightedSprite = lastSelectedSprite.highlightedImage;
               // cards2[i].SetCardSound(lastSelectedSprite.soundName);
            }

           // cards2[i].normalSprite = backSprite;
            //cards2[i].SetNormalSprite();
            cards2[i].transform.localScale = new Vector3(1, 1, 1);
            cards2[i].transform.localRotation = Quaternion.identity;
        }
    }

    public void UpdateGrid()
    {
        gameType = 0;//(GameType)memoryManager.GetPieces();

        switch (gameType)
        {
            case GameType.six:
                grid.constraintCount = 3;
                game.SetVictoryToWin(3);

                for (int i = 0; i < 6; i++)
                {
                    cards[i].gameObject.SetActive(false);
                }

                break;
            
            case GameType.eight:
                grid.constraintCount = 4;
                game.SetVictoryToWin(4);

                for (int i = 0; i < 4; i++)
                {
                    cards[i].gameObject.SetActive(false);
                }

                for (int i = 4; i < 6; i++)
                {
                    cards[i].gameObject.SetActive(true);
                }
                break;
            
            case GameType.twelve:
                grid.constraintCount = 4;
                game.SetVictoryToWin(6);

                for (int i = 0; i < 6; i++)
                {
                    cards[i].gameObject.SetActive(true);
                }
                break;
        }

        
    }

    private ImageAndSoundObject SortSprite()
    {
        int index = Random.Range(0, availableSprites.Count);
        lastSelectedSprite = sprites[availableSprites[index]];

        availableSprites.RemoveAt(index);

        return lastSelectedSprite;
    }
}