using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;
using Arcolabs.Home;

[CommandInfo("Scene", "Change to Home", "Changes the current scene to the Home scene ")]
public class FungusGoToHome : Command 
{
	private string homeName = "FluxoDeTelas";

	public override void OnEnter()
  {
		if(HomeIlhasHelper.firstAcess)
		{
			SceneManager.LoadScene(homeName);
		}
		else
		{
			LoadingScript.nextScene = ConstantClass.HOME;
    	SceneManager.LoadScene(ConstantClass.LOADING);
		}		
		Continue();
	}
}