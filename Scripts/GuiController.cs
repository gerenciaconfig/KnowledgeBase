using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mkey
{
    public class GuiController : MonoBehaviour
    {
        public Canvas popup;
        [Header("Popup prefabs", order = 1)]
        public GameObject MessageWindowPrefab;
        public GameObject SettingsWindowPrefab;
        public GameObject VictoryWindowPrefab;
        public GameObject LossWindowPrefab;
        public GameObject AboutPrefab;
        public GameObject TimeLeftPrefab;
        public GameObject CoinsShopWindowPrefab;
        public GameObject LifeShopWindowPrefab;
        public GameObject BoosterMulticolorShopWindowPrefab;
        public GameObject BoosterEyeBallShopWindowPrefab;
        public GameObject BoosterFireBallShopWindowPrefab;
        public GameObject MissionPrefab;

        [Space(8, order = 0)]
        [Header("Refresh handlers", order = 1)]
        public UnityEvent GUIrefreshers;

        [Header("Callbacks")]
        public UnityEvent onVictory;
        public UnityEvent onLoss;

        private List<GameObject> PopupsList;
        public static GuiController Instance;

        public Button modeButton;

        void Awake()
        {
            Application.targetFrameRate = 40;
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            PopupsList = new List<GameObject>();
#if UNITY_EDITOR
            if (modeButton)
            {
                modeButton.gameObject.SetActive(true);
                modeButton.GetComponentInChildren<Text>().text =(GameBoard.gMode == GameMode.Edit)? "GoTo" + System.Environment.NewLine + "PLAY": "GoTo" + System.Environment.NewLine + "EDIT";
                modeButton.onClick.AddListener(() =>
                {
                    if(GameBoard.gMode == GameMode.Edit)
                    {
                        GameBoard.gMode = GameMode.Play;
                        modeButton.GetComponentInChildren<Text>().text = "GoTo" + System.Environment.NewLine + "EDIT";
                    }
                    else
                    {
                        GameBoard.gMode = GameMode.Edit;
                        modeButton.GetComponentInChildren<Text>().text = "GoTo" + System.Environment.NewLine + "PLAY";
                    }
                    SceneLoader.Instance.ReLoadCurrentScene();
                });
            }
#else
            modeButton.gameObject.SetActive(false); 
#endif
        }

        private void PopUpOpenH(GameObject gO)
        {
            if (PopupsList.IndexOf(gO) == -1)
            {
                PopupsList.Add(gO);
            }
        }

        private void PopUpCloseH(GameObject gO)
        {
            if (PopupsList.IndexOf(gO) != -1)
            {
                PopupsList.Remove(gO);
                Destroy(gO);
            }
        }

        internal bool HasNoPopUp
        {
            get { return PopupsList.Count > 0; }
        }

        internal void RefreshGui()
        {
            GUIrefreshers.Invoke();
        }

        private PopUpsController ShowPopup(GameObject popup_prefab, Transform parent, Action closeCallBack)
        {
            return ShowPopup(popup_prefab,  parent, null, closeCallBack);
        }

        private PopUpsController ShowPopup(GameObject popup_prefab,  Transform parent, Action openCallBack, Action closeCallBack)
        {
            if (!popup_prefab) return null;
            PopUpsController pUp = CreateWindow(popup_prefab, parent);
            if (pUp)
            {
                pUp.PopUpInit(
                    (g) =>
                    {
                        PopUpOpenH(g); if (openCallBack != null) openCallBack();
                    }, (g) =>
                    {
                        PopUpCloseH(g);
                        if (closeCallBack != null) closeCallBack();
                    });
                pUp.ShowWindow();
            }
            return pUp;
        }

        private PopUpsController ShowPopup(GameObject popup_prefab, Transform parent, Vector3 position, Action openCallBack, Action closeCallBack)
        {
            if (!popup_prefab) return null;
            PopUpsController pUp = CreateWindow(popup_prefab, parent, position);
            if (pUp)
            {
                pUp.PopUpInit(
                    (g) =>
                    {
                        PopUpOpenH(g); if (openCallBack != null) openCallBack();
                    }, (g) =>
                    {
                        PopUpCloseH(g);
                        if (closeCallBack != null) closeCallBack();
                    });
                pUp.ShowWindow();
            }
            return pUp;
        }

        private PopUpsController CreateWindow(GameObject prefab, Transform parent)
        {
            GameObject gP = (GameObject)Instantiate(prefab.gameObject, parent);
            RectTransform mainRT = gP.GetComponent<RectTransform>();
            // rtP.localScale = parent.lossyScale;
            mainRT.SetParent(parent);
            Vector2 sD;
            WindowOpions winOptions = gP.GetComponent<GuiFader_v2>().winOptions;
            Vector3[] vC = new Vector3[4];
            mainRT.GetWorldCorners(vC);

            RectTransform rt = gP.GetComponent<GuiFader_v2>().guiPanel;
            Vector3[] vC1 = new Vector3[4];
            rt.GetWorldCorners(vC1);
            float height = (vC1[2] - vC1[0]).y;
            float width = (vC1[2] - vC1[0]).x;

            switch (winOptions.instantiatePosition)
            {
                case Position.LeftMiddleOut:
                    rt.position = new Vector3(vC[0].x - width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.RightMiddleOut:
                    rt.position = new Vector3(vC[2].x + width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.MiddleBottomOut:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[0].y - height / 2f, rt.position.z);
                    break;
                case Position.MiddleTopOut:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[2].y + height / 2f, rt.position.z);
                    break;
                case Position.LeftMiddleIn:
                    rt.position = new Vector3(vC[0].x + width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.RightMiddleIn:
                    rt.position = new Vector3(vC[2].x - width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.MiddleBottomIn:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[0].y + height / 2f, rt.position.z);
                    break;
                case Position.MiddleTopIn:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[2].y - height / 2f, rt.position.z);
                    break;
                case Position.CustomPosition:
                    rt.position = winOptions.position;
                    break;
                case Position.AsIs:
                    break;
                case Position.Center:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
            }
            PopUpsController pUp = gP.GetComponent<PopUpsController>();
            if (pUp)
            {
                pUp.SetControlActivity(false);
            }
            return pUp;
        }

        private PopUpsController CreateWindow(GameObject prefab, Transform parent, Vector3 position)
        {
            GameObject gP = (GameObject)Instantiate(prefab.gameObject, parent);
            RectTransform mainRT = gP.GetComponent<RectTransform>();
           // rtP.localScale = parent.lossyScale;
            mainRT.SetParent(parent);
            Vector2 sD;
            WindowOpions winOptions = gP.GetComponent<GuiFader_v2>().winOptions;

            Vector3[] vC = new Vector3[4];
            mainRT.GetWorldCorners(vC);

            RectTransform rt = gP.GetComponent<GuiFader_v2>().guiPanel;
            Vector3[] vC1 = new Vector3[4];
            rt.GetWorldCorners(vC1);
            float height = (vC1[2]-vC1[0]).y;
            float width = (vC1[2] - vC1[0]).x;
            if (winOptions == null) winOptions = new WindowOpions();
            winOptions.position =  position;

            switch (winOptions.instantiatePosition)
            {
                case Position.LeftMiddleOut:
                    rt.position = new Vector3(vC[0].x - width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.RightMiddleOut:
                    rt.position = new Vector3(vC[2].x + width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.MiddleBottomOut:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[0].y - height / 2f, rt.position.z);
                    break;
                case Position.MiddleTopOut:
                    rt.position = new Vector3((vC[0].x + vC[2].x)/2f,  vC[2].y + height / 2f, rt.position.z);
                    break;
                case Position.LeftMiddleIn:
                    rt.position = new Vector3(vC[0].x + width / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
                case Position.RightMiddleIn:
                    rt.position = new Vector3(vC[2].x - width / 2f, (vC[0].y + vC[2].y)/ 2f, rt.position.z);
                    break;
                case Position.MiddleBottomIn:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[0].y + height / 2f, rt.position.z);
                    break;
                case Position.MiddleTopIn:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, vC[2].y - height / 2f, rt.position.z);
                    break;
                case Position.CustomPosition:
                    rt.position = winOptions.position;
                    break;
                case Position.AsIs:
                    break;
                case Position.Center:
                    rt.position = new Vector3((vC[0].x + vC[2].x) / 2f, (vC[0].y + vC[2].y) / 2f, rt.position.z);
                    break;
            }
            PopUpsController pUp = gP.GetComponent<PopUpsController>();
            if (pUp)
            {
                pUp.SetControlActivity(false);
            }
            return pUp;
        }

        /// <summary>
        /// Set children buttons interactable = activity
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

        #region menus
        public void ShowSettings()
        {
            PopUpsController mm = ShowPopup(SettingsWindowPrefab,  popup.transform, null);
        }

        public void ShowVictory()
        {
            if (onVictory != null) onVictory.Invoke();
            //PopUpsController mm = ShowPopup(VictoryWindowPrefab, popup.transform, null);
            //BubblesPlayer.CurrentLevel += 1;
            //Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
        }

        public void NextLevel()
        {
            BubblesPlayer.CurrentLevel += 1;
            Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
        }

        public void ShowLoss()
        {
            if (onLoss != null) onLoss.Invoke();
            //PopUpsController mm = ShowPopup(LossWindowPrefab, popup.transform, null);
            Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
        }

        public void ShowMission(Action closeCallBack)
        {
            // Vítor Barcellos -- Tirando a popup de missão
            //PopUpsController mm = ShowPopup(MissionPrefab, popup.transform, closeCallBack);
        }

        public void ShowCoinsShop()
        {
            PopUpsController mm = ShowPopup(CoinsShopWindowPrefab,  popup.transform, null);
        }

        public void ShowLifeShop()
        {
            PopUpsController mm = ShowPopup(LifeShopWindowPrefab, popup.transform, null);
        }

        public void ShowBoosterMulticolorShop()
        {
            PopUpsController mm = ShowPopup(BoosterMulticolorShopWindowPrefab, popup.transform, null);
        }

        public void ShowBoosterFireBallShop()
        {
            PopUpsController mm = ShowPopup(BoosterFireBallShopWindowPrefab, popup.transform, null);
        }

        public void ShowBoosterEyeBallShop()
        {
            PopUpsController mm = ShowPopup(BoosterEyeBallShopWindowPrefab, popup.transform, null);
        }

        internal void ShowRateUs()
        {

        }
        #endregion menus

        #region messages
        internal void ShowMessageWithCloseButton(string caption, string message, Action cancelCallBack)
        {
            ShowMessageWithYesNoCloseButton(caption, message, null, cancelCallBack, null);
        }

        internal void ShowMessageWithYesCloseButton(string caption, string message, Action yesCallBack, Action cancelCallBack)
        {
            ShowMessageWithYesNoCloseButton(caption, message, yesCallBack, cancelCallBack, null);
        }

        internal void ShowMessageWithYesNoButton(string caption, string message, Action yesCallBack, Action noCallBack)
        {
            ShowMessageWithYesNoCloseButton(caption, message, yesCallBack, null, noCallBack);
        }

        internal void ShowMessageWithYesNoCloseButton(string caption, string message, Action yesCallBack, Action cancelCallBack, Action noCallBack)
        {
            WarningMessController wMC = CreateMessage(MessageWindowPrefab, caption, message, yesCallBack, cancelCallBack, noCallBack);
        }

        internal void ShowMessageAbout(string caption, string message, Action yesCallBack, Action cancelCallBack, Action noCallBack)
        {
            WarningMessController wMC = CreateMessage(AboutPrefab, caption, message, yesCallBack, cancelCallBack, noCallBack);
        }

        internal void ShowMessageTimeLeft(string caption, string message, float showTime)
        {
            WarningMessController pUp = CreateMessage(TimeLeftPrefab, caption, message, null, null, null);
            SimpleTween.Value(pUp.gameObject, 0,1, showTime).AddCompleteCallBack(()=> { pUp.CloseWindow(); });
        }

        public void ShowMessage(string caption, string message, float showTime, Action completeCallBack)
        {
            StartCoroutine(ShowMessageC(caption, message, showTime, completeCallBack));
        }

        private IEnumerator ShowMessageC(string caption, string message, float showTime, Action completeCallBack)
        {
            WarningMessController pUp = CreateMessage(MessageWindowPrefab, caption, message, null, null, null);
            yield return new WaitForSeconds(showTime);
            if (completeCallBack != null) completeCallBack();
            pUp.CloseWindow();
        }
       
        private WarningMessController CreateMessage(GameObject prefab, string caption, string message, Action yesCallBack, Action cancelCallBack, Action noCallBack)
        {
            PopUpsController p = CreateWindow(prefab, popup.transform);
            WarningMessController pUp = p.GetComponent<WarningMessController>();

            pUp.SetControlActivity(false);
            pUp.PopUpInit(new Action<GameObject>(PopUpOpenH), (g) =>
            {
                PopUpCloseH(g);
                switch (pUp.Answer)
                {
                    case MessageAnswer.Yes:
                        if (yesCallBack != null) yesCallBack();
                        break;
                    case MessageAnswer.Cancel:
                        if (cancelCallBack != null) cancelCallBack();
                        break;
                    case MessageAnswer.No:
                        if (noCallBack != null) noCallBack();
                        break;
                }
            });
            pUp.SetMessage(caption, message, yesCallBack != null, cancelCallBack != null, noCallBack != null);
            p.ShowWindow();
            return pUp;
        }
        #endregion messages
    }
}

 
