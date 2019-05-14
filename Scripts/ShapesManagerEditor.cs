using UnityEngine;
using UnityEditor;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
namespace Letra
{
    [CustomEditor(typeof(ShapesManager))]
    public class ShapesManagerEditor : Editor
    {
        private static bool showInstructions = true;

        public override void OnInspectorGUI()
        {
            ShapesManager shapesManager = (ShapesManager)target;//get the target

            EditorGUILayout.Separator ();
            #if !(UNITY_5 || UNITY_2017 || UNITY_2018_0 || UNITY_2018_1 || UNITY_2018_2)
                //Unity 2018.3 or higher
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Colors.cyanColor;
                EditorGUILayout.Separator();
                if (GUILayout.Button("Apply", GUILayout.Width(70), GUILayout.Height(30), GUILayout.ExpandWidth(false)))
                {
                    PrefabUtility.ApplyPrefabInstance(shapesManager.gameObject, InteractionMode.AutomatedAction);
                }
                GUI.backgroundColor = Colors.whiteColor;
                EditorGUILayout.EndHorizontal();
            #endif
            EditorGUILayout.Separator ();

            EditorGUILayout.Separator();
            EditorGUILayout.HelpBox("Follow the instructions below on how to add new shape", MessageType.Info);
            EditorGUILayout.Separator();

            showInstructions = EditorGUILayout.Foldout(showInstructions, "Instructions");
            EditorGUILayout.Separator();

            if (showInstructions)
            {
                EditorGUILayout.HelpBox("- Click on 'Add New Shape' button to add new Shape", MessageType.None);
                EditorGUILayout.HelpBox("- Click on 'Remove Last Shape' button to remove the lastest shape in the list", MessageType.None);
                EditorGUILayout.HelpBox("- Add Clip for each Shape and setup the Stars Time Period as you wish", MessageType.None);
                EditorGUILayout.HelpBox("- Click on 'Apply' button that located at the top to save your changes ", MessageType.None);
            }


            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Review Alphabet Tracing", GUILayout.Width(180), GUILayout.Height(25)))
            {
                Application.OpenURL(Links.packageURL);
            }

            GUI.backgroundColor = Colors.greenColor;

            if (GUILayout.Button("More Assets", GUILayout.Width(110), GUILayout.Height(25)))
            {
                Application.OpenURL(Links.indieStudioStoreURL);
            }
            GUI.backgroundColor = Colors.whiteColor;

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Colors.greenColor;

            if (GUILayout.Button("Add New Shape", GUILayout.Width(110), GUILayout.Height(20)))
            {
                shapesManager.shapes.Add(new ShapesManager.Shape() { starsTimePeriod = 15 });
            }

            GUI.backgroundColor = Colors.redColor;
            if (GUILayout.Button("Remove Last Shape", GUILayout.Width(150), GUILayout.Height(20)))
            {
                if (shapesManager.shapes.Count != 0)
                {
                    shapesManager.shapes.RemoveAt(shapesManager.shapes.Count - 1);
                }
            }

            GUI.backgroundColor = Colors.whiteColor;
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            for (int i = 0; i < shapesManager.shapes.Count; i++)
            {
                shapesManager.shapes[i].showContents = EditorGUILayout.Foldout(shapesManager.shapes[i].showContents, "Shape[" + i + "]");

                if (shapesManager.shapes[i].showContents)
                {
                    EditorGUILayout.Separator();
                    shapesManager.shapes[i].gamePrefab = EditorGUILayout.ObjectField("Game Prefab", shapesManager.shapes[i].gamePrefab, typeof(GameObject), true) as GameObject;
                    shapesManager.shapes[i].clip = EditorGUILayout.ObjectField("Clip", shapesManager.shapes[i].clip, typeof(AudioClip), true) as AudioClip;
                    shapesManager.shapes[i].starsTimePeriod = EditorGUILayout.IntSlider("Stars Time Period", shapesManager.shapes[i].starsTimePeriod, 5, 500);

                    EditorGUILayout.Separator();
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
                }
            }

            if (GUI.changed)
            {
                DirtyUtil.MarkSceneDirty();
            }
        }
    }
}