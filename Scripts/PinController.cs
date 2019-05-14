using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Home
{
    public class PinController : MonoBehaviour
    {
        public string currentNumber;
        Camera cam ;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void OnEnable()
        {
            currentNumber = string.Empty;
            cam.GetComponent<IntroHelper>().DisableColliders();
        }

        public void InsertNumber(int number)
        {
            currentNumber += number.ToString();

            if (currentNumber.Length == 4)
            {
                if (currentNumber == CurrentStatsInfo.currentUser.pin.ToString())
                {
                    //TODO - CHAMADA CASO O PIN ESTEJA CERTO DENTRO DESSE BLOCO IF.
                    HudManager.PinAcess();
                }
                else
                {
                    Message.instance.Show(MessageClass.ERROR_INCORRECT_PIN);
                }

                currentNumber = string.Empty;
            }
        }

        private void OnDisable()
        {
            cam.GetComponent<IntroHelper>().EnableColliders();
        }
    }
}
