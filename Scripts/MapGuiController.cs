using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class MapGuiController : MonoBehaviour
    {
        public static MapGuiController Instance;
        [SerializeField]
        private Text coinsText;
        [SerializeField]
        private Text lifesText;
        [SerializeField]
        private Image infiniteIcon;
        [SerializeField]
        private Text timerText;

        #region regular
        void Awake()
        {
            if (Instance) Destroy(Instance.gameObject);
            Instance = this;
        }

        void Start()
        {
            InfiniteLifeTimer.Instance.InitStart();
            LifeIncTimer.Instance.InitStart();
            Debug.Log("Map gui controller started");
            BubblesPlayer.Instance.ChangeCoinsEvent += RefreshCoins;
            BubblesPlayer.Instance.ChangeLifeEvent += RefreshLifes;
            if(timerText) timerText.text = restMinutes.ToString("00") + ":" + restSeconds.ToString("00");
            Refresh();
        }

        void OnGUI()
        {
            RefresTimerText();
        }

        void OnDestroy()
        {
            BubblesPlayer.Instance.ChangeCoinsEvent -= RefreshCoins;
            BubblesPlayer.Instance.ChangeLifeEvent -= RefreshLifes;
        }
        #endregion regular

        private void RefreshLifes()
        {
           if(lifesText) lifesText.text = BubblesPlayer.Instance.Life.ToString();
        }

        private void RefreshCoins()
        {
          if(coinsText) coinsText.text = BubblesPlayer.Instance.Coins.ToString();
        }

        float restHours = 0;
        float restMinutes = 0;
        float restSeconds = 0;
        private void RefresTimerText()
        {
            LifeIncTimer lifeIncTimer = LifeIncTimer.Instance;
            InfiniteLifeTimer infiniteLifeTimer = InfiniteLifeTimer.Instance;

            if (timerText)
            {
                if(infiniteLifeTimer && infiniteLifeTimer.IsWork)
                {
                    if (restHours != infiniteLifeTimer.RestHours || restMinutes != infiniteLifeTimer.RestMinutes || restSeconds != infiniteLifeTimer.RestSeconds)
                    {
                        restHours = infiniteLifeTimer.RestHours;
                        restMinutes = infiniteLifeTimer.RestMinutes;
                        restSeconds = infiniteLifeTimer.RestSeconds;
                        timerText.text = restHours.ToString("00") + ":" + restMinutes.ToString("00") + ":" + restSeconds.ToString("00");
                    }
                    if(lifesText.gameObject.activeSelf) lifesText.gameObject.SetActive(false);
                    if(!infiniteIcon.gameObject.activeSelf) infiniteIcon.gameObject.SetActive( true);
                    return;
                }

                if (lifeIncTimer)
                {
                    if (restMinutes != lifeIncTimer.RestMinutes || restSeconds != lifeIncTimer.RestSeconds)
                    {
                        restMinutes = lifeIncTimer.RestMinutes;
                        restSeconds = lifeIncTimer.RestSeconds;
                        timerText.text = restMinutes.ToString("00") + ":" + restSeconds.ToString("00");
                    }
                    if (!lifesText.gameObject.activeSelf) lifesText.gameObject.SetActive(true);
                    if (infiniteIcon.gameObject.activeSelf) infiniteIcon.gameObject.SetActive(false);
                }
            }
        }

        public void Refresh()
        {
            RefreshLifes();
            RefreshCoins();
            RefresTimerText();
        }

        /// <summary>
        /// Set all interactable as activity
        /// </summary>
        /// <param name="activity"></param>
        public void SetControlActivity(bool activity)
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        public void Settings_Click()
        {
            if (GuiController.Instance) GuiController.Instance.ShowSettings();
        }

        public void CoinShop_Click()
        {
            if (GuiController.Instance) GuiController.Instance.ShowCoinsShop();
        }

        public void LifeShop_Click()
        {
            if (GuiController.Instance) GuiController.Instance.ShowLifeShop();
        }

    }
}