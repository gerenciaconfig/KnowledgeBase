using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mkey
{
    [CustomEditor(typeof(SpriteText))]
    public class SpriteTextEditor : Editor 
    {
        SpriteText spriteText;

        public override void OnInspectorGUI()
        {
          //  base.OnInspectorGUI();
            spriteText = (SpriteText)target;
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sortingOrder"), false);
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spriteText, "sorting order");
                EditorUtility.SetDirty(spriteText);
                spriteText.RefreshSort();
            }

            EditorGUI.BeginChangeCheck();
            spriteText.sortingLayerID = DrawSortingLayersPopup("Sorting layer: ", spriteText.sortingLayerID);
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spriteText, "sorting layer");
                EditorUtility.SetDirty(spriteText);
                spriteText.RefreshSort();
            }
        }

        /// <summary>
        /// Draws a popup of the project's existing sorting layers.
        /// </summary>
        ///<param name="layerID">The internal layer id, can be assigned to renderer.SortingLayerID to change sorting layers.</param>
        /// <returns></returns>
        public static int DrawSortingLayersPopup(string label, int layerID)
        {
            /*
              https://answers.unity.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html
            */

            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label);
            }
            var layers = SortingLayer.layers;
            var names = layers.Select(l => l.name).ToArray();
            if (!SortingLayer.IsValid(layerID))
            {
                layerID = layers[0].id;
            }
            var layerValue = SortingLayer.GetLayerValueFromID(layerID);
            var newLayerValue = EditorGUILayout.Popup(layerValue, names);
            EditorGUILayout.EndHorizontal();
            SetSceneDirty(newLayerValue != layerValue);
            return layers[newLayerValue].id;
        }

        private static void SetSceneDirty(bool dirty)
        {
            if (dirty)
            {
                if (!SceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
            }
        }
    }
}