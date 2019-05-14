using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class FooterGUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject footerBoosterPrefab;
        [SerializeField]
        private RectTransform BoostersParent;
        [SerializeField]
        private GameObject FooterContent;
        [SerializeField]
        private Text MovesCountText;

        public static FooterGUIController Instance;

        #region regular
        void Awake()
        {
            if (Instance) Destroy(Instance.gameObject);
            Instance = this;
        }

        public void InitStart()
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                gameObject.SetActive(false);
                return;
            }
            CreateBoostersPanel();
        }
        #endregion regular

        private void CreateBoostersPanel()
        {
            FooterBoosterHelper[] fBH = BoostersParent.GetComponentsInChildren<FooterBoosterHelper>();
            foreach (FooterBoosterHelper item in fBH)
            {
                DestroyImmediate(item.gameObject);
            }
            BoostersHolder bHolder = BubblesPlayer.Instance.BoostHolder;
            foreach (var item in bHolder.Boosters)
            {
                item.CreateFooterBooster(BoostersParent, footerBoosterPrefab, () =>
               {
                   int id = item.bData.ID;
                   InGamePurchaser iGP = InGamePurchaser.Instance;
                   ShopThingDataInGame sd = iGP.GetProductById(id.ToString());
                   if (GuiController.Instance)
                       switch (sd.shopType)
                       {
                           case InGameShopType.None:
                               break;
                           case InGameShopType.BoosterMulticolor:
                               GuiController.Instance.ShowBoosterMulticolorShop();
                               break;
                           case InGameShopType.BoosterFireBall:
                               GuiController.Instance.ShowBoosterFireBallShop();
                               break;
                           case InGameShopType.BoosterEyeBall:
                               GuiController.Instance.ShowBoosterEyeBallShop();
                               break;
                           default:
                               break;
                       }

               });
            }
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

    }
}