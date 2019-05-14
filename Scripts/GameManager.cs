using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fungus;

public class GameManager : MonoBehaviour
{

    public List<Game> games = new List<Game>();

    [SerializeField]
    private int currentGame;

    public bool randomGameOrder;
    [Range(10, 20)]
    public int sortSeed;

    public bool playAllGames = true;

    public int gamesToPlay;

    public UnityEvent OnStartGames;

    public UnityEvent OnFinishAllGames;

    private List<Game> originalList = new List<Game>();

    private int gamesPlayed;

    public bool saveLoadGames = false;

    public int startSavingFrom;

    private ActivityAnalytics activityAnalytics;

    private void Awake()
    {
        originalList.AddRange(games);

        try
        {
            activityAnalytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<ActivityAnalytics>();
        } 
        catch
        {
            
        }      
    }

    /* void OnValidate()
    {
        if(saveLoadGames)
        {
            checkInt();
        }
    }

   public void checkInt()
   {
        if(currentGame > games.Count -1)
        {
            currentGame = games.Count -1;
        }
        
        if(currentGame < 0)
        {
            currentGame = 0;
        }        
       
       if(currentGame != PlayerPrefs.GetInt("Checkpoint" + PlayEducaAttributes.GetCurrentActivity() + PlayEducaAttributes.GetchildID()))
       {
           PlayerPrefs.SetInt(("Checkpoint" + PlayEducaAttributes.GetCurrentActivity() + PlayEducaAttributes.GetchildID()), currentGame);
       }
   }*/

    public void OnEnable()
    {
        games.Clear();
        games.AddRange(originalList);

        Game gameAux;

        if (playAllGames)
        {
            gamesToPlay = originalList.Count;
        }

        if (randomGameOrder == true)
        {
            for (int i = 0; i < sortSeed; i++)
            {
                int randomIndex = Random.Range(0, games.Count - 1);
                gameAux = games[randomIndex];
                games.RemoveAt(randomIndex);
                games.Add(gameAux);
            }
        }

        OnStartGames.Invoke();
    }

    public void GameStart()
    {
        DisabeAllGamesObjects();

        if(activityAnalytics != null)
        {
            Debug.Log("StartActivity");
            activityAnalytics.StartActivity();
        }
        

        if (saveLoadGames)
        {
            currentGame = PlayerPrefs.GetInt("Checkpoint" + PlayEducaAttributes.GetCurrentActivity() + PlayEducaAttributes.GetchildID());
        }
        else
        {
            currentGame = 0;
        }
        
        StartGame(currentGame);
    }

    private void StartGame(int gameIndex)
    {
        if(saveLoadGames && gameIndex >= startSavingFrom)
        {
            PlayerPrefs.SetInt(("Checkpoint" + PlayEducaAttributes.GetCurrentActivity() + PlayEducaAttributes.GetchildID()), gameIndex);
        }
        
        games[gameIndex].gameObject.SetActive(true);
        games[gameIndex].StartGame();
    }

    private void DisabeAllGamesObjects()
    {
        for (int i = 0; i < games.Count; i++)
        {
            games[i].gameObject.SetActive(false);
        }
    }

    public void GoToNextGame()
    {
        games[currentGame].gameObject.SetActive(false);

        if((!playAllGames && (currentGame + 1) >= gamesToPlay) || currentGame >= games.Count - 1)
        {
            EndAllGames();
        }
        else
        {
            currentGame++;
            StartGame(currentGame);
        }
    }

    public void GoToPreviousGame()
    {
        if (currentGame != 0)
        {
            games[currentGame].gameObject.SetActive(false);
            currentGame--;
            StartGame(currentGame);
        } else
        {
            Debug.Log("Already at first game!");
        }
    }

    public void GoToNextGameActivating()
    {
        games[currentGame].gameObject.SetActive(false);

        if (currentGame < games.Count - 1)
        {
            currentGame++;
            StartGame(currentGame);
        }
        else
        {
            EndAllGames();
        }
    }

    private void EndAllGames()
    {
        if (activityAnalytics != null)
        {
            Debug.Log("FinishActivity");
            activityAnalytics.FinishActivity();
        }

        if (saveLoadGames)
        {
            currentGame = 0;
            PlayerPrefs.SetInt(("Checkpoint" + PlayEducaAttributes.GetCurrentActivity() + PlayEducaAttributes.GetchildID()), currentGame);
        }
        OnFinishAllGames.Invoke();
    }

    public void AddRight()
    {
        if (activityAnalytics != null)
        {
            activityAnalytics.AddRight();
        }
    }

    public void AddWrong()
    {
        if (activityAnalytics != null)
        {
            activityAnalytics.AddWrong();
        }
    }
}