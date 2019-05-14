using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class PanelContainerController : MonoBehaviour
    {
        public Button OpenCloseButton;
        [SerializeField]
        private ScrollPanelController ScrollPanelPrefab;

        internal ScrollPanelController ScrollPanel;

        public ScrollPanelController InstantiateScrollPanel()
        {
            if (!ScrollPanelPrefab) return null;

            if (ScrollPanel) DestroyImmediate(ScrollPanel.gameObject);

            RectTransform panel = Instantiate(ScrollPanelPrefab).GetComponent<RectTransform>();
            panel.SetParent(GetComponent<RectTransform>());
            panel.anchoredPosition = new Vector2(0, 0);
            ScrollPanel = panel.GetComponent<ScrollPanelController>();
            return ScrollPanel;
        }


    }
}