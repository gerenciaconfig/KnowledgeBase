using UnityEngine.UI;
using UnityEngine;
namespace Mkey
{
    public class SettingsMenuController : PopUpsController
    {
        public Button musicButton;
        public Sprite musicButtonOnSprite;
        public Sprite musicButtonOffSprite;
        public Image musicIcon;
        public Sprite musicIconOnSprite;
        public Sprite musicIconOffSprite;


        public Button soundButton;
        public Sprite soundButtonOnSprite;
        public Sprite soundButtonOffSprite;
        public Image  soundIcon;
        public Sprite soundIconOnSprite;
        public Sprite soundIconOffSprite;
        public Text facebookButtonText;
        public Text getCoinsText;

        public string ANDROID_RATE_URL;
        public string IOS_RATE_URL;
        public string SUPPORT_URL;
        public static SettingsMenuController Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void MusikButton_Click()
        {
            SoundMasterController.Instance.MusicOn = !SoundMasterController.Instance.MusicOn;
            SetMusicButtSprite(SoundMasterController.Instance.MusicOn);
        }

        public void SoundButton_Click()
        {
            if (SoundMasterController.Instance)
            {
                SoundMasterController.Instance.SoundOn = !SoundMasterController.Instance.SoundOn;
                SetSoundButtSprite(SoundMasterController.Instance.SoundOn);
            }
        }

        public void FacebookButton_Click()
        {
            if (FBholder.Instance)
            {
                if (!FBholder.IsLogined) FBholder.Instance.FBlogin();
                else FBholder.Instance.FBlogOut(() => { SetFBButtText(); });
            }
        }

        public void AboutButton_Click()
        {
           if (GuiController.Instance)  GuiController.Instance.ShowMessageAbout("", "", () => { Debug.Log("Support"); if (!string.IsNullOrEmpty(SUPPORT_URL)) Application.OpenURL(SUPPORT_URL); }, ()=> { }, null);
        }

        public void RateButton_Click()
        {
#if UNITY_ANDROID
            if (!string.IsNullOrEmpty(ANDROID_RATE_URL)) Application.OpenURL(ANDROID_RATE_URL);
#elif UNITY_IOS
            if (!string.IsNullOrEmpty(IOS_RATE_URL)) Application.OpenURL(IOS_RATE_URL);
#else
            if (!string.IsNullOrEmpty(ANDROID_RATE_URL)) Application.OpenURL(ANDROID_RATE_URL);
#endif
       
        }

        private void SetMusicButtSprite(bool musicOn)
        {
            musicButton.image.sprite = (musicOn)? musicButtonOnSprite : musicButtonOffSprite;
            musicIcon.sprite = (musicOn) ? musicIconOnSprite : musicIconOffSprite;
        }

        private void SetSoundButtSprite(bool soundOn)
        {
            soundButton.image.sprite = (soundOn) ? soundButtonOnSprite : soundButtonOffSprite;
            soundIcon.sprite = (soundOn) ? soundIconOnSprite : soundIconOffSprite;
        }

        private void SetFBButtText()
        {
            if(facebookButtonText)
                facebookButtonText.text = (FBholder.IsLogined) ? "Logout" : "Login";
        }

        public override void RefreshWindow()
        {
           // SetSoundButtVolume(SoundMasterController.Instance.Volume); // not used
            SetMusicButtSprite(SoundMasterController.Instance.MusicOn);
            SetSoundButtSprite(SoundMasterController.Instance.SoundOn);
            SetFBButtText();
            if (getCoinsText) getCoinsText.text = "get +" + BubblesPlayer.Instance.facebookCoins;
            base.RefreshWindow();
        }

        #region set volume (not used)
        private Image[] volume;
        public void SoundPlusButton_Click()
        {
            SoundMasterController.Instance.Volume += 0.1f;
            SetSoundButtVolume(SoundMasterController.Instance.Volume);
        }

        public void SoundMinusButton_Click()
        {
            SoundMasterController.Instance.Volume -= 0.1f;
            SetSoundButtVolume(SoundMasterController.Instance.Volume);
        }

        private void SetSoundButtVolume(float soundVolume)
        {
            if (volume != null && volume.Length > 0)
            {
                int length = volume.Length;
                float vpl = 1.0f / (float)length;
                int count = Mathf.RoundToInt(soundVolume / vpl);
                Debug.Log("soundVol: " + soundVolume + " ; count: " + count + " ;s/vpl: " + soundVolume / vpl);
                SetVolume(count);
            }
        }

        private void SetVolume(int count)
        {
            for (int i = 0; i < volume.Length; i++)
            {
                volume[i].gameObject.SetActive(i < count);
            }
        }
        #endregion volume

    }
}