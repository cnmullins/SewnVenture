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
        bool updated = false;
        if (GUILayout.Button("Update All Buttons"))
        {
            foreach (var t in GameObject.FindObjectsOfType<LevelButton>())
            {
                t.EditorRefresh();
            }
            updated = true;
        }

        base.OnInspectorGUI();
        if (GUI.changed && !updated)
        {
            thisInstance.EditorRefresh();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
