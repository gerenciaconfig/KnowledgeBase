using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeButton : MonoBehaviour
{
    private int[] sizes = { 4, 6, 12 };

    public int index;
	
    public void SelectGrid()
    {
        int sizeX = 0, sizeY = 0;

        if (sizes[index] == 4)
        {
            sizeX = 2;
            sizeY = 2;
        }
        else if (sizes[index] == 6)
        {
            sizeX = 3;
            sizeY = 3;
        }
        else if (sizes[index] == 12)
        {
            sizeX = 3;
            sizeY = 4;
        }

        PlayerPrefs.SetInt("gridIndex", index);
        GameObject.Find("GamePuzzle").GetComponent<GamePuzzle>().ChangePuzzleSize(sizeX, sizeY);
        GameObject.Find("Win_GameMenu").GetComponent<Win_GameMenu>().StartPuzzle();
    }
}
