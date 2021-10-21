/*
LevelButtonEditor.cs
Author: Christian Mullins
Date: 9/28/21
Summary: Editor class for the LevelButton class.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelButton))]
public class LevelButtonEditor : Editor
{
    LevelButton thisInstance;
    SerializedObject targetObj;

    private void OnEnable()
    {
        thisInstance = (LevelButton)target;
        targetObj = new SerializedObject(thisInstance);
    }

    public override void OnInspectorGUI() 
    {
        
        /*
            Create drop down menu of the scene list
        */
        if (GUILayout.Button("Update All Buttons"))
        {
            /*
            foreach (var t in GameObject.FindObjectsOfType<LevelButton>())
            {
                t.RefreshValues();
                t.UpdateLevelPath();
            }
            */
            Debug.LogWarning("Umm, this button is broken bud.");
        }

        base.OnInspectorGUI();
        /*
        if (GUI.changed)
        {
            //TODO: create new function to replace the parameterless version of RefreshValues()
            thisInstance.RefreshValues();
        }
        */
        serializedObject.ApplyModifiedProperties();
    }
}
