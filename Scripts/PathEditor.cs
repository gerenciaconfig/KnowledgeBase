using UnityEngine;
using UnityEditor;
using System.Collections;
using TracingPackage;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
{
	[CustomEditor(typeof(Path))]
	public class PathEditor : Editor {

		public override void OnInspectorGUI ()
		{
			Path path = (Path)target;//get the target

			EditorGUILayout.Separator ();
			#if !(UNITY_5 || UNITY_2017 || UNITY_2018_0 || UNITY_2018_1 || UNITY_2018_2)
				//Unity 2018.3 or higher
				EditorGUILayout.BeginHorizontal();
				GUI.backgroundColor = Colors.cyanColor;
				EditorGUILayout.Separator();
				if (GUILayout.Button("Apply", GUILayout.Width(70), GUILayout.Height(30), GUILayout.ExpandWidth(false)))
				{
					PrefabUtility.ApplyPrefabInstance(path.gameObject, InteractionMode.AutomatedAction);
				}
				GUI.backgroundColor = Colors.whiteColor;
				EditorGUILayout.EndHorizontal();
			#endif
			EditorGUILayout.Separator ();
			
			path.fillMethod = (Path.FillMethod)EditorGUILayout.EnumPopup ("Fill Method",path.fillMethod);
			if (path.fillMethod == Path.FillMethod.Linear) {
				path.type = (Path.ShapeType)EditorGUILayout.EnumPopup ("Type", path.type);
				path.offset = EditorGUILayout.Slider ("Angle Offset", path.offset, -360, 360);
				path.flip = EditorGUILayout.Toggle ("Flip Direction", path.flip);
			} else if (path.fillMethod == Path.FillMethod.Radial) {
				path.quarterRestriction = EditorGUILayout.Toggle ("Quarter Restriction",path.quarterRestriction) ;
				path.radialAngleOffset = EditorGUILayout.Slider ("Radial Offset",path.radialAngleOffset,-360,360) ;
			}

			path.completeOffset = EditorGUILayout.Slider ("Complete Offset", path.completeOffset,0,1);
			path.firstNumber = EditorGUILayout.ObjectField ("First Number",path.firstNumber, typeof(Transform)) as Transform;
			if (path.fillMethod != Path.FillMethod.Point) 
				path.secondNumber = EditorGUILayout.ObjectField ("Second Number",path.secondNumber, typeof(Transform)) as Transform;

		}
	}	
}