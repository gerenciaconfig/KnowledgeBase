using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    [Serializable]
    public class GiftConstruct
    {
        [SerializeField]
        private int life;
        public int Life { get { return life; } }

        [SerializeField]
        private int coins;
        public int Coins { get { return coins; } }

        [SerializeField]
        private List<int> boosters;
        public ICollection<int> Boosters { get { return boosters.AsReadOnly(); } }

        public Action ChangeLifeCountEvent;
        public Action ChangeCoinsCountEvent;
        public Action ChangeBoostersEvent;
        public Action SaveEvent; // need to save object data, used for constructor

        #region lifes
        /// <summary>
        /// Add lifes gift count
        /// </summary>
        /// <param name="count"></param>
        public void AddLifes(int count)
        {
            int tCount = Mathf.Max(0, life + count);
            bool changed = (life != tCount);
            life = tCount;
            if (changed)
            {
                OnChangeLifeCountEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Set lifes gift count
        /// </summary>
        /// <param name="count"></param>
        public void SetLifesCount(int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (life != count);
            life = count;
            if (changed)
            {
                OnChangeLifeCountEvent();
                OnSaveEvent();
            }
        }
        #endregion lifes

        #region coins
        /// <summary>
        /// Add coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void AddCoins(int count)
        {
            int tCount = Mathf.Max(0, coins + count);
            bool changed = (coins != tCount);
            coins = tCount;

            if (changed)
            {
                OnChangeCoinsCountEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Set coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void SetCoinsCount(int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (coins != count);
            coins = count;

            if (changed)
            {
                OnChangeCoinsCountEvent();
                OnSaveEvent();
            }
        }
        #endregion coins

        #region boosters
        /// <summary>
        /// Add booster gift
        /// </summary>
        /// <param name="count"></param>
        public void AddBooster(int boosterID)
        {
            boosters.Add(boosterID);
            OnChangeBoosterEvent();
            OnSaveEvent();
        }

        /// <summary>
        /// Add booster gift, count
        /// </summary>
        /// <param name="count"></param>
        private void AddBooster(int boosterID, int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (count != 0);
            if (changed)
            {
                for (int i = 0; i < count; i++)
                {
                    boosters.Add(boosterID);
                }
                OnChangeBoosterEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Remove booster gift, count
        /// </summary>
        /// <param name="count"></param>
        private void RemoveBooster(int boosterID, int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (count != 0);
            if (changed)
            {
                for (int i = 0; i < count; i++)
                {
                    boosters.Remove(boosterID);
                }
                OnChangeBoosterEvent();
                OnSaveEvent();
            }
        }

        /// <summary>
        /// Remove booster gift
        /// </summary>
        /// <param name="count"></param>
        public void RemoveBooster(int boosterID)
        {
            if (boosters.Contains(boosterID))
            {
                boosters.Remove(boosterID);
                OnChangeBoosterEvent();
                OnSaveEvent();
            }

        }

        /// <summary>
        /// Retun count of gift boosters by id
        /// </summary>
        /// <param name="boosterID"></param>
        /// <returns></returns>
        public int GetBoosterCount(int boosterID)
        {
            int count = 0;
            if (boosters == null || boosters.Count == 0) return 0;
            for (int i = 0; i < boosters.Count; i++)
            {
                if (boosters[i] == boosterID) count++;
            }
            return count;
        }

        /// <summary>
        /// Set coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void SetBoosterCount(int count, int boosterId)
        {
            int bCount = GetBoosterCount(boosterId);
            count = Mathf.Max(0, count);
            int dC = count - bCount;
            bool changed = (dC!=0);

            if (changed)
            {
                Debug.Log("canged : " + count);
                if (dC > 0)
                    AddBooster(boosterId, dC);
                else
                    RemoveBooster(boosterId, -dC);
            }
        }
        #endregion boosters

        #region raise events
        private void OnChangeLifeCountEvent()
        {
            if (ChangeLifeCountEvent != null) ChangeLifeCountEvent();
        }

        private void OnChangeCoinsCountEvent()
        {
            if (ChangeCoinsCountEvent != null) ChangeCoinsCountEvent();
        }

        private void OnChangeBoosterEvent()
        {
            if (ChangeBoostersEvent != null) ChangeBoostersEvent();
        }

        private void OnSaveEvent()
        {
            if (SaveEvent != null) SaveEvent();
        }
        #endregion raise events

    }
}