using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class LevelButtonBehavior : MonoBehaviour {

    public Text buttonText;
    public Action<int> LevelLoadDelegate;  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Button_OnClick()
    {
      //  if (LevelLoadDelegate != null) LevelLoadDelegate();
    }
}
