using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mkey
{
    public class ShopThingHelper : MonoBehaviour
    {
        public Image thingImage;
        public Image thingLabelImage;
        public Text thingTextCount;
        public Text thingTextCountOld;
        public Text thingTextPrice;
        public Button thingBuyButton;

        public void SetData(ShopThingData shopThing)
        {
            if (thingImage)
            {
                thingImage.sprite = shopThing.thingImage;
                thingImage.SetNativeSize();
            }
            if (thingTextCount) thingTextCount.text = shopThing.thingCountText;
            if (thingTextCountOld) thingTextCountOld.text = shopThing.thingCountOldText;
            if (thingTextPrice) thingTextPrice.text = shopThing.thingPriceText;

            if (thingLabelImage)
            {
                if (shopThing.thingLabelImage)
                {
                    thingLabelImage.sprite = shopThing.thingLabelImage;
                    thingLabelImage.SetNativeSize();
                }
                else
                {
                    thingLabelImage.gameObject.SetActive(false);
                }
            }
            if (thingBuyButton)
            {
                thingBuyButton.onClick.RemoveAllListeners();
                thingBuyButton.onClick = shopThing.clickEvent;
            }
        }

        public static ShopThingHelper CreateShopThingsHelper(GameObject prefab, RectTransform parent, ShopThingData shopThingData)
        {
            if (!prefab) return null;
            GameObject shopThing = UnityEngine.Object.Instantiate(prefab);
            shopThing.transform.localScale = parent.transform.lossyScale;
            shopThing.transform.SetParent(parent.transform);
            ShopThingHelper sC = shopThing.GetComponent<ShopThingHelper>();
            sC.SetData(shopThingData);
            return sC;
        }

    }

    [System.Serializable]
    public class ShopThingData
    {
        public string name;
        public Sprite thingImage;
        public Sprite thingLabelImage;
        public int thingCount;
        public string thingCountText;
        public string thingCountOldText;
        public float thingPrice;
        public string thingPriceText;
        public string kProductID;
        public GameObject prefab;
        [HideInInspector]
        public Button.ButtonClickedEvent clickEvent;

        public ShopThingData(ShopThingData prod)
        {
            if (prod == null) return;
            name = prod.name;
            thingImage = prod.thingImage;
            thingLabelImage = prod.thingLabelImage;
            thingCount = prod.thingCount;
            thingPrice = prod.thingPrice;
            thingPriceText = prod.thingPriceText;
            thingCountText = prod.thingCountText;
            thingCountOldText = prod.thingCountOldText;
            kProductID = prod.kProductID;
            clickEvent = prod.clickEvent;
        }
    }

    [System.Serializable]
    public class ShopThingDataReal : ShopThingData
    {
        public RealShopType shopType = RealShopType.Coins;
        [Space(8, order = 0)]
        [Header("Purchase Event: ", order = 1)]
        public UnityEvent PurchaseEvent;

        public ShopThingDataReal(ShopThingDataReal prod) : base(prod)
        {
            shopType = prod.shopType;
            PurchaseEvent = prod.PurchaseEvent;
        }

    }

    [System.Serializable]
    public class ShopThingDataInGame: ShopThingData
    {
        public InGameShopType shopType = InGameShopType.BoosterMulticolor;

        public ShopThingDataInGame(ShopThingDataInGame prod) : base(prod)
        {
            shopType = prod.shopType;
        }

    }
}