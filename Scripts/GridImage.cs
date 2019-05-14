using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridImage : MonoBehaviour
 {
	public List <Sprite> gridSprites = new List <Sprite>();

	private Image gridImg;

	void Start()
	{
		gridImg = GetComponent<Image>();		
	}

	void Update()
	{
		if(PuzzleSize.changeGrid)
		{
			ChangeSprite();
		}
	}

	public void ChangeSprite()
	{
		switch(PuzzleSize.actualSize)
		{
			case 0:
			gridImg.sprite = gridSprites[PuzzleSize.actualSize];
			break;
			case 1:
			gridImg.sprite = gridSprites[PuzzleSize.actualSize];
			break;
			case 2:
			gridImg.sprite = gridSprites[PuzzleSize.actualSize];
			break;
		}
	}
}