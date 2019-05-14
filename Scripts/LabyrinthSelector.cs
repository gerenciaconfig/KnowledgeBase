using Arcolabs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSelector : MonoBehaviour
{
	[SerializeField]
	private LabyrinthManager[] labyrinths;
	private int randomIndex = -1; // initializing with an impossible index

	private void Start()
	{
		// upon start, all game objects should be deactivated
		// note: onEnable is called before start		
		for (int i = 0; i < labyrinths.Length; i++)
		{
			labyrinths[i].gameObject.SetActive(i == randomIndex);
		}
	}

	public void OnEnable()
	{
		// Destroy all the children from the previous one
		// Select one labyrinth at random
		// labyrinths[0].ResetPathState(); // maybe we don't need to do this??
		EnableRandomLabyrinth();
	}

	public void EnableRandomLabyrinth()
	{
		UpdateRandomIndex();
		labyrinths[randomIndex].gameObject.SetActive(true);
		Debug.Log("Selecting a new random labyrinth. Index: " + randomIndex);
	}

	public void OnDisable()
	{
		// disable last Labyrinth GameObject
		labyrinths[randomIndex].gameObject.SetActive(false);
		Debug.Log("Disabling Labyrinth Index " + randomIndex);
	}

	/// <summary>
	/// Selects a new random index based on the labyrinths array size.
	/// <para>
	/// If we have at least two elements in our array, this method is guaranteed
	/// to return a new index
	/// </para>
	/// </summary>
	private void UpdateRandomIndex()
	{
		randomIndex = 0;
		return;
		// Reimplement this method once the system is implemented
		int total = labyrinths.Length - 1;
		if (total == 0) return; // prevents infinite loop since we only have one index;

		int lastIndex = randomIndex;
		while (lastIndex == randomIndex)
		{
			randomIndex = Random.Range(0, total);
		}
	}
}
