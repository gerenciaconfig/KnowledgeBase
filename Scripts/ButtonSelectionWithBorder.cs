using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ButtonSelectionWithBorder : SerializedMonoBehaviour
{
    public static ButtonSelectionWithBorder instance;

    public Button rightBtn;

    [Space(10)]
    public KeyValuePair<Image, Image> currentSelection = new KeyValuePair<Image, Image>();
    public Dictionary<Image, Image> imagesDicitionary;

    private void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        foreach (var item in imagesDicitionary)
        {
            item.Key.color = new Color(item.Key.color.r, item.Key.color.g, item.Key.color.b, 1);
            item.Value.color = new Color(item.Value.color.r, item.Value.color.g, item.Value.color.b, 1);
        }
    }

    public void SelectButton()
    {
        foreach (var item in imagesDicitionary)
        {
            item.Key.color = new Color(item.Key.color.r, item.Key.color.g, item.Key.color.b, 0.2f);
            item.Value.color = new Color(item.Value.color.r, item.Value.color.g, item.Value.color.b, 0.2f);
        }

        currentSelection.Key.color = new Color(currentSelection.Key.color.r, currentSelection.Key.color.g, currentSelection.Key.color.b, 1f);
        currentSelection.Value.color = new Color(currentSelection.Value.color.r, currentSelection.Value.color.g, currentSelection.Value.color.b, 1f);
    }
}
