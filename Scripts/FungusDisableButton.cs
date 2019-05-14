using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Fungus;

[CommandInfo("UI", "Enable Button", "")]
public class FungusDisableButton : Command
{
    public enum Action { Enable, Disable};

    public Action action;

    public Button button;

    public override void OnEnter()
    {
        switch(action)
        {
            case Action.Disable:
                button.interactable = false;
                break;

            case Action.Enable:
                button.interactable = true;
                break;
        }

        Continue();
    }
}
