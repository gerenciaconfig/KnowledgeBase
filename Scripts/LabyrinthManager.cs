using Arcolabs.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs
{
	public class LabyrinthManager : MonoBehaviour
	{
		[SerializeField]
		private LineTracingManager[] tracingManagers;

		private void Start()
		{
			// select one activity to be the winner
			// change the color of the paths
		}

		private void OnEnable()
		{
			// TODO find another way to reset the game, since it's not happening
			// only when this container is enabled
			foreach (LineTracingManager ltm in tracingManagers)
			{
				ltm.ResetPath();
				Debug.Log("Resetting line");
			}
		}

		public void ResetPathState()
		{
			// ltm.ResetPath();
		}
	}
}

