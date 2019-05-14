using System;
using System.Collections;
using UnityEngine;
/*  how to use
  
    private GlobalTimer gTimer;
    private string lifeIncTimerName = "lifeinc";
    [Tooltip ("Time span to life increase")]
    [SerializeField]
    private int  lifeIncTime = 15; // 

    [Tooltip("Calc global time (between games)")]
    [SerializeField]
    private bool calcGlobalTime = true; // 
    private float currMinutes = 0;
    private float currSeconds = 0;

    void Start()
    {
        gTimer = new GlobalTimer(lifeIncTimerName, 0, 0, lifeIncTime, 0, !calcGlobalTime);
        gTimer.OnTickRestDaysHourMinSec += TickRestDaysHourMinSecHandler;
        gTimer.OnTimePassed += TimePassed;
    }

    void OnDestroy()
    {
        gTimer.OnTickRestDaysHourMinSec -= TickRestDaysHourMinSecHandler;
        gTimer.OnTimePassed -= TimePassed;
    }

    void Update()
    {
        gTimer.Update();
    }
 
#region timerhandlers
    private void TickRestDaysHourMinSecHandler(int d, int h, int m, float s)
    {
        currMinutes = m;
        currSeconds = s;
        RefresTimerText();
    }

    private void TimePassed()
    {
        BubblesPlayer.Instance.AddLifes(1);
        gTimer.Restart();
    }
#endregion timerhandlers

    private void RefresTimerText()
    {
        if (timerText) timerText.text = currMinutes.ToString("00") + ":" + currSeconds.ToString("00");
    }

*/

/* changes
  
    13.11.18
    add time span validation
        daySpan = Mathf.Max(0, daySpan);
        hoursSpan = Mathf.Max(0, hoursSpan);
        minutesSpan = Mathf.Max(0, minutesSpan);
        secondsSpan = Mathf.Max(0, secondsSpan);
    21.11.18
    add clamp   restTime = Mathf.Max(0, restTime);
        days = Mathf.Max(0, restTime.Days);
        hours = Mathf.Max(0, restTime.Hours);
        minutes = Mathf.Max(0, restTime.Minutes);
        seconds = Mathf.Max(0, restTime.Seconds + Mathf.RoundToInt(restTime.Milliseconds*0.001f));
 */
namespace Mkey
{
    public class SessionTimer
    {
        private float lastTime = 0;
        private float passedTime = 0;
        private float passedTimeOld = 0;
        private bool pause = false;

        public Action <float> OnTickPassedSeconds;
        public Action<float> OnTickRestSeconds;
        public Action<int, int, int, float> OnTickPassedDaysHourMinSec;
        public Action<int ,int, int, float> OnTickRestDaysHourMinSec;

        public Action OnTimePassed;

        public bool IsTimePassed
        {
            get { return passedTime >= InitTime; }
        }

        public void PassedTime (out int days,  out int hours, out int minutes, out float seconds)
        {
            days = (int)(passedTimeOld / (86400f));
            float rest = passedTimeOld - days * 86400f;

            hours =(int) (rest / (3600f));
            rest = rest - hours * 3600f;

            minutes = (int) (rest / 60f);
            rest = rest - minutes * 60f;

            seconds = rest;
        }

        public void RestTime(out int days, out int hours, out int minutes, out float seconds)
        {
            float restTime = InitTime - passedTimeOld;
            restTime = Mathf.Max(0, restTime);

            days = (int)(restTime / (86400f));
            float rest = restTime - days * 86400f;

            hours = (int)(rest / (3600f));
            rest = rest - hours * 3600f;

            minutes = (int)(rest / 60f);
            rest = rest - minutes * 60f;

            seconds = rest;
        }

        public float InitTime
        {
            get; private set;
        }

        public SessionTimer(float secondsSpan)
        {
            Debug.Log("SessionTimer: " + secondsSpan);
            secondsSpan = Mathf.Max(0, secondsSpan);
            InitTime = secondsSpan;
            pause = true;
        }

        public SessionTimer(float minutesSpan, float secondsSpan)
        {
            minutesSpan = Mathf.Max(0, minutesSpan);
            secondsSpan = Mathf.Max(0, secondsSpan);
            InitTime = minutesSpan*60f + secondsSpan;
            pause = true;
        }

        public SessionTimer(float hoursSpan,  float minutesSpan, float secondsSpan)
        {
            hoursSpan = Mathf.Max(0, hoursSpan);
            minutesSpan = Mathf.Max(0, minutesSpan);
            secondsSpan = Mathf.Max(0, secondsSpan);
            InitTime = hoursSpan * 3600f  +  minutesSpan * 60f + secondsSpan;
            pause = true;
        }

