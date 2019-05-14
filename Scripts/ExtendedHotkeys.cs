

//Hotkeys:
//Create a 2D Bone by pressing Shift + 1

using UnityEngine;
using UnityEditor;
using Anima2D;
using System.Reflection;

public class ExtendedHotkeys : ScriptableObject {
	public static GameObject go;
	public static Vector3 goPos;

#if UNITY_EDITOR
    //Create a 2D Bone by pressing Shift + 1
    [MenuItem("ArcoLabs Hotkeys/Create 2D Bone #_1")]
    static void Create2DBone()
    {
        GameObject bone = new GameObject("New bone");
        Bone2D boneComponent = bone.AddComponent<Bone2D>();

        Undo.RegisterCreatedObjectUndo(bone, "Create bone");

        bone.transform.position = GetDefaultInstantiatePosition();

        GameObject selectedGO = Selection.activeGameObject;
        if (selectedGO)
        {
            bone.transform.parent = selectedGO.transform;

            Vector3 localPosition = bone.transform.localPosition;
            localPosition.z = 0f;

            bone.transform.localPosition = localPosition;
            bone.transform.localRotation = Quaternion.identity;
            bone.transform.localScale = Vector3.one;

            Bone2D selectedBone = selectedGO.GetComponent<Bone2D>();

            if (selectedBone)
            {
                if (!selectedBone.child)
                {
                    bone.transform.position = selectedBone.endPosition;
                    selectedBone.child = boneComponent;
                }
            }
        }

        Selection.activeGameObject = bone;
    }





    static Vector3 GetDefaultInstantiatePosition()
    {
        Vector3 result = Vector3.zero;
        if (SceneView.lastActiveSceneView)
        {
            if (SceneView.lastActiveSceneView.in2DMode)
            {
                result = SceneView.lastActiveSceneView.camera.transform.position;
                result.z = 0f;
            }
            else
            {
                PropertyInfo prop = typeof(SceneView).GetProperty("cameraTargetPosition", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                result = (Vector3)prop.GetValue(SceneView.lastActiveSceneView, null);
            }
        }
        return result;
    }

    static void AfterCreation(){
		goPos = SceneView.currentDrawingSceneView.camera.transform.TransformPoint (Vector3.forward * 1.1f);
		go.transform.position = goPos;
		Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
		Selection.activeObject = go;
	}
#endif
}