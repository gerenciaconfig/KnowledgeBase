using UnityEngine.UI;
using System;
using UnityEngine;

namespace Mkey
{
    public class MissionWindowController : PopUpsController
    {
        [SerializeField]
        private Text descriptionText;

        [Space(8)]
        [SerializeField]
        private GameObject TimeLevelGO;
        [SerializeField]
        private Text timeLimitText;

        [Space(8)]
        [SerializeField]
        private GameObject FishLevelGO;
        [SerializeField]
        private Text targetCountText;

        [Space(8)]
        [SerializeField]
        private GameObject AnchorLevelGO;
        [SerializeField]
        private Text anchorCountText;

        [Space(8)]
        [SerializeField]
        private GameObject LoopTopRowLevelGO;
        [SerializeField]
        private Text bubblesCountText;

        [Space(8)]
        [SerializeField]
        private Text levelNumberText;

        public void Play_Click()
        {
            CloseWindow();
        }

        public void Cancel_Click()
        {
            CloseWindow();
            SceneLoader.Instance.LoadScene(0);
        }

        public override void RefreshWindow()
        {
            LevelConstructSet lCS = GameBoard.LcSet;
            MissionConstruct lM = lCS.levelMission;
            LevelType lType = lM.GetLevelType();

            int time = lM.TimeConstrain;
            int moves = lM.MovesConstrain;
            int level = BubblesPlayer.CurrentLevel+1;
            WinController wC = GameBoard.Instance.WController;

            if (descriptionText)
            {
                descriptionText.text = lM.Description;
            }

            if (levelNumberText) levelNumberText.text = level.ToString();

            if(TimeLevelGO) TimeLevelGO.SetActive(lType== LevelType.TimeLevel);
            if (FishLevelGO) FishLevelGO.SetActive(lType == LevelType.FishLevel);
            if(AnchorLevelGO) AnchorLevelGO.SetActive(lType== LevelType.AnchorLevel);
            if(LoopTopRowLevelGO) LoopTopRowLevelGO.SetActive(lType== LevelType.LoopTopRowLevel);

            switch (lType)
            {
                case LevelType.LoopTopRowLevel:
                    if (bubblesCountText) bubblesCountText.text = "x " + wC.TopRowBubblesCountToCollect;
                    break;
                case LevelType.TimeLevel:
                    if (timeLimitText) timeLimitText.text = "x " + time;
                    break;
                case LevelType.AnchorLevel:
                    if (anchorCountText) anchorCountText.text = "x " + 1;
                    break;
                case LevelType.FishLevel:
                    int fishCount = 0;
                    int fishCountCollected = 0;
                    wC.GetCurrTarget(out fishCountCollected, out fishCount);
                    if (targetCountText) targetCountText.text = "x " + fishCount;
                    break;
            }
            base.RefreshWindow();
        }
    }
}