        public SessionTimer(float daySpan, float hoursSpan, float minutesSpan, float secondsSpan)
        {
            daySpan = Mathf.Max(0, daySpan);
            hoursSpan = Mathf.Max(0, hoursSpan);
            minutesSpan = Mathf.Max(0, minutesSpan);
            secondsSpan = Mathf.Max(0, secondsSpan);
            InitTime = daySpan * 24f * 3600f + hoursSpan * 3600f + minutesSpan * 60f + secondsSpan;
            pause = true;
        }

        public void Start()
        {
            Debug.Log("start timer, istaimepassed " + (IsTimePassed) + " : " + passedTime);
            if (IsTimePassed) return;
            pause = false;
        }

        public void Pause()
        {
            pause = true;
        }

        public void Restart()
        {
            passedTime = 0;
            passedTimeOld = 0;
            pause = false;
        }

        bool firstUpdate = true;
        /// <summary>
        /// for timer update set Time.time param
        /// </summary>
        /// <param name="time"></param>
        public void Update(float gameTime)
        {
            if (firstUpdate)
            {
                firstUpdate = false;
                lastTime = gameTime;
            }

            float dTime = gameTime - lastTime;
            lastTime = gameTime;
            if (pause) return;

            passedTime += dTime;
            if (IsTimePassed && !pause)
            {
                pause = true;
                if (OnTimePassed != null) OnTimePassed();
            }


            if(passedTime - passedTimeOld >= 1.0f)
            {
                passedTimeOld = Mathf.Floor(passedTime);
                if (OnTickPassedSeconds != null)
                {
                    OnTickPassedSeconds(passedTimeOld);
                }
                if (OnTickRestSeconds != null)
                {
                    OnTickRestSeconds(InitTime - passedTimeOld);
                }
                if (OnTickPassedDaysHourMinSec != null)
                {
                    int d =0;
                    int h = 0;
                    int m = 0;
                    float s = 0;
                    PassedTime(out d, out h, out m, out s);
                    OnTickPassedDaysHourMinSec(d, h,m, s);
                }
                if (OnTickRestDaysHourMinSec != null)
                {
                    int d = 0;
                    int h = 0;
                    int m = 0;
                    float s = 0;
                    RestTime(out d, out h, out m, out s);
                    OnTickRestDaysHourMinSec(d, h, m, s);
                }
            }
        }
    }

    public class GlobalTimer
    {
        private double initTime; // time span in seconds
        private bool cancel = false;
        private string name = "timer_default";

        private DateTime startDT;
        private DateTime lastDT;
        private DateTime endDt ;
        private DateTime currentDT;

        string lastTickSaveKey;
        string startTickSaveKey;
        string initTimeSaveKey;

        public Action<double> OnTickPassedSeconds;
        public Action<double> OnTickRestSeconds;
        public Action<int, int, int, float> OnTickPassedDaysHourMinSec;
        public Action<int, int, int, float> OnTickRestDaysHourMinSec;
        public Action OnTimePassed;

        public bool IsTimePassed
        {
            get { return cancel; }
        }

        /// <summary>
        /// Returns the elapsed time from the beginning
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public void PassedTime( out int days, out int hours, out int minutes, out float seconds)
        {
            TimeSpan passedTime = (!cancel) ? lastDT - startDT : endDt - startDT ;
            days = passedTime.Days;
            hours = passedTime.Hours;
            minutes = passedTime.Minutes;
            seconds =passedTime.Seconds + Mathf.RoundToInt(passedTime.Milliseconds * 0.001f);
        }

        /// <summary>
        /// Returns the remaining time to the end
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public void RestTime(out int days, out int hours, out int minutes, out float seconds)
        {
            TimeSpan restTime = (!cancel) ? endDt - lastDT : endDt - endDt;
            days = Mathf.Max(0, restTime.Days);
            hours = Mathf.Max(0, restTime.Hours);
            minutes = Mathf.Max(0, restTime.Minutes);
            seconds = Mathf.Max(0, restTime.Seconds + Mathf.RoundToInt(restTime.Milliseconds*0.001f));
        }

        public double InitTime
        {
            get { return initTime; }
        }

