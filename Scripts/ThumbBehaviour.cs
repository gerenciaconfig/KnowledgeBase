namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class ThumbBehaviour : MonoBehaviour
    {
        Activity activity;

        public TextMeshProUGUI titleText;

        public TextMeshProUGUI franchiseText;

        public Image actvImage;

        public void SetActivity(Activity activity)
        {
            this.activity = activity;
            UpdateThumb();
        }

        private void UpdateThumb()
        {
            titleText.text = activity.levelDTO.name;

            franchiseText.text = activity.levelDTO.franchise;

            actvImage.sprite = activity.activitySprites.thumbSprite;
        }

        public void ThumbButton()
        {
            LoadingScript.nextScene = activity.activtyScene;
            SceneManager.LoadScene(ConstantClass.LOADING);
        }
    }
}