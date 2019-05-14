//v1_0(05062018)

using UnityEngine;
using UnityEditor;

namespace Mkey
{
    [CustomEditor(typeof(PFAnchor))]
    public class PFAnchorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            PFAnchor anch = (PFAnchor)target;
            EditorGUILayout.BeginHorizontal("box");

			if (GUILayout.Button ("Debug Path"))
            {
                anch.DebugPath();
			}

            EditorGUILayout.EndHorizontal();

        }
    }
}