using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOsIncriveisTemp : MonoBehaviour
{
    public static GameManagerOsIncriveisTemp instance;
    public float timeToShowHint;
    public List<Animator> maskAnimators;
    [Space(10)]
    private GameStates gameState = GameStates.mainMenu;

    float timer;

    public void SetGameState(GameStates state)
    {
        gameState = state;
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    public enum GameStates
    {
        mainMenu,
        game1
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        HintTimer();
    }

    public void HintTimer()
    {
        timer += Time.deltaTime;

        if (GetGameState() == GameStates.game1)
        {
            if (timer >= timeToShowHint)
            {
                foreach (var item in maskAnimators)
                {
                    item.SetTrigger("hint");
                }

                ResetHintTimer();
            }
        }
        else
        {
            ResetHintTimer();
        }

    }

    public void MaskClick(Animator maskAnim)
    {
        ResetHintTimer();
        maskAnim.SetTrigger("click");
    }

    public void ResetHintTimer()
    {
        timer = 0;
    }

    public void RebindMaskAnimators()
    {
        foreach (var item in maskAnimators)
        {
            item.Rebind();
        }
    }




    
}
