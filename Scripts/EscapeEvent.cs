using UnityEngine;
using System.Collections;
using UnityEngine.Events;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

/// <summary>
/// Escape or Back event
/// </summary>
namespace Letra
{
	[DisallowMultipleComponent]
	public class EscapeEvent : MonoBehaviour
	{
		/// <summary>
		/// On escape/back event
		/// </summary>
		public UnityEvent escapeEvent;

		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Escape)) {
				OnEscapeClick ();
			}
		}

		/// <summary>
		/// On Escape click event.
		/// </summary>
		public void OnEscapeClick ()
		{
			escapeEvent.Invoke ();
		}
	}
}