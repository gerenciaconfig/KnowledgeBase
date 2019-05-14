using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetGameMenu : MonoBehaviour
 {
	public List <GameObject> lvlManager = new List<GameObject>();

	public void ResetAllLevels()
	{
		for(int i = 0; i < lvlManager.Count; i++)
		{
			foreach(GameObject lvl in lvlManager)
			{
				lvl.GetComponent<Manager>().ResetManager();
			}
		}
	}
}