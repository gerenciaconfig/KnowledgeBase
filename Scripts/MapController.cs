using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Fungus;
namespace Mkey
{
    public class MapController : MonoBehaviour
    {
        // Vítor Barcellos -- Adicionando uma referência ao Flowchart que controla o painel do mapa, e ao Game
        [SerializeField] private Flowchart mapFlowchart;
        [SerializeField] private string goToGameBlock;
        [SerializeField] private Game game;
        [SerializeField] private int defaultNumberOfVictoriesToWin = 5;

        private List<LevelButton> mapLevelButtons;
        public List<LevelButton> MapLevelButtons
        {
            get { return mapLevelButtons; }
            set { mapLevelButtons = value; }
        }
        public static MapController Instance;
        public LevelButton ActiveButton
        {
            get; set;
        }

        [HideInInspector]
        public Canvas parentCanvas;
        private ScrollRect sRect;
        private RectTransform content;

       // public static int currentLevel = 1; // set from this script by clicking on button. Use this variable to load appropriate level.
        [SerializeField]
        private int gameSceneOffset = 1;


        [SerializeField]
        private List<Biome> biomeList;
        [Header("If true, then the map will scroll to the Active Level Button", order = 1)]
        public bool scrollToActiveButton = true;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            Debug.Log("Map controller started");
            content = GetComponent<RectTransform>();

            if (!content)
            {
                Debug.LogError("No RectTransform component. Use RectTransform for MapMaker.");
                return;
            }

            biomeList.RemoveAll((b) => { return b == null; });

            MapLevelButtons = new List<LevelButton>();
            foreach (var b in biomeList)
            {
                MapLevelButtons.AddRange(b.levelButtons);
            }

            int topPassedLevel = BubblesPlayer.Instance.TopPassedLevel;
            Debug.Log("topPassedLevel " + topPassedLevel);
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                //1 add listeners
                int buttonNumber = i + 1;
                int currLev = i;
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    if (SoundMasterController.Instance) SoundMasterController.Instance.SoundPlayClick(0, null);
                    BubblesPlayer.CurrentLevel = currLev;
                    if (BubblesPlayer.Instance.Life <= 0) { GuiController.Instance.ShowMessage("Sorry!", "You have no lifes.", 1.5f, () => { GuiController.Instance.ShowLifeShop(); }); return; }
                    Debug.Log("load scene : " + gameSceneOffset + " ;CurrentLevel: " + BubblesPlayer.CurrentLevel);
                    GameBoard.showLevelMission = true;
                    // Vítor Barcellos -- adaptando a chamada ao jogo, pois, agora, toda a atividade fica numa mesma cena
                    game.SetVictoryToWin(defaultNumberOfVictoriesToWin - currLev);
                    mapFlowchart.ExecuteBlock(goToGameBlock);
                    if(SceneLoader.Instance) SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
                });

                // activate buttons
                SetButtonActive(buttonNumber, buttonNumber == topPassedLevel + 2, topPassedLevel + 1 >= buttonNumber);
                //MapLevelButtons[i].numberText.text = (buttonNumber).ToString();
            }
            parentCanvas = GetComponentInParent<Canvas>();
            sRect = GetComponentInParent<ScrollRect>();
            if (scrollToActiveButton) StartCoroutine(SetMapPositionToAciveButton());
        }

        IEnumerator SetMapPositionToAciveButton()
        {
            yield return new WaitForSeconds(0.1f);
            if (sRect)
            {
                int bCount = biomeList.Count;
                float contentSizeY = content.sizeDelta.y / (bCount) * (bCount - 1.0f);
                float relPos = content.InverseTransformPoint(ActiveButton.transform.position).y; // Debug.Log("contentY : " + contentSizeY +  " ;relpos : " + relPos + " : " + relPos / contentSizeY);
                float vpos = (-contentSizeY / (bCount * 2.0f) + relPos) / contentSizeY; // 
                sRect.verticalNormalizedPosition = Mathf.Clamp01(vpos); // Debug.Log("vpos : " + Mathf.Clamp01(vpos));

            }
            else
            {
                Debug.Log("no scrolling rect");
            }
        }

        private void SetButtonActive(int buttonNumber, bool active, bool isPassed)
        {
            int activeStarsCount = BubblesPlayer.Instance.GetLevelStars(buttonNumber);
            MapLevelButtons[buttonNumber - 1].SetActive(active, activeStarsCount, isPassed);
        }

        public void SetControlActivity(bool activity)
        {
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                if (!activity) MapLevelButtons[i].button.interactable = activity;
                else
                {
                    MapLevelButtons[i].button.interactable = MapLevelButtons[i].Interactable;
                }
            }
        }
    }
}