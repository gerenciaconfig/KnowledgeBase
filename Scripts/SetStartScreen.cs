using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    [CommandInfo("Game",
             "SetStartScreen",
             "Create Start Screen")]
    [AddComponentMenu("")]
    public class SetStartScreen : SetGame
    {
        // Insert Command Fields Here ! ! !
        private GameObject playButton;
        private GameObject closeButton;

        public override void OnEnter()
        {
            //Comando de teste
           
            playButton = GameObject.FindGameObjectWithTag("PlayButton");
            closeButton = GameObject.FindGameObjectWithTag("CloseButton");
            playButton.GetComponent<Button>().onClick.AddListener(game.OnGameEnd.Invoke);
           // closeButton.GetComponent<Button>().onClick.AddListener(game.OnGameEnd.Invoke);
        }

    }
}