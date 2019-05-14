using UnityEngine.UI;
using UnityEngine;

namespace Mkey
{
    public class ButtonClickSound : MonoBehaviour
    {

        Button b;

        void Start()
        {
            b = GetComponent<Button>();
            if (b)
            {
                b.onClick.RemoveListener(ClickSound);
                b.onClick.AddListener(ClickSound);
            }
        }

        public void ClickSound()
        {
            SoundMasterController.Instance.SoundPlayClick(0, null);
        }
    }
}
