//v1_0(05062018)

using UnityEngine;
using UnityEditor;

namespace Mkey
{
    [CustomEditor(typeof(GameBoard))]
    public class GamePlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.BeginHorizontal("box");
			if (GUILayout.Button ("Add 500 coins"))
            {
			    BubblesPlayer.Instance.AddCoins(500);
			}

            if (GUILayout.Button("Clear coins"))
            {
                BubblesPlayer.Instance.SetCoinsCount(0);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Inc stars"))
            {
                BubblesPlayer.Instance.AddStars(1);
            }

            if (GUILayout.Button("Dec stars"))
            {
                BubblesPlayer.Instance.AddStars(-1);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Inc life"))
            {
                BubblesPlayer.Instance.AddLifes(1);
            }

            if (GUILayout.Button("Dec life"))
            {
                BubblesPlayer.Instance.AddLifes(-1);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Add score 200"))
            {
                BubblesPlayer.Instance.AddScore(200);
            }

            if (GUILayout.Button("Clean infinite life"))
            {
                BubblesPlayer.Instance.CleanInfiniteLife();
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}