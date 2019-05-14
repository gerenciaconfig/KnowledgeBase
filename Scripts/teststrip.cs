using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mkey
{

    [ExecuteInEditMode]
    public class teststrip : MonoBehaviour
    {

        private Image ScoreStrip;

        public RectTransform yellowLight;

        void Update()
        {
            if (!ScoreStrip) ScoreStrip = GetComponent<Image>();
            if (!ScoreStrip) return;
            float sizeX = ScoreStrip.GetComponent<RectTransform>().sizeDelta.x;
            if (yellowLight)
                yellowLight.anchoredPosition = new Vector2(sizeX * Mathf.Min(ScoreStrip.fillAmount, 0.925f), yellowLight.anchoredPosition.y);
        }
    }
}