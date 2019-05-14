using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mkey
{
    [CustomEditor(typeof(GameObjectsSet))]
    public class GameObjectsSetEditor : Editor
    {
        GameObjectsSet mS;
        private ReorderableList regBO;
        string[] boChoises;

        bool showDefault = true;
        private void OnEnable()
        {
            regBO = new ReorderableList(serializedObject, serializedObject.FindProperty("payTable"),
                 true, true, true, true);

            regBO.onRemoveCallback += RemoveCallback;
            regBO.drawElementCallback += OnDrawCallback;
            regBO.onAddCallback += OnAddCallBack;
            regBO.onSelectCallback += OnSelectCallBack;
            regBO.drawHeaderCallback += DrawHeaderCallBack;
            regBO.onChangedCallback += OnChangeCallBack;
            //  reg.onAddDropdownCallback += OnAddDropDownCallBack;
        }

        private void OnDisable()
        {
            if (regBO != null)
            {
                regBO.onRemoveCallback -= RemoveCallback;
                regBO.drawElementCallback -= OnDrawCallback;
                regBO.onAddCallback -= OnAddCallBack;
                regBO.onSelectCallback -= OnSelectCallBack;
                regBO.drawHeaderCallback -= DrawHeaderCallBack;
                regBO.onChangedCallback -= OnChangeCallBack;
                regBO.onAddDropdownCallback -= OnAddDropDownCallBack;
            }
        }

        public override void OnInspectorGUI()
        {
            mS = (GameObjectsSet)target;

            #region default
            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            if (showDefault = EditorGUILayout.Foldout(showDefault, "Default Inspector"))
            {
                DrawDefaultInspector();
            }
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
            #endregion default
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }


        #region regular list CallBacks
        private void OnAddDropDownCallBack(Rect buttonRect, ReorderableList list)
        {
        }

        private void OnChangeCallBack(ReorderableList list)
        {
            // Debug.Log("onchange");
        }

        private void DrawHeaderCallBack(Rect rect)
        {
            // EditorGUI.LabelField(rect, "Regular");
        }

        private void OnSelectCallBack(ReorderableList list)
        {
        }

        private void OnAddCallBack(ReorderableList list)
        {
            //if (slotController == null || slotController.slotGroupsBeh == null || slotController.slotGroupsBeh.Length == 0) return;
            //if (slotController.payTable != null && slotController.payTable.Count > 0)
            //{
            //    slotController.payTable.Add(new PayLine(slotController.payTable[slotController.payTable.Count - 1]));
            //}
            //else
            //    slotController.payTable.Add(new PayLine(slotController.slotGroupsBeh.Length));
            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            //// Debug.Log("OnAddCallBack");
        }

        private void OnDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.LabelField(rect, (index + 1).ToString());
            //var element = payTableList.serializedProperty.GetArrayElementAtIndex(index);
            //rect.y += 2;
            //rect.x += 20;
            //ShowPayLine(choises, rect, 5, 70, 20, element, slotController.payTable[index]);

        }

        private void RemoveCallback(ReorderableList list)
        {
            //if (EditorUtility.DisplayDialog("Warning!", "Are you sure?", "Yes", "No"))
            //{
            //    slotController.payTable.RemoveAt(list.index); //ReorderableList.defaultBehaviours.DoRemoveButton(list);
            //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            //}

        }

        #endregion payTableList  CallBacks

        #region showProperties
        private void ShowProperties(string[] properties, bool showHierarchy)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(properties[i]), showHierarchy);
            }
        }

        private void ShowPropertiesBox(string[] properties, bool showHierarchy)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            ShowProperties(properties, showHierarchy);
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }

        private void ShowPropertiesBoxFoldOut(string bName, string[] properties, ref bool fOut, bool showHierarchy)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            if (fOut = EditorGUILayout.Foldout(fOut, bName))
            {
                ShowProperties(properties, showHierarchy);
            }
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }

        private void ShowReordListBoxFoldOut(string bName, ReorderableList rList, ref bool fOut)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            if (fOut = EditorGUILayout.Foldout(fOut, bName))
            {
                rList.DoLayoutList();
            }
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }
        #endregion showProperties

        #region array
        public static void ShowList(SerializedProperty list, bool showListSize = true, bool showListLabel = true)
        {
            if (showListLabel)
            {
                EditorGUILayout.PropertyField(list);
                EditorGUI.indentLevel += 1;
            }
            if (!showListLabel || list.isExpanded)
            {
                if (showListSize)
                {
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                }
                for (int i = 0; i < list.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                }
            }
            if (showListLabel)
            {
                EditorGUI.indentLevel -= 1;
            }
        }
        #endregion array

        #region showChoise EditorGuiLayOut
        private void ShowHeartChoiseLO(string[] sChoise)
        {
            EditorGUILayout.BeginHorizontal();
            ShowProperties(new string[] { "useAsHeartMajor" }, false);
            /*
            if (slotController.useAsHeartMajor)
            {
                //  EditorGUILayout.LabelField("Select heart: ");
                int choiseIndex = slotController.heart_id;
                int oldIndex = choiseIndex;
                choiseIndex = EditorGUILayout.Popup(choiseIndex, sChoise);
                slotController.heart_id = choiseIndex;
                if (oldIndex != choiseIndex) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            */
            EditorGUILayout.EndHorizontal();
        }

        private void ShowChoiseLO(string[] choises)
        {
            int _choiceIndex = 0;
            if (choises == null || choises.Length == 0) return;
            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, choises);
            Debug.Log("choice: " + _choiceIndex);
            EditorUtility.SetDirty(target);
        }
        #endregion showChoise EditorGuiLayOut

        #region showChoise EditorGui
        private void ShowChoise(string[] choises, Rect rect, float width, float height, float dx, float dy, int index)
        {
            /*
            if (choises == null || choises.Length == 0 || pLine.line == null || pLine.line.Length == 0 || pLine.line.Length <= index) return;

            int choiseIndex = pLine.line[index] + 1; // any == 0;
            int oldIndex = choiseIndex;
            choiseIndex = EditorGUI.Popup(new
                Rect(rect.x + dx, rect.y + dy, width, height),
                //  "Icon: ",
                choiseIndex, choises);
            pLine.line[index] = choiseIndex - 1;
            if (oldIndex != choiseIndex) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            */

        }

        private void ShowLine(string[] choises, Rect rect, int count, float width, float height, SerializedProperty element)
        {
            // if (pLine == null) return;

            for (int i = 0; i < count; i++)
            {
                //  ShowChoise(choises, rect, width, height, i * width + i * 1.0f, 0, pLine, i);
            }
            float dx = rect.x + count * width + count * 1.0f;
            float w = 40;
            EditorGUI.LabelField(new Rect(dx, rect.y, w, EditorGUIUtility.singleLineHeight), "Pay: ");
            dx += w;
            w = 50;

            EditorGUI.PropertyField(new Rect(dx, rect.y, w, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("pay"), GUIContent.none);
            dx += w; w = 70;
            EditorGUI.LabelField(new Rect(dx, rect.y, w, EditorGUIUtility.singleLineHeight), "FreeSpins:");
            dx += w; w = 50;
            EditorGUI.PropertyField(new Rect(dx, rect.y, w, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("freeSpins"), GUIContent.none);
        }
        #endregion showChoise EditorGui

    }
}
/*
   ReorderableList CreateList(SerializedObject obj, SerializedProperty prop) // https://pastebin.com/WhfRgcdC
        {
            ReorderableList list = new ReorderableList(obj, prop, true, true, true, true);

            list.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Sprites");
            };

            List<float> heights = new List<float>(prop.arraySize);

            list.drawElementCallback = (rect, index, active, focused) => {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                Sprite s = (element.objectReferenceValue as Sprite);

                bool foldout = active;
                float height = EditorGUIUtility.singleLineHeight * 1.25f;
                if (foldout)
                {
                    height = EditorGUIUtility.singleLineHeight * 5;
                }

                try
                {
                    heights[index] = height;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.LogWarning(e.Message);
                }
                finally
                {
                    float[] floats = heights.ToArray();
                    Array.Resize(ref floats, prop.arraySize);
                    heights = new List<float> (floats);
                }

                float margin = height / 10;
                rect.y += margin;
                rect.height = (height / 5) * 4;
                rect.width = rect.width / 2 - margin / 2;

                if (foldout)
                {
                    if (s)
                    {
                        EditorGUI.DrawPreviewTexture(rect, s.texture);
                    }
                }
                rect.x += rect.width + margin;
                EditorGUI.ObjectField(rect, element, GUIContent.none);
            };

            list.elementHeightCallback = (index) => {
                Repaint();
                float height = 0;

                try
                {
                    height = heights[index];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.LogWarning(e.Message);
                }
                finally
                {
                    float[] floats = heights.ToArray();
                    Array.Resize(ref floats, prop.arraySize);
                    heights = new List<float>(floats);
                }

                return height;
            };

            list.drawElementBackgroundCallback = (rect, index, active, focused) => {
                rect.height = heights[index];
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, new Color(0.33f, 0.66f, 1f, 0.66f));
                tex.Apply();
                if (active)
                    GUI.DrawTexture(rect, tex as Texture);
            };

            list.onAddDropdownCallback = (rect, li) => {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Element"), false, () => {
                    serializedObject.Update();
                    li.serializedProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();
                });

                menu.ShowAsContext();

                float[] floats = heights.ToArray();
                Array.Resize(ref floats, prop.arraySize);
                heights = new List<float>(floats);
            };

            return list;
        }
 */
