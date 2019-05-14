using UnityEngine.UI;
using UnityEngine;

public class GUIObjectTargetHelper : MonoBehaviour {

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text countText;

    public void SetData(int collected, int toCollect)
    {
        string collectedString = (collected > toCollect) ? toCollect.ToString() : collected.ToString();
        countText.text = collectedString + "/" + toCollect;
    }

}
