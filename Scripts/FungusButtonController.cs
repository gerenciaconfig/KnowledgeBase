using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("UI", "Button Controller", "Manipulates the button behaviour script")]
public class FungusButtonController : Command 
{
	public enum ChangeButtonTo {Interact, Deactivate, Activate, Desinteract, DeactivateAll,ActivateAll};

    public ChangeButtonTo changeTo;
    public ButtonBehaviour buttonBehaviour;

	private GameObject[] buttons;

    public override void OnEnter()
    {
		buttons = GameObject.FindGameObjectsWithTag("Button");

        switch (changeTo)
        {
            case ChangeButtonTo.Interact:
                buttonBehaviour.InteractedBtn();
                break;

            case ChangeButtonTo.Deactivate:
                buttonBehaviour.DeactivateButton();
                break;
			
			case ChangeButtonTo.Activate:
                buttonBehaviour.ActivateButton();
                break;

			case ChangeButtonTo.DeactivateAll:

				for(int i = 0; i < buttons.Length; i++)
				{
					buttons[i].GetComponent<ButtonBehaviour>().DeactivateButton();
				}
				break;


            case ChangeButtonTo.Desinteract:

                buttonBehaviour.Desinteract();
                break;

            case ChangeButtonTo.ActivateAll:

				for(int i = 0; i < buttons.Length; i++)
				{
					buttons[i].GetComponent<ButtonBehaviour>().ActivateButton();
				}
                break;
        }
        Continue();
	}
}