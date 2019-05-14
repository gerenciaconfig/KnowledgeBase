using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Win_Board : MonoBehaviour
{
    void Start()
	{
		UIRoot.Show(gameObject);
	}

	public void BackGameMenu()
	{
        UIRoot.Clear();
		EventDispatcher.SendEvent(EventName.GameSelect, GamePuzzle.gameID);
	}
}
