using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class SimpleSlider : MonoBehaviour
    {
        public Image fillImage;

        public float value
        {
            get
            {
                return fillImage.fillAmount;
            }
            set
            {
                fillImage.fillAmount = value;
            }
        }
    }
}