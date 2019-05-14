using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchProgressValidator : ProgressValidator {


	public List<int> choosen = new List<int>();

	public void ClearChoosen()
	{
		choosen.Clear();
	}

	public override bool Progressed()
	{
		return true;
	}

	public void AddChoosen(GameObject gameObject)
	{
		choosen.Add(gameObject.transform.parent.GetSiblingIndex());
	}

	public override bool Succeeded()
	{
		int id = choosen[0]; 
		foreach(int element in choosen )
		{
			if (element != id)
				return false;
		}
		return true;
	}

	public void ProgressGroup()
	{
		transform.GetChild(choosen[0]).GetComponent<ProgressController>().Progress();
	}

}
