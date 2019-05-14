using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofiaCorMagicaManager : MonoBehaviour
{
	//[SerializeField]
	//private bool introLevel = true;

	//[SerializeField]
	//private int randomLevels = 3;

	//private int currentLevel = 0;

	private void Start()
	{
		StartCoroutine(IntroLevel());
	}

	private IEnumerator IntroLevel()
	{
		// turn on scene with two potions
		// change color of the potions
		// reset color of the big potions
		// turn off second scene
		// fade in
		// play audio
		// when the correct bottle is down, play second audio
		// when correct bottle is down, play congratulations audio
		// fade out
		// =======
		// turn off screen one
		// turn on screen two
		// select random potion
		// change potions accordingly
		// randomize potion positions (this could be done later)
		yield return null;
	}

	private void NextGame()
	{
		
	}
}
