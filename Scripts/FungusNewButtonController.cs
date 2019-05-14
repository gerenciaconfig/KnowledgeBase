using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("UI", "New Button Controller", "Manipulates the button behaviour script")]
public class FungusNewButtonController : Command
{

    public enum ChangeButtonTo { Deactivate, Activate, DeactivateAll, ActivateAll };

    public ChangeButtonTo changeTo;
    public NewButtonBehaviour buttonBehaviour;

    public Transform buttonsParent;

    private NewButtonBehaviour[] buttons;

    public override void OnEnter()
    {
        switch (changeTo)
        {
            case ChangeButtonTo.Deactivate:
                buttonBehaviour.DesactivateButton();
                break;

            case ChangeButtonTo.Activate:
                buttonBehaviour.ActivateButton();
                break;

            case ChangeButtonTo.DeactivateAll:
                buttons = buttonsParent.GetComponentsInChildren<NewButtonBehaviour>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].DesactivateButton();
                }
                break;

            case ChangeButtonTo.ActivateAll:
                buttons = buttonsParent.GetComponentsInChildren<NewButtonBehaviour>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].ActivateButton();
                }
                break;
        }
        Continue();
    }
}