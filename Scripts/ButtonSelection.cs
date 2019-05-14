using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    public static ButtonSelection instance;

    public Button rightBtn;

    [Space(10)]
    public static Image currentSelection;
    public List<Image> imagesList;

    private void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        instance = this;

        foreach (var item in imagesList)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 1);
        }
    }

    public void SelectButton()
    {
        foreach (var item in imagesList)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0.5f);
        }

        currentSelection.color = new Color(currentSelection.color.r, currentSelection.color.g, currentSelection.color.b, 1f);
    }
}
