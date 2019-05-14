using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSize : MonoBehaviour 
{
	public static int actualSize = 0;
	public static bool changeGrid = false;

	public int sizeX;
	public int sizeY;

	void Start()
	{
		Check();
	}

	void Update()
	{
		if(actualSize >= 2)
		{
			actualSize = 2;
		}

		if(actualSize <= 0)
		{
			actualSize = 0;
		}				
	}

	public void Add()
	{
		actualSize ++;
		Check();
	}

	public void Remove()
	{
		actualSize--;
		Check();
	}

	public void Check()
	{
		switch(actualSize)
		{
			case 0:
			sizeX = 2;
			sizeY = 2;
			break;
			case 1:
			sizeX = 3;
			sizeY = 3;
			break;
			case 2:
			sizeX = 3;
			sizeY = 4;
			break;
		}
		
		changeGrid = true;
		GameObject.Find("GamePuzzle").GetComponent<GamePuzzle>().ChangePuzzleSize(sizeX,sizeY);
	}

	public void ResetSize()
	{
		actualSize = 0;
		Check();
	}
}