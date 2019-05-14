using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BubblesPlayer
    {
        private static BubblesPlayer instance;
        public static BubblesPlayer Instance { get { if (instance == null) return new BubblesPlayer(); else return instance; } }
        public bool SaveData { get { return true; } }

        #region temporary stores
        private List<int> levelScores;  // temporary array for store levels scores
        private List<int> levelsStars;  // temporary array for store levels stars

        public IList<int> LevelsScoresStore { get { return levelScores.AsReadOnly(); } }
        public IList<int> LevelsStarsStore { get { return levelsStars.AsReadOnly(); } }
        #endregion temporary stores

        /// <summary>
        ///  set from map,  value start from 0 for first game level
        /// </summary>
        public static int CurrentLevel
        {
            get; set;
        }
        public BoostersHolder BoostHolder { get; private set; }

        #region keys
        private static string savePrefix = "mkbubbles_";
        private string saveCoinsKey = savePrefix + "coins"; // current coins
        private string saveLifeKey = savePrefix + "life"; // current lifes
        private string saveTopPassedLevelKey = savePrefix + "toppassedlevel";
        private string saveFbCoinsKey = savePrefix + "coinsfb"; // saved flag for facebook coins (only once)
        private string saveInfLifeTimeStampEnd = savePrefix + "inflifetsend"; // saved time stamp for infinite life end

        private string SaveLevelStarsKey { get { return CurrentLevel.ToString() + savePrefix + "levelstars"; } }  // level stars
        private string SaveLevelScoresKey { get { return CurrentLevel.ToString() + savePrefix + "levelscores"; } } // level score
        #endregion keys

        #region events
        public Action ChangeCoinsEvent;
        public Action ChangeLifeEvent;
        public Action ChangeStarsEvent;
        public Action ChangeScoreEvent;
        public Action PassLevelEvent;
        public Action StartInfiniteLifeEvent;
        public Action EndInfiniteLifeEvent;
        #endregion events

        #region default data
        public const int defCoinsCount = 500; // default data
        public const int defLifesCount = 5; // default data
        public const int maxStarsCount = 3;
        public int facebookCoins = 100;
        public int maxLifeCount = 5;
        #endregion default data

        #region per game saved properties
        public int TopPassedLevel // set from PassLevel method,value start from 0 for first game level
        {
            get; private set;
        }

        public int Coins { get; private set; }

        public int Life { get; private set; }
        #endregion per game properties

        #region per level properties
        public int StarCount // per level, zero at level start
        {
            get; private set;

        }

        public int LevelScore // per level, zero at level start
        {
            get; private set;
        }

        public int AverageScore { get; private set; }

        #endregion per level properties

        private BubblesPlayer()
        {
            instance = this;
            TopPassedLevel = -1; // none passed

            Coins = defCoinsCount;
            Life = defLifesCount;

            levelsStars = new List<int>();
            levelScores = new List<int>();

            // load saved data
            if (SaveData)
            {
                string key = saveCoinsKey;
                if (PlayerPrefs.HasKey(key)) Coins = PlayerPrefs.GetInt(key);

                key = saveLifeKey;
                if (PlayerPrefs.HasKey(key)) Life = PlayerPrefs.GetInt(key);

                key = saveTopPassedLevelKey;
                if (PlayerPrefs.HasKey(key))
                    TopPassedLevel = PlayerPrefs.GetInt(key);

                levelScores = GetPassedLevelsScores();
                levelsStars = GetPassedLevelsStars();
            }
            //  CurrentLevel = Mathf.Max(0, TopPassedLevel);
            Debug.Log("CurrentLevel: " + CurrentLevel);
        }

        #region score
        public void AddScore(int scores)
        {
            int tScore = Mathf.Max(0, LevelScore + scores);
            bool changed = (tScore != LevelScore);
            LevelScore = tScore;
            if (changed)
            {
                RaiseEvent(ChangeScoreEvent);
                SetStarsByScore();
            }
        }

        public void SetScore(int scores)
        {
            scores = Mathf.Max(0, scores);
            bool changed = (scores != LevelScore);
            LevelScore = scores;
            if (changed)
            {
                RaiseEvent(ChangeScoreEvent);
                SetStarsByScore();
            }
        }

        public void SetAverageScore(int averageScore)
        {
            AverageScore = averageScore;
        }

        /// <summary>
        /// Get score for level. If level not passed return 0;
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private int GetLevelScore(int level)
        {
            int result = 0;
            string key = SaveLevelScoresKey;
            if (PlayerPrefs.HasKey(key)) result = PlayerPrefs.GetInt(key);
            return result;
        }

        /// <summary>
        /// Return list of scores for all passed levels
        /// </summary>
        /// <returns></returns>
        private List<int> GetPassedLevelsScores()
        {
            List<int> result = new List<int>(TopPassedLevel + 1);

            for (int i = 0; i < TopPassedLevel; i++)
            {
                result.Add(GetLevelScore(i));
            }
            return result;
        }

        private void StoreScoresTemporary()
        {
            int count = levelScores.Count;
            if (count <= CurrentLevel)
            {
                levelScores.AddRange(new int[CurrentLevel - count + 10]); // add 10 for next scores
            }
            if (LevelScore > levelScores[CurrentLevel]) levelScores[CurrentLevel] = LevelScore;
        }
        #endregion score

        #region stars
        public void AddStars(int count)
        {
            int tStars = Mathf.Clamp(StarCount + count, 0, maxStarsCount);
            bool changed = (StarCount != tStars);
            StarCount = tStars;
            if (changed) RaiseEvent(ChangeStarsEvent);
            // Debug.Log("StarCount: " + StarCount);
        }

        public void SetStarsCount(int count)
        {
            count = Mathf.Clamp(count, 0, maxStarsCount);
            bool changed = (StarCount != count);
            StarCount = count;
            if (changed) RaiseEvent(ChangeStarsEvent);
            // Debug.Log("StarCount: " + StarCount);
        }

        /// <summary>
        /// Get stars for level. If level not passed return 0;
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLevelStars(int level)
        {
            int result = 0;
            string key = SaveLevelStarsKey;
            if (PlayerPrefs.HasKey(key)) result = PlayerPrefs.GetInt(key);
            return result;
        }

        /// <summary>
        /// Return list of scores for all passed levels
        /// </summary>
        /// <returns></returns>
        private List<int> GetPassedLevelsStars()
        {
            List<int> result = new List<int>(TopPassedLevel + 1);

            for (int i = 0; i <= TopPassedLevel; i++)
            {
                result.Add(GetLevelStars(i));
            }
            return result;
        }

        /// <summary>
        /// Store levels stars in temporary array
        /// </summary>
        private void StoreStarsTemporary()
        {
            int count = levelsStars.Count;
            if (count <= CurrentLevel)
            {
                levelsStars.AddRange(new int[CurrentLevel - count + 10]); // add 10 for next levels stars
            }
            if (StarCount > levelsStars[CurrentLevel]) levelsStars[CurrentLevel] = StarCount;
        }

        private void SetStarsByScore()
        {
            int stars = 0;
            if (LevelScore > (AverageScore)) stars = 3;
            else if (LevelScore > (AverageScore * 0.75f)) stars = 2;
            else if (LevelScore > (AverageScore * 0.45f)) stars = 1;
            SetStarsCount(stars);
        }
        #endregion stars

        #region life
        /// <summary>
        /// Add lifes count and save result
        /// </summary>
        /// <param name="count"></param>
        public void AddLifes(int count)
        {
            if (HasInfiniteLife()) return;
            int tCount = Mathf.Clamp(Life + count, 0, maxLifeCount);
            bool changed = (Life != tCount);
            Life = tCount;
            if (SaveData && changed)
            {
                string key = saveLifeKey;
                PlayerPrefs.SetInt(key, Life);
            }
            if (changed) RaiseEvent(ChangeLifeEvent);
        }

        /// <summary>
        /// Set lifes count and save result
        /// </summary>
        /// <param name="count"></param>
        public void SetLifesCount(int count)
        {
            if (HasInfiniteLife()) return;
            count = Mathf.Clamp(count, 0, maxLifeCount);
            bool changed = (Life != count);
            Life = count;
            if (SaveData && changed)
            {
                string key = saveLifeKey;
                PlayerPrefs.SetInt(key, Life);
            }
            if (changed) RaiseEvent(ChangeLifeEvent);
        }

        public void StartInfiniteLife(int hours)
        {
            SetLifesCount(5);
            PlayerPrefs.SetString(saveInfLifeTimeStampEnd, DateTime.Now.AddHours(hours).ToString());
            Debug.Log("End inf life: " + (DateTime.Now + new TimeSpan(hours, 0, 0)));
            RaiseEvent(StartInfiniteLifeEvent);
        }

        public void CleanInfiniteLife()
        {
            PlayerPrefs.SetString(saveInfLifeTimeStampEnd, DateTime.Now.ToString());
            RaiseEvent(EndInfiniteLifeEvent);
        }

        public void EndInfiniteLife()
        {
            RaiseEvent(EndInfiniteLife);
        }

        public bool HasInfiniteLife()
        {
            if (!PlayerPrefs.HasKey(saveInfLifeTimeStampEnd)) return false;
            DateTime end = GlobalTimer.DTFromSring(PlayerPrefs.GetString(saveInfLifeTimeStampEnd));// Debug.Log("end: "+end);
            return (DateTime.Now < end);
        }

        public TimeSpan GetInfLifeTimeRest()
        {
            if (!PlayerPrefs.HasKey(saveInfLifeTimeStampEnd)) return new TimeSpan(0, 0, 0);
            DateTime end = GlobalTimer.DTFromSring(PlayerPrefs.GetString(saveInfLifeTimeStampEnd));
            return end - DateTime.Now;
        }
        #endregion life

        #region coins
        /// <summary>
        /// Add coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void AddCoins(int count)
        {
            int tCount = Mathf.Max(0, Coins + count);
            bool changed = (Coins != tCount);
            Coins = tCount;
            if (SaveData && changed)
            {
                string key = saveCoinsKey;
                PlayerPrefs.SetInt(key, Coins);
            }
            if (changed) RaiseEvent(ChangeCoinsEvent);
        }

        /// <summary>
        /// Set coins and save result
        /// </summary>
        /// <param name="count"></param>
        public void SetCoinsCount(int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (Coins != count);
            Coins = count;
            if (SaveData && changed)
            {
                string key = saveCoinsKey;
                PlayerPrefs.SetInt(key, Coins);
            }
            if (changed) RaiseEvent(ChangeCoinsEvent);
        }

        /// <summary>
        /// Add facebook gift (only once), and save flag.
        /// </summary>
        public void AddFbCoins()
        {
            if (!PlayerPrefs.HasKey(saveFbCoinsKey) || PlayerPrefs.GetInt(saveFbCoinsKey) == 0)
            {
                PlayerPrefs.SetInt(saveFbCoinsKey, 1);
                AddCoins(facebookCoins);
            }
        }
        #endregion coins

        #region boosters
        /// <summary>
        /// Create boosterholder, if not created
        /// </summary>
        /// <param name="goSet"></param>
        public void CreateBoosterHolder(GameObjectsSet goSet)
        {
            if (BoostHolder == null)
                BoostHolder = new BoostersHolder(goSet, SaveData, savePrefix);
        }
        #endregion boosters

        #region raise events
        private void RaiseEvent(Action eventAction)
        {
            if (eventAction != null) eventAction();
        }
        #endregion raise events

        #region level
        public void PassLevel()
        {
            if (CurrentLevel > TopPassedLevel)
                TopPassedLevel = CurrentLevel;

            // store temporary data
            StoreScoresTemporary();
            StoreStarsTemporary();

            //save per level data
            if (SaveData)
            {
                // save stars
                int oldStarCount = 0;
                string key = SaveLevelStarsKey;
                if (PlayerPrefs.HasKey(key))
                    oldStarCount = PlayerPrefs.GetInt(key);
                if (oldStarCount < StarCount)
                    PlayerPrefs.SetInt(key, StarCount); // save only best stars result

                // save score
                int oldScore = 0;
                key = SaveLevelScoresKey;
                if (PlayerPrefs.HasKey(key))
                    oldScore = PlayerPrefs.GetInt(key);
                if (oldScore < LevelScore)
                    PlayerPrefs.SetInt(key, LevelScore); // save only best score result

                // save top passed level
                key = saveTopPassedLevelKey;
                PlayerPrefs.SetInt(key, TopPassedLevel);

            }
            if (PassLevelEvent != null) PassLevelEvent();
        }
        #endregion level

        internal static void Destroy()
        {
            instance = null;
            GC.Collect();
        }
    }
}