using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;


[CommandInfo("Game", "Set Scale ", "Sets the scale")]
public class FungusScaleObjects : Command 
{
	public List<GameObject> scaleObj = new List<GameObject>();

	public float scaleX;
	public float scaleY;
	public float scaleZ;

	public override void OnEnter()
    {
		for(int i = 0; i < scaleObj.Count; i++)
		{
			scaleObj[i].GetComponent<RectTransform>().localScale = new Vector3(scaleX,scaleY,scaleZ);
		}


		Continue();
	}

}