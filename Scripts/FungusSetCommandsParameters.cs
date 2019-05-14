using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[CommandInfo("Flow", "Set Commands Parameters", "Sets the game object parameter of all of the folowing commands of a block")]
public class FungusSetCommandsParameters : Command
{
    public int parameterIndex;

    public enum CommandType {ScaleTo, MoveTo, RotateTo, ButtonSpriteChange, ButtonController, PlayCardSound};

    public List<CommandType> commands = new List<CommandType>();

    public GenericParameter gp;

    //public GameObjectData a;
    public override void OnEnter()
    {
        //gb. = CardFlippedEvent.GetFlippedCard();
        for (int i = 0; i < commands.Count; i++)
        {
            switch(commands[i])
            {
                case CommandType.ScaleTo:
                foreach (ScaleTo scaleTo in GetComponents<ScaleTo>())
                {
                        scaleTo._targetObject.gameObjectVal = gp.GetParameter(parameterIndex);
                }
                break;

                case CommandType.MoveTo:
                foreach (MoveTo moveTo in GetComponents<MoveTo>())
                {
                        moveTo._targetObject.gameObjectVal = gp.GetParameter(parameterIndex);
                }
                break;

                case CommandType.RotateTo:
                foreach (RotateTo rotateTo in GetComponents<RotateTo>())
                {
                    rotateTo._targetObject.gameObjectVal = gp.GetParameter(parameterIndex);
                }
                break;

                case CommandType.ButtonSpriteChange: 

                foreach (FungusSpriteChanger fungusSpriteChanger in GetComponents<FungusSpriteChanger>())
                {
                    fungusSpriteChanger.buttonBehaviour = gp.GetParameter(parameterIndex).GetComponent<ButtonBehaviour>();
                }
                break;

                case CommandType.ButtonController:

                    foreach (FungusButtonController buttonController in GetComponents<FungusButtonController>())
                    {
                        buttonController.buttonBehaviour = gp.GetParameter(parameterIndex).GetComponent<ButtonBehaviour>();
                    }
                    break;

                case CommandType.PlayCardSound:

                    foreach (FungusPlayCardSound playCardSound in GetComponents<FungusPlayCardSound>())
                    {
                        playCardSound.card = gp.GetParameter(parameterIndex).GetComponent<Card>();
                    }
                    break;
            }
        }
        Continue();
    }
}
