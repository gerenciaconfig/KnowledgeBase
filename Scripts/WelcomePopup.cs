using UnityEditor;
using UnityEngine;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
{
	[InitializeOnLoad]
	public class WelcomePopup : EditorWindow
	{
		private static WelcomePopup window;
		private static bool initilized;
		private static bool dontShowWeclomeMessageAgain;
		private static Vector2 size = new Vector2 (600, 280);
		private static string strKey = "ATUEditor_WelcomePopup";

		static WelcomePopup ()
		{
			EditorApplication.update += Update;
		}

		[MenuItem ("Tools/English Alphabet A-Z/Welcome", false, 2)]
		static void ReadManual ()
		{
			initilized = false;
			PlayerPrefs.SetInt (strKey, CommonUtil.TrueFalseBoolToZeroOne (false));
			Init ();
		}

		[MenuItem ("Tools/English Alphabet A-Z/About Author", false, 3)]
		static void AboutMe ()
		{
			Application.OpenURL (Links.authorPath);
		}

		private static void Init ()
		{

			if (initilized) {
				return;
			}

			if (PlayerPrefs.HasKey (strKey)) {
				dontShowWeclomeMessageAgain = CommonUtil.ZeroOneToTrueFalseBool (PlayerPrefs.GetInt (strKey));
			}

			if (dontShowWeclomeMessageAgain) {
				return;
			}

			window = (WelcomePopup)EditorWindow.GetWindow (typeof(WelcomePopup));
			window.titleContent.text = "Welcome";
			window.maxSize = size;
			window.maximized = true;
			window.position = new Rect ((Screen.currentResolution.width - size.x) / 2, (Screen.currentResolution.height - size.y) / 2, size.x, size.y);
			window.Show ();
			window.Focus ();

			initilized = true;

			PlayerPrefs.SetInt (strKey, CommonUtil.TrueFalseBoolToZeroOne (true));
		}

		static void Update ()
		{
			if (Application.isPlaying) {
				if (window != null) {
					window.Close ();
					window = null;
				}
				return;
			}

			if (window == null) {
				Init ();
			}
		}

		void OnGUI ()
		{
			if (window == null) {
				return;
			}

			EditorGUILayout.Separator ();
			EditorGUILayout.LabelField ("English Alphabet A-Z " + Links.versionCode, EditorStyles.boldLabel);
			EditorGUILayout.Separator ();

			EditorGUILayout.TextArea ("Thank you for buying/downloading English Alphabet A-Z Package.\nIf you have any questions, suggestions, comments , feature requests or bug detected,\ndo not hesitate to Contact US.\n", GUI.skin.label);
			EditorGUILayout.Separator ();

			EditorGUILayout.TextArea ("We always strive to provide high quality assets. If you have enjoyed with English Alphabet A-Z,\nwe would be happy if you would spend few minutes and write a review for us on the \n" + Links.storeName + ".\n", GUI.skin.label);
			EditorGUILayout.Separator ();

			EditorGUILayout.TextArea ("If one of the buttons below does not work , you will find its file under the package folder.\n", GUI.skin.label);

			EditorGUILayout.BeginHorizontal ();
			GUI.backgroundColor = Colors.yellowColor;
			if (GUILayout.Button ("Read the Manual", GUILayout.Width (120), GUILayout.Height (22))) {
				Application.OpenURL (Links.docPath);
			}
			GUI.backgroundColor = Colors.whiteColor;

			if (GUILayout.Button ("Version Changes", GUILayout.Width (120), GUILayout.Height (22))) {
				Application.OpenURL (Links.versionChangesPath);
			}

			if (GUILayout.Button ("More Assets", GUILayout.Width (100), GUILayout.Height (22))) {
				Application.OpenURL (Links.indieStudioStoreURL);
			}

			if (GUILayout.Button ("Legal Terms", GUILayout.Width (100), GUILayout.Height (22))) {
				Application.OpenURL (Links.assetStoreEULA);
			}

			if (GUILayout.Button ("Contact US", GUILayout.Width (100), GUILayout.Height (22))) {
				Application.OpenURL (Links.indieStudioContactUsURL);
			}

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Separator ();
			EditorGUILayout.Separator ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space (size.x/2.0f - 100);
			GUI.backgroundColor = Color.clear;
			if (GUILayout.Button ("Indie Studio", GUILayout.Width (200), GUILayout.Height (22))) {
				Application.OpenURL (Links.indieStudioStoreURL);
			}
			EditorGUILayout.EndHorizontal ();
		}


		void OnInspectorUpdate ()
		{
			Repaint ();
		}
	}
}