using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

///Developed By Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
namespace Letra
{
	[CustomEditor (typeof(AudioSources))]
	public class AudioSourcesEditor: Editor
	{
		public override void OnInspectorGUI ()
		{
			AudioSources audioSources = (AudioSources)target;//get the target

			EditorGUILayout.Separator ();
			audioSources.bubbleSFX = EditorGUILayout.ObjectField ("Bubble SFX",audioSources.bubbleSFX, typeof(AudioClip)) as AudioClip;
			EditorGUILayout.HelpBox ("Use the first AudioSource component below for the Music.", MessageType.Info);
			EditorGUILayout.HelpBox ("Use the second AudioSource component below for the Sound Effects.", MessageType.Info);
			EditorGUILayout.HelpBox ("* Click on Apply button that located on the top to save your changes", MessageType.Info);
			EditorGUILayout.Separator ();
		}
	}
}