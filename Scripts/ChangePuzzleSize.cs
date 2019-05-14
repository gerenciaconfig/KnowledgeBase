using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChangePuzzleSize : MonoBehaviour {

//	[Range(2,18)]
//	public int sizeX = 2;
//	[Range(2, 18)]
//	public int sizeY = 2;

	private int[] sizes = {4,6,12};

	public Button plus;
	public Button minus;
    
    /*
	void OnEnable() {

		if (plus == null)
			GameObject.Find("Plus").GetComponent<Button>();

		if (minus == null)
			GameObject.Find("Minus").GetComponent<Button>();
		
		plus.onClick.AddListener(Plus);
		minus.onClick.AddListener(Minus);
		
		Change();

		minus.interactable = false;
	}

	void OnDisable() {
		plus.onClick.RemoveAllListeners();
		minus.onClick.RemoveAllListeners();
	}*/
	
	public void Change(int curSize) {

		int sizeX = 0, sizeY = 0;

		if (sizes[curSize] == 4) {
			sizeX = 2;
			sizeY = 2;
		} else if (sizes[curSize] == 6) {
			sizeX = 3;
			sizeY = 3;
		} else if (sizes[curSize] == 12) {
			sizeX = 3;
			sizeY = 4;
		}
		
		
		GameObject.Find("GamePuzzle").GetComponent<GamePuzzle>().ChangePuzzleSize(sizeX,sizeY);
	}

    /*
	public void Plus() {

		if (curSize < sizes.Length - 1)
			curSize++;
		
		if(curSize >= sizes.Length - 1)
			plus.interactable = false;

		if (curSize > 0)
			minus.interactable = true;

		Change();
	}

	public void Minus() {
		if (curSize > 0)
			curSize--;
		
		if(curSize == 0)
			minus.interactable = false;

		if (curSize < sizes.Length)
			plus.interactable = true;

		Change();
	}
    */
}
