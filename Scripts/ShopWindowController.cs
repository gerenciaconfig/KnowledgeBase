using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{

    public class ShopWindowController : PopUpsController
    {
        public RectTransform ThingsParent;
        private List<ShopThingHelper> shopThings;

        [SerializeField]
        private RealShopType shopType = RealShopType.Coins;

        void Start()
        {
            CreateThingTab();
        }

        public override void RefreshWindow()
        {

            base.RefreshWindow();
        }

        public void Cancel_Click()
        {
            if (SoundMasterController.Instance) SoundMasterController.Instance.SoundPlayClick(0.0f, null);
            CloseButton_click();
        }

        private void CreateThingTab()
        {
            ShopThingHelper[] sT = ThingsParent.GetComponentsInChildren<ShopThingHelper>();
            foreach (var item in sT)
            {
                DestroyImmediate(item.gameObject);
            }

            Purchaser p = Purchaser.Instance;
            if (p == null) return;

            List<ShopThingDataReal> products = new List<ShopThingDataReal>();
            if (p.consumable != null && p.consumable.Length > 0) products.AddRange(p.consumable);
            if (p.nonConsumable != null && p.nonConsumable.Length > 0) products.AddRange(p.nonConsumable);
            if (p.subscriptions != null && p.subscriptions.Length > 0) products.AddRange(p.subscriptions);

            VideoPurchaser vP = VideoPurchaser.Instance;
            if (vP && vP.gameProducts!=null &&  vP.gameProducts.Length > 0)
            {
                products.AddRange(vP.gameProducts);
            }

            if (products.Count==0) return;

            shopThings = new List<ShopThingHelper>();
            for (int i = 0; i < products.Count; i++)
            {
              if(products[i]!=null && (products[i].shopType == shopType) && products[i].prefab)  shopThings.Add(ShopThingHelper.CreateShopThingsHelper(products[i].prefab, ThingsParent, products[i]));
            }
        }
    }
}