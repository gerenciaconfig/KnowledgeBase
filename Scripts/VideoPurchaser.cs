using System;
using UnityEngine;

namespace Mkey
{
    public class VideoPurchaser : MonoBehaviour
    {
        public ShopThingDataReal[] gameProducts;

        public static VideoPurchaser Instance;

        void Awake()
        {
            if (Instance) Destroy(gameObject);
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            InitializePurchasing();
        }

        /// <summary>
        /// Add for each button product clickEevnt
        /// </summary>
        private void InitializePurchasing()
        {
            if (gameProducts != null && gameProducts.Length > 0)
            {
                for (int i = 0; i < gameProducts.Length; i++)
                {
                    if (gameProducts[i] != null && !string.IsNullOrEmpty(gameProducts[i].kProductID))
                    {
                        string prodID = gameProducts[i].kProductID;
                        string prodName = gameProducts[i].name;
                        int count = gameProducts[i].thingCount;
                        int price = (int)gameProducts[i].thingPrice;

                        gameProducts[i].clickEvent.RemoveAllListeners();
                        gameProducts[i].clickEvent.AddListener(() => { ShowVideo(prodID, prodName, count, price); });
                    }
                }
            }
        }

        /// <summary>
        /// Buy product, increase product count
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="prodName"></param>
        /// <param name="count"></param>
        /// <param name="price"></param>
        public void ShowVideo(string prodID, string prodName, int count, int price)
        {
            int id;
            bool result = false;
            //if (BubblesPlayer.Instance.Coins >= price)
            //{
            //    if (int.TryParse(prodID, out id))
            //    {
            //        if (id > 0)
            //        {
            //            Booster b = BubblesPlayer.Instance.BoostHolder.GetBoosterById(id);
            //            if (b != null)
            //            {
            //                b.AddCount(count);
            //                result = true;
            //            }
            //        }
            //    }
            //}

            if (result)
            {
                GoodPurchaseMessage(prodID, prodName);
            }
            else
            {
                FailedPurchaseMessage(prodID, prodName);
            }
        }

        public void AddCoins(int count)
        {
            BubblesPlayer.Instance.AddCoins(count);
        }

        public void SetInfiniteLife(int hours)
        {
            BubblesPlayer.Instance.StartInfiniteLife(hours);
        }

        public void AddLife(int count)
        {
            BubblesPlayer.Instance.AddLifes(count);
        }

        /// <summary>
        /// Show good purchase message
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="prodName"></param>
        private void GoodPurchaseMessage(string prodId, string prodName)
        {
            if (GuiController.Instance)
            {
                GuiController.Instance.ShowMessage("Succesfull!!!", prodName + " received successfull.", 3, null);
            }
        }

        /// <summary>
        /// Show failed purchase message
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="prodName"></param>
        private void FailedPurchaseMessage(string prodId, string prodName)
        {
            if (GuiController.Instance)
            {
                GuiController.Instance.ShowMessage("Sorry.", prodName + " - not received.", 3, null);
            }
        }

        /// <summary>
        /// Search in array gameProducts appropriate product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShopThingDataReal GetProductById(string id)
        {
            if (gameProducts != null && gameProducts.Length > 0)
                for (int i = 0; i < gameProducts.Length; i++)
                {
                    if (gameProducts[i] != null)
                        if (String.Equals(id, gameProducts[i].kProductID, StringComparison.Ordinal))
                            return gameProducts[i];
                }
            return null;
        }
    }
}