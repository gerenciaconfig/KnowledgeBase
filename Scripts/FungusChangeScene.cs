using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

[CommandInfo("Scene", "Change Scene", "Changes the current scene to the given scene name")]
public class FungusChangeScene : Command
{
    public string sceneName;

    public override void OnEnter()
    {
       SceneManager.LoadScene(sceneName);
        Continue();
    }
}