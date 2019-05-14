using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Fungus.EditorUtils
{
    [CustomEditor(typeof(FungusChangeGameState), true)]
    public class ChangeGameStateEditor : GameEditor
    {

        protected SerializedProperty targetFlowchartProp;
        protected SerializedProperty succededBlockProp;
        protected SerializedProperty failedBlockProp;

        protected void OnEnable()
        {
            if (NullTargetCheck()) // Check for an orphaned editor instance
                return;

            targetFlowchartProp = serializedObject.FindProperty("targetFlowchart");
            succededBlockProp = serializedObject.FindProperty("succededBlock");
            failedBlockProp = serializedObject.FindProperty("failedBlock");
        }


        public override void DrawCommandGUI()
        {
            serializedObject.Update();

            FungusChangeGameState t = target as FungusChangeGameState;
            Flowchart flowchart = t.GetFlowchart();

            if (flowchart != null)
            {
                BlockEditor.BlockField(succededBlockProp,
                                       new GUIContent("Succeded Block", "Block to call"),
                                       new GUIContent("<None>"),
                                       flowchart);

                BlockEditor.BlockField(failedBlockProp,
                                      new GUIContent("Failed Block", "Block to call"),
                                      new GUIContent("<None>"),
                                      flowchart);
            }
            serializedObject.ApplyModifiedProperties();
            base.DrawCommandGUI();
        }
    }
}