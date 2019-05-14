using UnityEngine;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
{
	public class AudioSources : MonoBehaviour 
	{

		/// <summary>
		/// The loading canvas instance.
		/// </summary>
		public static AudioSources instance;

		/// <summary>
		/// The audio sources.
		/// </summary>
		[HideInInspector]
		public AudioSource [] audioSources;

		/// <summary>
		/// The bubble sound effect.
		/// </summary>
		public AudioClip bubbleSFX;

		// Use this for initialization
		void Awake ()
		{
			if (instance == null) {
				instance = this;
				audioSources = GetComponents<AudioSource>();
				DontDestroyOnLoad(gameObject);
			} else {
				Destroy (gameObject);
			}
		}

		public void PlayBubbleSFX(){
			if (bubbleSFX != null && audioSources[1] != null) {
				CommonUtil.PlayOneShotClipAt (bubbleSFX, Vector3.zero, audioSources[1].volume);
			}

		}
	}
}