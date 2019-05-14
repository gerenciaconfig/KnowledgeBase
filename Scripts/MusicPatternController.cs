using UnityEngine;
using UnityEngine.Events;

namespace Arcolabs.Utilities
{
	public class MusicPatternController : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent callbackEvents;
		public static float baseTime = 0f;
		private static int totalBeats = 15;
		private static int oldBeat = 0;
		private static int currentBeat = 0;
		private static float bpm = 120;
		private static float sixteenthNoteLength;

		void Start()
		{
			sixteenthNoteLength = bpm / 960f;
			currentBeat = oldBeat = 0;
		}

		// Update is called once per frame
		void Update()
		{
			// adapt the 2 to work with different bpm
			baseTime = (baseTime + Time.deltaTime) % 2;
			currentBeat = (int) (baseTime / sixteenthNoteLength);
			if (currentBeat != oldBeat)
			{
				oldBeat = currentBeat;
				callbackEvents.Invoke();
				// Debug.Log("DJ! Spin that sh*t");
			}
		}

		public static int GetCurrentBeatIndex()
		{
			return currentBeat;
		}
	}
}
