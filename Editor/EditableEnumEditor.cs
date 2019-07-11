using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;					// Editor, AssetDatabase
using System;						// Array

[CustomEditor(typeof(EditableEnum))]
[CanEditMultipleObjects]
public class EditableEnumEditor : Editor
{
    public override void OnInspectorGUI(){
    	
    	EditableEnum[] enums = Array.ConvertAll(serializedObject.targetObjects,
    		(UnityEngine.Object o) => { return (EditableEnum)o; } );

    	int updateCount = 0;
    	foreach(EditableEnum e in enums){
    		if(e.WillOverwrite){
    			++updateCount;
    		}
    	}

    	string buttonText = "";
    	if(updateCount == enums.Length){
    		buttonText = "Update";
    	}
    	else if(updateCount == 0){
    		buttonText = "Generate";
    	}
    	else{
    		buttonText = "Update and Generate";
    	}

    	if(GUILayout.Button(buttonText)){
            foreach(EditableEnum e in enums){
            	e.UpdateEnumerator();
            }
        }

        DrawDefaultInspector();
    }
}
