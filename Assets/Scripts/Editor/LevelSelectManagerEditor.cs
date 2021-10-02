/*
LevelSelectManagerEditor.cs
Author: Christian Mullins
Date: 9/29/21
Summary: Editor class for the LevelSelectManager class.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSelectManager))]
public class LevelSelectManagerEditor : Editor
{
    LevelSelectManager thisInstance;
    SerializedObject targetObj;

    private void OnEnable() 
    {
        thisInstance = (LevelSelectManager)target;
        targetObj = new SerializedObject(thisInstance);
    }

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        /*
        if (GUI.changed)
        {

        }
        */
        serializedObject.ApplyModifiedProperties();
    }
}
