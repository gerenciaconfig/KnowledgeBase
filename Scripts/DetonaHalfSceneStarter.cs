using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Mkey;
using UnityEngine.SceneManagement;

public class DetonaHalfSceneStarter : MonoBehaviour
{
    [SerializeField] private Flowchart mapFlowchart;
    [SerializeField] private Flowchart menuFlowchart;
    [SerializeField] private Flowchart gameFlowchart;
    [SerializeField] private Flowchart playAgainFlowchart;

    [SerializeField] private string mapFlowchartStartBlock;
    [SerializeField] private string menuFlowchartStartBlock;
    [SerializeField] private string gameFlowchartStartBlock;
    [SerializeField] private string gameFlowchartContinueBlock;
    [SerializeField] private string playAgainFlowchartStartBlock;

    private const string stateKey = "DetonaHalfState";
    private const string mapState = "Map";
    private const string menuState = "Menu";
    private const string gameState = "Game";
    private const string playAgainState = "Play Again";
    private static string currentState = "";
    private static string lastState = "";

    void Start()
    {
        currentState = "";
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoadedForTheFirstTime();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentState == "")
        {
            SaveMenuState();
            print("Current State: " + currentState + ". (Got from default value)");
        }
        else
        {
            currentState = PlayerPrefs.GetString(stateKey, menuState);
            print("Current State: " + currentState + ".");
        }

        print("Entering switch statement. CurrentState: " + currentState + ".");
        switch (currentState)
        {
            case mapState:
                if (mapFlowchart)
                {
                    mapFlowchart.ExecuteBlock(mapFlowchartStartBlock);
                }
                break;

            case menuState:
                if (menuFlowchart)
                {
                    menuFlowchart.ExecuteBlock(menuFlowchartStartBlock);
                }
                break;

            case gameState:
                if (lastState != gameState)
                {
                    if (gameFlowchart) gameFlowchart.ExecuteBlock(gameFlowchartStartBlock);
                }
                else
                {
                    if (gameFlowchart) gameFlowchart.ExecuteBlock(gameFlowchartContinueBlock);
                }
                break;

            case playAgainState:
                if (playAgainFlowchart)
                {
                    playAgainFlowchart.ExecuteBlock(playAgainFlowchartStartBlock);
                }
                break;
        }
    }

    private void OnSceneLoadedForTheFirstTime()
    {
        if (currentState == "")
        {
            SaveMenuState();
            print("Current State: " + currentState + ". (Got from default value)");
        }
        else
        {
            currentState = PlayerPrefs.GetString(stateKey, menuState);
            print("Current State: " + currentState + ".");
        }

        print("Entering switch statement. CurrentState: " + currentState + ".");
        switch (currentState)
        {
            case mapState:
                if (mapFlowchart)
                {
                    mapFlowchart.ExecuteBlock(mapFlowchartStartBlock);
                }
                break;

            case menuState:
                if (menuFlowchart)
                {
                    menuFlowchart.ExecuteBlock(menuFlowchartStartBlock);
                }
                break;

            case gameState:
                if (lastState != gameState)
                {
                    if (gameFlowchart) gameFlowchart.ExecuteBlock(gameFlowchartStartBlock);
                }
                else
                {
                    if (gameFlowchart) gameFlowchart.ExecuteBlock(gameFlowchartContinueBlock);
                }
                break;

            case playAgainState:
                if (playAgainFlowchart)
                {
                    playAgainFlowchart.ExecuteBlock(playAgainFlowchartStartBlock);
                }
                break;
        }
    }

    public void SaveMapState()
    {
        currentState = mapState;
        PlayerPrefs.SetString(stateKey, currentState);
    }

    public void SaveMenuState()
    {
        currentState = menuState;
        PlayerPrefs.SetString(stateKey, currentState);
    }

    public void SaveGameState()
    {
        currentState = gameState;
        PlayerPrefs.SetString(stateKey, currentState);
    }

    public void SavePlayAgainState()
    {
        currentState = playAgainState;
        PlayerPrefs.SetString(stateKey, currentState);
    }

    public void SaveMapLastState()
    {
        lastState = mapState;
    }

    public void SaveMenuLastState()
    {
        lastState = menuState;
    }

    public void SaveGameLastState()
    {
        lastState = gameState;
    }

    public void SavePlayAgainLastState()
    {
        lastState = playAgainState;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(mapState, stateKey);
    }

    public void ResetGameLevels()
    {
        BubblesPlayer.CurrentLevel = 0;
    }
}
