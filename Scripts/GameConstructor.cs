using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mkey
{
    public class GameConstructor : MonoBehaviour
    {
#if UNITY_EDITOR

        private List<RectTransform> openedPanels;

        [SerializeField]
        private Text editModeText;

        #region selected brush
        private BaseObjectData mainBrush;
        [Space(8, order = 0)]
        [Header("Grid Brushes", order = 1)]
        [SerializeField]
        private Image mainBrushImage;
        [SerializeField]
        private Image selectedMainBrushImage;

        private BaseObjectData overBrush;
        [SerializeField]
        private Image overBrushImage;
        [SerializeField]
        private Image selectedOverBrushImage;
        [SerializeField]
        private PanelContainerController MainBrushContainer;
        [SerializeField]
        private PanelContainerController OverBrushContainer;

        #endregion selected brush

        #region gift
        //[Space(8, order = 0)]
        //[Header("Gift", order = 1)]
        //[SerializeField]
        //private PanelContainerController GiftPanelContainer;
        [SerializeField]
        private IncDecInputPanel IncDecPanelPrefab;
        #endregion gift

        #region mission
        [Space(8, order = 0)]
        [Header("Mission", order = 1)]
        [SerializeField]
        private PanelContainerController MissionPanelContainer;
        [SerializeField]
        private IncDecInputPanel InputTextPanelMissionPrefab;
        [SerializeField]
        private IncDecInputPanel IncDecTogglePanelMissionPrefab;
        [SerializeField]
        private IncDecInputPanel TogglePanelMissionPrefab;
        #endregion mission

        #region grid construct
        [Space(8, order = 0)]
        [Header("Grid", order = 1)]
        [SerializeField]
        private PanelContainerController GridPanelContainer;
        [SerializeField]
        private IncDecInputPanel IncDecGridPrefab;
        #endregion grid construct

        #region game construct
        private List<GameObjectsSet> MatchSets;
        [Space(8, order = 0)]
        [Header("Game construct", order = 0)]
        [SerializeField]
        private GameObject levelButtonPrefab;
        [SerializeField]
        private GameObject smallButtonPrefab;
        [SerializeField]
        private GameObject constructPanel;
        [SerializeField]
        private Button openConstructButton;
        [SerializeField]
        private ScrollRect LevelButtonsContainer;
        #endregion game construct

        private GridCell selected;

        //resource folders
        private string gameObjectSetSubFolder = "GameObjectSets";
        private string levelConstructSetSubFolder = "LevelConstructSets";
        private string gameConstructSetSubFolder = "GameConstructSets";

        private int minVertSize = 5;
        private int minHorSize = 5;
        private int maxHorSize = 15;

        public void InitStart(GameConstructSet gcSet)
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                Debug.Log("Game Contructor init start");
                MatchSets = LoadGameObjectSetAssets();
                LoadLevelConstructAssets();
                LoadGameConstructAsset();
                CreateLevelButtons();
                ShowConstructMenu(true);

                LevelConstructSet lcs = GameBoard.LcSet;
                mainBrush = lcs.Matchset.Empty;
                mainBrushImage.sprite = mainBrush.ObjectImage;
                SelectMainBrush();

                overBrush = lcs.Matchset.Empty;
                overBrushImage.sprite = overBrush.ObjectImage;
                if (editModeText) editModeText.text = "EDIT MODE" + '\n' + "Level " + (BubblesPlayer.CurrentLevel + 1);
                GameBoard.Instance.gcSet.Clean();
                GameBoard.LcSet.Clean();
            }

        }

        private void ShowLevelData()
        {
            GameBoard.Instance.gcSet.Clean();
            GameBoard.LcSet.Clean();
            Debug.Log("Show level data: " + BubblesPlayer.CurrentLevel);
            GameBoard.Instance.CreateGameBoard(false);
            LevelButtonsRefresh();
            if (editModeText) editModeText.text = "EDIT MODE" + '\n' + "Level " + (BubblesPlayer.CurrentLevel+1);
        }

        #region board move
        public void MoveBoard(int steps)
        {
            bool up = steps > 0;
            int aSteps = Mathf.Abs(steps);
            TweenSeq tS = new TweenSeq();
            for (int i = 0; i < aSteps; i++)
            {
                tS.Add((callBack) => 
                {
                    GameBoard.Instance.grid.MoveStep(up,0.1f, callBack);
                });
            }
            tS.Start();
        }
        #endregion board move

        #region construct menus
        bool openedConstr = false;

        public void OpenConstructPanel()
        {
            SetConstructControlActivity(false);
            RectTransform rt = constructPanel.GetComponent<RectTransform>();//Debug.Log(rt.offsetMin + " : " + rt.offsetMax);
            float startX = (!openedConstr) ? 0 : 1f;
            float endX = (!openedConstr) ? 1f : 0;

            SimpleTween.Value(constructPanel, startX, endX, 0.2f).SetEase(EaseAnim.EaseInCubic).
                               SetOnUpdate((float val) =>
                               {
                                   rt.transform.localScale = new Vector3(val, 1, 1);
                               // rt.offsetMax = new Vector2(val, rt.offsetMax.y);
                           }).AddCompleteCallBack(() =>
                           {
                               SetConstructControlActivity(true);
                               openedConstr = !openedConstr;
                               LevelButtonsRefresh();
                           });
        }

        private void SetConstructControlActivity(bool activity)
        {
            Button[] buttons = constructPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        private void ShowConstructMenu(bool show)
        {
            constructPanel.SetActive(show);
            openConstructButton.gameObject.SetActive(show);
        }

        public void CreateLevelButtons()
        {
            GameObject parent = LevelButtonsContainer.content.gameObject;
            Button[] existButtons = parent.GetComponentsInChildren<Button>();
            for (int i = 0; i < existButtons.Length; i++)
            {
                DestroyImmediate(existButtons[i].gameObject);
            }

            for (int i = 0; i < GameBoard.Instance.gcSet.levelSets.Count; i++)
            {
                GameObject buttonGO = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity);
                buttonGO.transform.SetParent(parent.transform);
                buttonGO.transform.localScale = Vector3.one;
                Button b = buttonGO.GetComponent<Button>();
                b.onClick.RemoveAllListeners();
                int level = i + 1;
                b.onClick.AddListener(() =>
                {
                    BubblesPlayer.CurrentLevel = level-1;
                    CloseOpenedPanels();
                    ShowLevelData();
                    GameBoard.Instance.grid.MoveToVisible(null);
                });
                buttonGO.GetComponentInChildren<Text>().text = "" + level.ToString();
            }
        }

        public void RemoveLevel()
        {
            Debug.Log("Click on Button <Remove level...> ");
            if (GameBoard.Instance.gcSet.LevelCount < 2)
            {
                Debug.Log("Can't remove the last level> ");
                return;
            }
            GameBoard.Instance.gcSet.RemoveLevel(BubblesPlayer.CurrentLevel);
            BubblesPlayer.CurrentLevel = (BubblesPlayer.CurrentLevel < GameBoard.Instance.gcSet.LevelCount-1) ? BubblesPlayer.CurrentLevel : BubblesPlayer.CurrentLevel - 1;
            CreateLevelButtons();
            ShowLevelData();
            GameBoard.Instance.grid.MoveToVisible(null);
        }

        public void InsertBefore()
        {
            Debug.Log("Click on Button <Insert level before...> ");
            LevelConstructSet lcs = ScriptableObjectUtility.CreateResourceAsset<LevelConstructSet>(levelConstructSetSubFolder, "", " " + 1.ToString());
            lcs.Matchset = MatchSets[0];
            GameBoard.Instance.gcSet.InsertBeforeLevel(BubblesPlayer.CurrentLevel, lcs);
            CreateLevelButtons();
            ShowLevelData();
            GameBoard.Instance.grid.MoveToVisible(null);
        }

        public void InsertAfter()
        {
            Debug.Log("Click on Button <Insert level after...> ");
            LevelConstructSet lcs = ScriptableObjectUtility.CreateResourceAsset<LevelConstructSet>(levelConstructSetSubFolder, "", " " + 1.ToString());
            lcs.Matchset = MatchSets[0];
            GameBoard.Instance.gcSet.InsertAfterLevel(BubblesPlayer.CurrentLevel, lcs);
            BubblesPlayer.CurrentLevel += 1;
            CreateLevelButtons();
            ShowLevelData();
            GameBoard.Instance.grid.MoveToVisible(null);
        }

        void LevelButtonsRefresh()
        {
            Button[] levelButtons = LevelButtonsContainer.content.gameObject.GetComponentsInChildren<Button>();
            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (i == BubblesPlayer.CurrentLevel)
                {
                    SelectButton(levelButtons[i], true);
                }
                else
                {
                    SelectButton(levelButtons[i], false);
                }
            }
        }

        void SelectButton(Button b, bool select)
        {
            if (select)
            {
                b.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                b.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }

        #endregion construct menus

        #region grid settings
        private void ShowLevelSettingsMenu(bool show)
        {
            constructPanel.SetActive(show);
            openConstructButton.gameObject.SetActive(show);
        }

        public void OpenGridSettingsPanel_Click()
        {
            Debug.Log("open grid settings click");
            BubbleGrid grid = GameBoard.Instance.grid;

            ScrollPanelController sRC = GridPanelContainer.ScrollPanel;
            if (sRC) // 
            {
                if (sRC) sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();
                //instantiate ScrollRectController
                sRC = GridPanelContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Grid panel";

                LevelConstructSet lcSet = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
                MissionConstruct levelMission = lcSet.levelMission;

                //create  vert size block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "VertSize", grid.Rows.Count.ToString(),
                    () => { IncVertSize(); },
                    () => { DecVertSize(); },
                    (val) => {  },
                    () => { return grid.Rows.Count.ToString(); },
                    null);

                //create hor size block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "HorSize", lcSet.HorSize.ToString(),
                    () => { IncHorSize(); },
                    () => { DecHorSize(); },
                    (val) => { },
                    () => { return lcSet.HorSize.ToString(); },
                    null);

                //create background block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "BackGrounds", lcSet.backGroundNumber.ToString(),
                    () => { IncBackGround(); },
                    () => { DecBackGround(); },
                    (val) => { },
                    () => { return lcSet.backGroundNumber.ToString(); },
                    null);

                //create game object set block
                GameObjectsSet ms = lcSet.Matchset;
                int indexMS = MatchSets.IndexOf(ms);
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "GameSets", ms.name.ToString(),
                    () => { if (indexMS < 0 || MatchSets.Count < 1) return; indexMS = (int) Mathf.Repeat(++indexMS, MatchSets.Count); lcSet.Matchset = MatchSets[indexMS]; ShowLevelData(); },
                    () => { if (indexMS < 0 || MatchSets.Count < 1) return; indexMS = (int) Mathf.Repeat(--indexMS, MatchSets.Count); lcSet.Matchset = MatchSets[indexMS]; ShowLevelData(); },
                    (val) => { },
                    () => { return lcSet.Matchset.name.ToString(); },
                    null);


                sRC.OpenScrollPanel(null);
            }
        }

        public void IncVertSize()
        {
            Debug.Log("Click on Button <VerticalSize...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int vertSize = lcs.VertSize;
            vertSize += 1;
            lcs.VertSize = vertSize;
            ShowLevelData();
        }

        public void DecVertSize()
        {
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int vertSize = lcs.vertSize;
            vertSize = (vertSize > minVertSize) ? --vertSize : minVertSize;
            lcs.vertSize = vertSize;
            ShowLevelData();
        }

        public void IncHorSize()
        {
            Debug.Log("Click on Button <HorizontalSize...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int horSize = lcs.HorSize;
            horSize = (horSize < maxHorSize) ? ++horSize : maxHorSize;
            lcs.HorSize = horSize;
            ShowLevelData();
        }

        public void DecHorSize()
        {
            Debug.Log("Click on Button <HorizontalSize...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int horSize = lcs.HorSize;
            horSize = (horSize > minHorSize) ? --horSize : minHorSize;
            lcs.HorSize = horSize;
            ShowLevelData();
        }

        public void IncDistX()
        {
            Debug.Log("Click on Button <DistanceX...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            float dist = lcs.DistX;
            dist += 0.05f;
            lcs.DistX = (dist > 1f) ? 1f : dist;
            ShowLevelData();
        }

        public void DecDistX()
        {
            Debug.Log("Click on Button <DistanceX...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            float dist = lcs.DistX;
            dist -= 0.05f;
            lcs.DistX = (dist > 0f) ? dist : 0f;
            ShowLevelData();
        }

        public void IncDistY()
        {
            Debug.Log("Click on Button <DistanceY...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            float dist = lcs.DistY;
            dist += 0.05f;
            lcs.DistY = (dist > 1f) ? 1f : dist;
            ShowLevelData();
        }

        public void DecDistY()
        {
            Debug.Log("Click on Button <DistanceY...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            float dist = lcs.DistY;
            dist -= 0.05f;
            lcs.DistY = (dist > 0f) ? dist : 0f;
            ShowLevelData();
        }

        public void DecScale()
        {
            Debug.Log("Click on Button <Scale...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int scale = Mathf.RoundToInt(lcs.Scale * 100f);
            scale -= 5;
            lcs.Scale = (scale > 0) ? scale/100f : 0f;
            ShowLevelData();
        }

        public void IncScale()
        {
            Debug.Log("Click on Button <Scale...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            int scale = Mathf.RoundToInt(lcs.Scale *100f);
            scale += 5;
            lcs.Scale = scale/100f;
            ShowLevelData();
        }

        public void IncBackGround()
        {
            Debug.Log("Click on Button <BackGround...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            lcs.IncBackGround();
            ShowLevelData();
        }

        public void DecBackGround()
        {
            Debug.Log("Click on Button <BackGround...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
            lcs.DecBackGround();
            ShowLevelData();

        }
        #endregion grid settings

        #region shoot bubbles
        public void OpenShootBubblestPanel_Click()
        {
            Debug.Log("shoot bubble click");
            BubbleGrid grid = GameBoard.Instance.grid;
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);

            //ScrollPanelController sRC = ShootBubbleContainer.ScrollPanel;
            //if (sRC) // 
            //{
            //    sRC.CloseScrollPanel(true, null);
            //}
            //else
            //{
            //    CloseOpenedPanels();

                //List<BaseObjectData> mData = new List<BaseObjectData>();
                //// mData.Add(lcs.Matchset.Empty);
                //if (lcs.Matchset.MainObjects != null)
                //    foreach (var item in lcs.Matchset.MainObjects)
                //    {
                //        if (item.canUseAsShootBubbles) mData.Add(item);
                //    }

                ////create shoot bubble buttons
                //for (int i = 0; i < mData.Count; i++)
                //{
                //    BaseObjectData mD = mData[i];
                //    Button b = CreateButton(smallButtonPrefab, parent.transform, mD.ObjectImage, () =>
                //       {
                //           Debug.Log("Click on Button <" + mD.Name + "...> ");
                //           lcs.AddShootBubble(mD.ID);
                //           ShowLevelData();
                //       });

                //    if (lcs.shootBubbles.Contains(mD.ID))
                //    {
                //        SelectButton(b);
                //    }

                //}
            //    sRC.OpenScrollPanel(null);
            //}
        }
        #endregion shoot bubbles

        #region grid brushes
        public void Cell_Click(GridCell cell)
        {
            if (cell.GRow.IsSeviceRow)
            {
                Debug.Log("Click on cell <" + cell.ToString() + "...> - Is Service cell. Don't use it.");
                return;
            }

            Debug.Log("Click on cell <" + cell.ToString() + "...> ");
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);

            if (selectedMainBrushImage.enabled)
            {
                Debug.Log("main brush enabled");
                if (cell.Mainobject && cell.Mainobject.ID == mainBrush.ID) return;
                else
                {
                    if (!cell.Mainobject && mainBrush.ID == 0) return;
                    lcs.AddFeaturedCell(new CellData(mainBrush.ID, cell.Row, cell.Column));
                   if(lcs)  Debug.Log("add feat cell, count " + lcs.featuredCells.Count);
                }
            }
            else if (selectedOverBrushImage.enabled)
            {
                Debug.Log("over brush enabled");
                if (cell.Overlay && cell.Overlay.ID == overBrush.ID) return;
                else
                {
                    if (!cell.Mainobject) return;
                    Debug.Log("add overlay");
                    lcs.AddOverlay(new CellData(overBrush.ID, cell.Row, cell.Column));
                }
            }

            CloseOpenedPanels();
            ShowLevelData();
        }

        public void OpenMainBrushPanel_Click()
        {
            Debug.Log("open main brush click");
            BubbleGrid grid = GameBoard.Instance.grid;
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);

            ScrollPanelController sRC = MainBrushContainer.ScrollPanel;
            if (sRC) // 
            {
                sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();
                //instantiate ScrollRectController
                sRC = MainBrushContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Main brush panel";

                List<BaseObjectData> mData = new List<BaseObjectData>();
                mData.Add(lcs.Matchset.Empty);
                if (lcs.Matchset.MainObjects != null)
                    foreach (var item in lcs.Matchset.MainObjects)
                    {
                        mData.Add(item);
                    }

                //create main bubbles brushes
                for (int i = 0; i < mData.Count; i++)
                {
                    BaseObjectData mD = mData[i];
                    CreateButton(smallButtonPrefab, sRC.scrollContent, mD.ObjectImage, () =>
                    {
                        Debug.Log("Click on Button <" + mD.ID + "...> ");
                        mainBrush = (!GameObjectsSet.IsEmptyObject(mD.ID)) ? lcs.Matchset.GetMainObject(mD.ID) : lcs.Matchset.Empty;
                        mainBrushImage.sprite = mainBrush.ObjectImage;
                        SelectMainBrush();
                    });
                }
                sRC.OpenScrollPanel(null);
            }
        }

        public void OpenOverBrushPanel_Click()
        {
            Debug.Log("open over brush click");
            BubbleGrid grid = GameBoard.Instance.grid;
            LevelConstructSet lcs = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);

            ScrollPanelController sRC = OverBrushContainer.ScrollPanel;
            if (sRC) // 
            {
                sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();
                //instantiate ScrollRectController
                sRC = OverBrushContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Over brush panel";

                List<BaseObjectData> mData = new List<BaseObjectData>();
                mData.Add(lcs.Matchset.Empty);
                if (lcs.Matchset.OverlayObjects != null) mData.AddRange(lcs.Matchset.OverlayObjects.GetBaseList());

                //create over brushes
                for (int i = 0; i < mData.Count; i++)
                {
                    BaseObjectData mD = mData[i];
                    CreateButton(smallButtonPrefab, sRC.scrollContent, mD.ObjectImage, () =>
                    {
                        Debug.Log("Click on Button <" + mD.ID + "...> ");
                        overBrush = (!GameObjectsSet.IsEmptyObject(mD.ID)) ? lcs.Matchset.GetOverlayObject(mD.ID) : lcs.Matchset.Empty;
                        overBrushImage.sprite = overBrush.ObjectImage;
                        SelectOverBrush();
                    });
                }
                sRC.OpenScrollPanel(null);
            }
        }

        private void CloseOpenedPanels()
        {
            ScrollPanelController[] sRCs = GetComponentsInChildren<ScrollPanelController>();
            foreach (var item in sRCs)
            {
                item.CloseScrollPanel(true, null);
            }

        }

        private void SetSpriteControlActivity(RectTransform panel, bool activity)
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        public void SelectMainBrush()
        {
            DeselectAllBrushes();
            selectedMainBrushImage.enabled = true;
        }

        public void SelectOverBrush()
        {
            DeselectAllBrushes();
            selectedOverBrushImage.enabled = true;
        }

        private void DeselectAllBrushes()
        {
            selectedMainBrushImage.enabled = false;
            selectedOverBrushImage.enabled = false;
        }

        #endregion  brushes

        //#region gift
        //public void OpenGiftPanel_Click()
        //{
        //    Debug.Log("open click");
        //    ScrollPanelController sRC = GiftPanelContainer.ScrollPanel;
        //    if (sRC) // 
        //    {
        //        if (sRC) sRC.CloseScrollPanel(true, null);
        //    }
        //    else
        //    {
        //        //instantiate ScrollRectController
        //        sRC = GiftPanelContainer.InstantiateScrollPanel();
        //        sRC.textCaption.text = "Gift panel";

        //        LevelConstructSet lcSet = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
        //        GameConstructSet gcSet = GameBoard.Instance.gcSet;
        //        GiftConstruct levelGift = gcSet.gift;

        //        //create life gift
        //        IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Life", levelGift.Life.ToString(), 
        //            () => { levelGift.AddLifes(1); }, 
        //            () => { levelGift.AddLifes(-1); }, 
        //            (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelGift.SetLifesCount(res); }},
        //            () => { return levelGift.Life.ToString(); },
        //            null);

        //        //create coins gift
        //        IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Coins", levelGift.Coins.ToString(),
        //        () => { levelGift.AddCoins(1); },
        //        () => { levelGift.AddCoins(-1); },
        //        (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelGift.SetCoinsCount(res); } },
        //        () => { return levelGift.Coins.ToString(); },
        //        null);

        //        //create booster gift
        //        GameObjectsSet goSet = lcSet.Matchset;
        //        IList<BoosterObjectData> bDataL = goSet.BoosterObjects;
        //        foreach (var item in bDataL)
        //        {
        //            int id = item.ID;
        //            IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Booster", levelGift.GetBoosterCount(id).ToString(),
        //            () => { levelGift.AddBooster(id); },
        //            () => { levelGift.RemoveBooster(id); },
        //            (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelGift.SetBoosterCount(res, id); } },
        //            () => { return levelGift.GetBoosterCount(id).ToString(); },
        //            item.GuiImage);
        //        }

        //        sRC.OpenScrollPanel(null);
        //    }
        //}
        //#endregion gift

        #region mission
        public void OpenMissionPanel_Click()
        {
            Debug.Log("open mission click");
            BubbleGrid grid = GameBoard.Instance.grid;

            ScrollPanelController sRC = MissionPanelContainer.ScrollPanel;
            if (sRC) // 
            {
               sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();
                //instantiate ScrollRectController
                sRC = MissionPanelContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Mission panel";

                LevelConstructSet lcSet = GameBoard.Instance.gcSet.GetLevelConstructSet(BubblesPlayer.CurrentLevel);
                MissionConstruct levelMission = lcSet.levelMission;

                //create mission moves constrain
                IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Moves", levelMission.MovesConstrain.ToString(),
                    () => { levelMission.AddMoves(1); },
                    () => { levelMission.AddMoves(-1); },
                    (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetMovesCount(res); } },
                    () => { return levelMission.MovesConstrain.ToString(); },
                    null);

                //create time constrain
                IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Time", levelMission.TimeConstrain.ToString(),
                () => { levelMission.AddTime(1); },
                () => { levelMission.AddTime(-1); },
                (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetTime(res); } },
                () => { return levelMission.TimeConstrain.ToString(); },
                null);

                //description input field
                IncDecInputPanel.Create(sRC.scrollContent, InputTextPanelMissionPrefab, "Description", levelMission.Description,
                null,
                null,
                (val) => { levelMission.SetDescription(val); },
                () => { return levelMission.Description; },
                null);

                // create clean top row check box condition
                IncDecInputPanel.Create(sRC.scrollContent, TogglePanelMissionPrefab, "Clean top row", null,
                  levelMission.LoopTopRow,
                  null,
                  null,
                  null,
                  (val) => { levelMission.SetLoopTopRow(val);},
                  null,
                  null);

                // create raise anchor check box condition
                IncDecInputPanel.Create(sRC.scrollContent, TogglePanelMissionPrefab, "Raise anchor", null,
                  levelMission.RaiseAnchor,
                  null,
                  null,
                  null,
                  (val) => { levelMission.SetRaiseAnchor(val); },
                  null,
                  null);


                //create object targets
                GameObjectsSet goSet = lcSet.Matchset;
                IList<BaseObjectData> tDataL = goSet.TargetObjects;
                foreach (var item in tDataL)
                {
                    if (item!=null)
                    {
                        Debug.Log("target ID: " + item.ID);
                        int id = item.ID;
                        IncDecInputPanel.Create(sRC.scrollContent, IncDecTogglePanelMissionPrefab, "Target", levelMission.GetTargetCount(id).ToString(),
                        levelMission.GetTargetCount(id) == 10000,
                        () => { levelMission.AddTarget(id, 1); },
                        () => { levelMission.RemoveTarget(id, 1); },
                        (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetTargetCount(id, res); } },
                        (val) => { if (val) { levelMission.SetTargetCount(id, 10000); } else { levelMission.SetTargetCount(id, 0); } },
                        () => { return  levelMission.GetTargetCount(id).ToString(); }, // grid.GetObjectsCountByID(id).ToString()); },
                        item.GuiImage);
                    }
                }

                sRC.OpenScrollPanel(null);
            }
        }
        #endregion mission

        #region load assets

        private void LoadGameConstructAsset()
        {
            if (GameBoard.Instance.gcSet != null)
            {
                return;
            }
            GameConstructSet[] os = LoadResourceAssets<GameConstructSet>(gameConstructSetSubFolder);
            if (os.Length > 0)
            {
                GameBoard.Instance.gcSet = os[0];
            }
            else
            {
                GameBoard.Instance.gcSet = ScriptableObjectUtility.CreateAsset<GameConstructSet>(gameConstructSetSubFolder, "", " " + 1.ToString());
            }
        }

        private List<GameObjectsSet> LoadGameObjectSetAssets()
        {
            List<GameObjectsSet> GameSets = new List<GameObjectsSet>(LoadResourceAssets<GameObjectsSet>(gameObjectSetSubFolder));
            if (GameSets == null || GameSets.Count == 0)
            {
                GameSets = new List<GameObjectsSet>();
                GameSets.Add(ScriptableObjectUtility.CreateResourceAsset<GameObjectsSet>(gameObjectSetSubFolder, "", " " + 1.ToString()));
                Debug.Log("New GameObjectSet created: " + GameSets[0].ToString());
            }
            return GameSets;
        }

        private List<LevelConstructSet> LoadLevelConstructAssets()
        {
            List<LevelConstructSet> LevelConstructSets = new List<LevelConstructSet>(LoadResourceAssets<LevelConstructSet>(levelConstructSetSubFolder));
            if (LevelConstructSets == null || LevelConstructSets.Count == 0)
            {
                LevelConstructSets = new List<LevelConstructSet>();
                LevelConstructSets.Add(ScriptableObjectUtility.CreateResourceAsset<LevelConstructSet>(levelConstructSetSubFolder, "", " " + 1.ToString()));
                Debug.Log("New LevelConstructSet created: " + LevelConstructSets[0].ToString());
            }
            // all empty level MatchSets - set to default
            LevelConstructSets.ForEach((l) => { if (!l.mSet) l.mSet = MatchSets[0]; });
            return LevelConstructSets;
        }

        private T[] LoadResourceAssets<T>(string subFolder) where T : BaseScriptable
        {
            T[] t = Resources.LoadAll<T>(subFolder);
            if (t != null && t.Length > 0)
            {
                string s = "";
                foreach (var m in t)
                {
                    s += m.ToString() + "; ";
                }
                Debug.Log("Scriptable assets <" + typeof(T).ToString() + "> loaded, count: " + t.Length + "; sets : " + s);
            }

            else
            {
                Debug.Log("Scriptable assets <" + typeof(T).ToString() + "> in " + subFolder + " folder"  + " not found!!!");
            }
            return t;
        }

#endregion load assets

        #region utils
        private void DestroyGoInChildrenWithComponent<T>(Transform parent) where T : Component
        {
            T[] existComp = parent.GetComponentsInChildren<T>();
            for (int i = 0; i < existComp.Length; i++)
            {
                DestroyImmediate(existComp[i].gameObject);
            }
        }

        private Button CreateButton(GameObject prefab, Transform parent, Sprite sprite, System.Action listener)
        {
            GameObject buttonGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            buttonGO.transform.SetParent(parent);
            buttonGO.transform.localScale = new Vector3(1, 1, 1);
            Button b = buttonGO.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.GetComponent<Image>().sprite = sprite;
            if(listener!=null)  b.onClick.AddListener(() =>
            {
                listener();
            });

            return b;
        }

        private void SelectButton(Button b)
        {
            Text t = b.GetComponentInChildren<Text>();
            if (!t) return;
            t.enabled = true;
            t.gameObject.SetActive(true);
            t.text = "selected";
            t.color = Color.black;
        }

        private void DeselectButton(Button b)
        {
            Text t = b.GetComponentInChildren<Text>();
            if (!t) return;
            t.enabled = true;
            t.gameObject.SetActive(true);
            t.text = "";
        }

        #endregion utils
#endif
    }

#if UNITY_EDITOR

    public static class ScriptableObjectUtility //http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(string subFolder, string namePrefix, string nameSuffics) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            Debug.Log(path);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Resources/" + subFolder + "/" + namePrefix + typeof(T).ToString() + nameSuffics + ".asset");
            Debug.Log(assetPathAndName);
            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files in Resource/Subfolder .
        /// </summary>
        public static T CreateResourceAsset<T>(string subFolder, string namePrefix, string nameSuffics) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string path = "Assets/Bubbles/Resources/";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + subFolder + "/" + namePrefix + typeof(T).ToString() + nameSuffics + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files in Resource/Subfolder .
        /// </summary>
        public static void DeleteResourceAsset(UnityEngine.Object o)
        {
            string path = AssetDatabase.GetAssetPath(o);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

    }
#endif

}