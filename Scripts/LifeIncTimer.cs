using UnityEngine;

namespace Mkey
{
    public class LifeIncTimer : MonoBehaviour
    {
        private GlobalTimer gTimer;
        private string lifeIncTimerName = "lifeinc";
        [Tooltip("Time span to life increase, minutes")]
        [SerializeField]
        private int lifeIncTime = 15; // 
        [Tooltip("Calc global time (between games)")]
        [SerializeField]
        private bool calcGlobalTime = true; // 
        [SerializeField]
        private bool restartAuto = true; // 

        private float restDays = 0;
        private float restHours = 0;
        private float restMinutes = 0;
        private float restSeconds = 0;

        public bool IsWork { get; private set; }

        #region properties
        public float RestMinutes { get { return restMinutes; } }
        public float RestSeconds { get { return restSeconds; } }
        public static LifeIncTimer Instance;
        #endregion properties

        #region regular
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void InitStart()
        {
            BubblesPlayer.Instance.StartInfiniteLifeEvent += StartInfiniteLifeHandler;
            BubblesPlayer.Instance.EndInfiniteLifeEvent += EndInfiniteLifeHandler;
            BubblesPlayer.Instance.ChangeLifeEvent += ChangeLifeHandler;
            if (!BubblesPlayer.Instance.HasInfiniteLife() && BubblesPlayer.Instance.Life < 5 && !IsWork)
            {
                CreateNewTimerAndStart();
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
            BubblesPlayer.Instance.EndInfiniteLifeEvent -= EndInfiniteLifeHandler;
            BubblesPlayer.Instance.ChangeLifeEvent -= ChangeLifeHandler;
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
            BubblesPlayer.Instance.AddLifes(1);
            if (restartAuto) gTimer.Restart();
        }
        #endregion timerhandlers

        #region player life handlers

        private void ChangeLifeHandler()
        {
            Debug.Log("change timer");
            if (BubblesPlayer.Instance.Life >= 5 && IsWork)
            {
                restDays = 0;
                restHours = 0;
                restMinutes = 0;
                restSeconds = 0;
                IsWork = false;
                Debug.Log("timer stop");
            }
            else if(BubblesPlayer.Instance.Life < 5 && !IsWork)
            {
                CreateNewTimerAndStart();
            }
        }

        private void StartInfiniteLifeHandler()
        {
            restDays = 0;
            restHours = 0;
            restMinutes = 0;
            restSeconds = 0;
            IsWork = false;
        }

        private void EndInfiniteLifeHandler()
        {
            CreateNewTimerAndStart();
        }
        #endregion player life handlers

        private void CreateNewTimerAndStart()
        {
            Debug.Log("new timer");
            gTimer = new GlobalTimer(lifeIncTimerName, 0, 0, lifeIncTime, 0, true);
            gTimer.OnTickRestDaysHourMinSec += TickRestDaysHourMinSecHandler;
            gTimer.OnTimePassed += TimePassed;
            IsWork = true;
        }
    }
}