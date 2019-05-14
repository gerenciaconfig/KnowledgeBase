using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{

    public class Game : MonoBehaviour
    {
        public UnityEvent OnGameStart;
        public UnityEvent OnGameEnd;
        public UnityEvent OnEachVictory;
        public UnityEvent StopGame;

        public int victoriesToWin = 1;

        private int victoriesCount = 0;

        private bool sucess;


        public void AddVictory(bool sucess)
        {
            victoriesCount++;

            this.sucess = sucess;

            OnEachVictory.Invoke();

            if (victoriesCount == victoriesToWin)
            {
                OnGameEnd.Invoke();
                ResetGame();
            }
        }

        public void RemoveVictory(bool sucess)
        {
            victoriesCount--;

            this.sucess = sucess;

            //OnEachVictory.Invoke();
        }

        public void EndGameTest()
        {
            sucess = false;
            OnGameEnd.Invoke();
            ResetGame();
        }
        
        public bool GetSucess()
        {
            return sucess;
        }

        public void stopGame()
        {
            StopGame.Invoke();
            ResetGame();
        }

        public void ResetGame()
        {
            victoriesCount = 0;
        }

        public void SetVictoryToWin(int x)
        {
            victoriesToWin = x;
        }

        public void StartGame()
        {
            ResetGame();

            OnGameStart.Invoke();
        }
    }
}
