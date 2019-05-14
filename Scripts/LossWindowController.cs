using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class LossWindowController : PopUpsController
    {
        public Text missionDescriptionText;

        public override void RefreshWindow()
        {
            string description = (GameBoard.LcSet) ? GameBoard.LcSet.levelMission.Description : "";
            missionDescriptionText.text = description;
            missionDescriptionText.enabled = !string.IsNullOrEmpty(description);
            base.RefreshWindow();
        }

        public void Cancel_Click()
        {
            CloseWindow();
            SceneLoader.Instance.LoadScene(0);
        }

        public void Retry_Click()
        {
            CloseWindow();
            SceneLoader.Instance.LoadScene(0);
        }

    }
}