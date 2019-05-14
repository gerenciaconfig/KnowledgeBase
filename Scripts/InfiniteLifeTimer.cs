using System;
using UnityEngine;

namespace Mkey
{
    public class InfiniteLifeTimer : MonoBehaviour
    {
        private GlobalTimer gTimer;
        private string lifeIncTimerName = "lifeinfinite";

        private float restDays = 0;
        private float restHours = 0;
        private float restMinutes = 0;
        private float restSeconds = 0;
       
        #region properties
        public float RestDays { get { return restDays; } }
        public float RestHours { get { return restHours; } }
        public float RestMinutes { get { return restMinutes; } }
        public float RestSeconds { get { return restSeconds; } }
        public bool IsWork { get; private set; }
        public static InfiniteLifeTimer Instance;
        #endregion properties

        #region regular
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void InitStart()
        {
            IsWork = false;
            BubblesPlayer.Instance.StartInfiniteLifeEvent +=  StartInfiniteLifeHandler;
            if (BubblesPlayer.Instance.HasInfiniteLife())
            {
                StartInfiniteLifeHandler();
            }
        }

        void Update()
        {
            if (IsWork)
                gTimer.Update();
        }

        private void OnDestroy()
        {
            BubblesPlayer.Instance.StartInfiniteLifeEvent -= StartInfiniteLifeHandler;
        }
        #endregion regular

        #region timerhandlers
        private void TickRestDaysHourMinSecHandler(int d, int h, int m, float s)
        {
            restDays = d;
            restHours = h;
            restMinutes = m;
            restSeconds = s;
        }

        private void TimePassed()
        {
            IsWork = false;
            BubblesPlayer.Instance.EndInfiniteLife();
        }
        #endregion timerhandlers

        #region player life handlers
        private void StartInfiniteLifeHandler()
        {
            TimeSpan ts = BubblesPlayer.Instance.GetInfLifeTimeRest();
            gTimer = new GlobalTimer(lifeIncTimerName, ts.Days, ts.Hours, ts.Minutes, ts.Seconds, true);
            gTimer.OnTickRestDaysHourMinSec += TickRestDaysHourMinSecHandler;
            gTimer.OnTimePassed += TimePassed;
            IsWork = true;
        }
        #endregion player life handlers
    }
}