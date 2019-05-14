using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Fungus.EditorUtils
{
    [CustomEditor(typeof(SetGame),true)]
    public class GameEditor : CommandEditor
    {

        public override void DrawCommandGUI()
        {
            serializedObject.Update();

            SetGame t = target as SetGame;

            if(GUILayout.Button("Configure"))
            {
                OdinEditorWindow.InspectObject(t);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}