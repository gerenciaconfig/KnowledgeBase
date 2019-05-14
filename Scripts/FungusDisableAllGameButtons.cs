using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Game", "Disable All Game Buttons", "")]
public class FungusDisableAllGameButtons : Command 
{
	public enum Action {Activate, Desactivate, Desinteract};

	public Action action;

	public override void OnEnter()
    {
		foreach(ButtonBehaviour bb in FindObjectsOfType(typeof(ButtonBehaviour)))
		{
			switch(action)
			{
				case Action.Activate:
				    bb.ActivateButton();
				    break;

				case Action.Desactivate:
				    bb.DeactivateButton();
				    break;

                case Action.Desinteract:
                    bb.Desinteract();
                    break;
            }
			
		}

		Continue();
	}
}