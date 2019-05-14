using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Fungus;

[EventHandlerInfo("Game",
                    "Hint",
                    "The block will execute when the hint is needed")]
[AddComponentMenu("")]

public class HintEvent : EventHandler 
{
	public static HintEvent instance;
   	private void Start()
	{
        instance = this;
    }
	
	public static void Hint()
	{
		//GenericParameter gp = GameObject.FindGameObjectWithTag("Game").GetComponent<GenericParameter>();
        //gp.SetParameter(hint);
		instance.ExecuteBlock();
	}
}