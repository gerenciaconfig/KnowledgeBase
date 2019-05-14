using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arcolabs.Utilities;

namespace Arcolabs.Utilities
{
	public class MusicPatternInstrument : SerializedMonoBehaviour
	{
		[SerializeField]
		private string patternLoopName;
		public bool isPatternLoop = false;

		[BoxGroup("The drum pattern for this instrument", true, true, 1)]
		public bool[,] pattern = new bool[16, 1];

		private int instrumentsLength;
		private int currentInstrument;

		private static float beatTime = 0f;
		private static float currentTime = 0f;

		private bool canPlay = false;

		private void OnEnable()
		{
			canPlay = false;
		}

		private void OnDisable()
		{
			canPlay = false;
		}

		public void Play()
		{
			canPlay = true;
		}

		// TODO Update the name of this method
		public void BeatUpdate()
		{
			int beatStep = MusicPatternController.GetCurrentBeatIndex();
			if (isPatternLoop && canPlay && pattern[beatStep, 0])
			{
				AudioManager.instance.PlaySound(patternLoopName);
				// currentInstrument = (currentInstrument + 1) % instrumentsLength;
			}
		}
	}
}
