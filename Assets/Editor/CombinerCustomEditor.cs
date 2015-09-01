using UnityEngine;
using System.Collections;
using UnityEditor;

//
//Simple Editor script used to better display the Combiner classes member values and Output texture in the Editor.
//Not compiled into the final Build, only used to modify the Unity Editor.
//
//
[CustomEditor(typeof(Combiner))]
public class CombinerCustomEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		Combiner myScript = (Combiner)target;
		GUILayout.Space (20);
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label (myScript.Output, GUILayout.Width(256), GUILayout.Height(256));
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.Space (20);
		if (GUILayout.Button ("Add Module")) 
		{
			myScript.AddModule();
		}
		GUILayout.Space (10);
		if (GUILayout.Button ("Remove Module")) 
		{
			myScript.RemoveModule();
		}
		GUILayout.Space (10);
		if (GUILayout.Button ("Generate Output")) 
		{
			myScript.Generate();
		}
		GUILayout.Space (30);
	}
}
