using UnityEngine;
using System.Collections;
using UnityEditor;

//Unused Script.
//
//Originally I wanted to add a "-" button in the Unity Editor inside each Module so that you could just Click the button
//to Delete the Module from the Combiner's List. But it turns out you can't do this with a Class not derived from UnityEngine.Object
//and I didn't feel it was worth messing with further...
//
//
[CustomEditor(typeof(BlendModule))]
public class BlendModuleCustomEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//BlendModule myScript = (BlendModule)target;
		if (GUILayout.Button ("-")) 
		{
			//myScript.Generate();
		}
	}
}
