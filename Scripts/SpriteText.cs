using UnityEngine;
namespace Mkey
{
    /*
     25.11.18
     V1.1
     Add editor
     Add method RefreshSort();
     */
    [ExecuteInEditMode]
    public class SpriteText : MonoBehaviour
    {
        public int sortingOrder;
        public int sortingLayerID;

        private Renderer rend;

        void Start()
        {
            RefreshSort();
        }
        
        public void RefreshSort()
        {
            sortingOrder = Mathf.Max(0, sortingOrder);

            if (!rend)
                rend = GetComponent<Renderer>();
            if (!rend) return;

            rend.sortingLayerID = sortingLayerID;
            rend.sortingOrder = sortingOrder;
        }
    }
}