        public GlobalTimer(string timerName, float daySpan, float hoursSpan, float minutesSpan, float secondsSpan, bool removeOld)
        {
            name = timerName;
            lastTickSaveKey = name + "_lastTick";
            startTickSaveKey = name + "_startTick";
            initTimeSaveKey = name + "_initTime";
            daySpan = Mathf.Max(0, daySpan);
            hoursSpan = Mathf.Max(0, hoursSpan);
            minutesSpan = Mathf.Max(0, minutesSpan);
            secondsSpan = Mathf.Max(0, secondsSpan);

            if (removeOld)
            {
                if (PlayerPrefs.HasKey(name))
                {
                    PlayerPrefs.DeleteKey(name);
                }
                if (PlayerPrefs.HasKey(lastTickSaveKey))
                {
                    PlayerPrefs.DeleteKey(lastTickSaveKey);
                }
                if (PlayerPrefs.HasKey(initTimeSaveKey))
                {
                    PlayerPrefs.DeleteKey(initTimeSaveKey);
                }
                if (PlayerPrefs.HasKey(startTickSaveKey))
                {
                    PlayerPrefs.DeleteKey(startTickSaveKey);
                }

                initTime = daySpan * 24.0 * 3600.0 + hoursSpan * 3600.0 + minutesSpan * 60.0 + secondsSpan;
                PlayerPrefs.SetString(initTimeSaveKey, initTime.ToString());
                PlayerPrefs.SetString(name, name);
                Debug.Log("-------------------------remove old " + name + "-------------------------------");
            }
            else // continue
            {
                if (PlayerPrefs.HasKey(name) && PlayerPrefs.HasKey(lastTickSaveKey) && PlayerPrefs.HasKey(initTimeSaveKey) && PlayerPrefs.HasKey(startTickSaveKey))
                {
                    startDT = DTFromSring(PlayerPrefs.GetString(startTickSaveKey));
                    lastDT = DTFromSring(PlayerPrefs.GetString(lastTickSaveKey));
                    initTime = daySpan * 24.0 * 3600.0 + hoursSpan * 3600.0 + minutesSpan * 60.0 + secondsSpan;
                    if (initTime == 0)
                        if (!double.TryParse(PlayerPrefs.GetString(initTimeSaveKey), out initTime))
                        {
                            Debug.Log("try parse error");
                            initTime = 0;
                        }
                    endDt = startDT.AddSeconds(initTime);
                    startTickCreated = true;
                    lastTickCreated = true;
                    Debug.Log("--------------------continue " +name+ " ------------------------");
                }
                else //if data lost
                {
                    initTime = daySpan * 24.0 * 3600.0 + hoursSpan * 3600.0 + minutesSpan * 60.0 + secondsSpan;
                    PlayerPrefs.SetString(initTimeSaveKey, initTime.ToString());
                    PlayerPrefs.SetString(name, name);
                    Debug.Log("--------------------old lost " + name + "------------------------");
                }
            }
            Debug.Log(initTime);
        }

        bool startTickCreated = false;
        bool lastTickCreated = false;

        /// <summary>
        /// Timer update.
        /// </summary>
        /// <param name="time"></param>
        public void Update()
        {
            if (cancel) return;
            currentDT = DateTime.Now;

            if (!startTickCreated)
            {
                startDT = currentDT;
                PlayerPrefs.SetString(startTickSaveKey, currentDT.ToString());
                startTickCreated = true;
                endDt = startDT.AddSeconds(initTime); // Debug.Log( "strtTick: "+startTick);
            }
            if (!lastTickCreated)
            {
                lastDT = currentDT;
                PlayerPrefs.SetString(lastTickSaveKey, currentDT.ToString());
                lastTickCreated = true;//  Debug.Log("lastTick: "+lastTick);
            }

            double dTime = (currentDT - startDT).TotalSeconds;
            double passedSeconds = dTime;
         
            if (dTime>=initTime && !cancel)
            {
                cancel = true;
                if (OnTimePassed != null) OnTimePassed();
                passedSeconds = initTime;
            }

            if ((currentDT-lastDT).TotalSeconds >= 1.0 || cancel)
            {
               // Debug.Log("dTime: " + dTime +" current: "+ currentDT.ToString() + " last: " + lastDT.ToString());
                lastDT = currentDT;
                PlayerPrefs.SetString(lastTickSaveKey, currentDT.ToString());

                if (OnTickPassedSeconds != null)
                {
                    OnTickPassedSeconds(passedSeconds);
                }
                if (OnTickRestSeconds != null)
                {
                    OnTickRestSeconds(initTime - passedSeconds);
                }
                if (OnTickPassedDaysHourMinSec != null)
                {
                    int d = 0;
                    int h = 0;
                    int m = 0;
                    float s = 0;
                    PassedTime(out d, out h, out m, out s);
                    OnTickPassedDaysHourMinSec(d, h, m, s);
                }
                if (OnTickRestDaysHourMinSec != null)
                {
                    int d = 0;
                    int h = 0;
                    int m = 0;
                    float s = 0;
                    RestTime(out d, out h, out m, out s);
                    OnTickRestDaysHourMinSec(d, h, m, s);
                }
            }
        }

        /// <summary>
        /// Restart new timer cycle
        /// </summary>
        public void Restart()
        {
            startTickCreated = false;
            lastTickCreated = false;
            cancel = false;
            Debug.Log("restart");
        }

        public static DateTime DTFromSring(string dtString)
        {
            if (string.IsNullOrEmpty(dtString)) return DateTime.Now;
            return Convert.ToDateTime(dtString);
        }

        private double GetTimeSpanSeconds(DateTime dtStart, DateTime dtEnd)
        {
            return (dtEnd - dtStart).TotalSeconds;
        }

        private double GetTimeSpanSeconds(string dtStart, string dtEnd)
        {
            return (DTFromSring(dtEnd) - DTFromSring(dtStart)).TotalSeconds;
        }
    }

}

