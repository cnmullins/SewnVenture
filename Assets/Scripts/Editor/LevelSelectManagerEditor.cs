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
        if (GUILayout.Button("Data Path"))
            Debug.Log("SavedData goes to: " + Application.persistentDataPath);
        if (GUILayout.Button("Focus Sewing Room"))
            thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Sewing]);
        if (GUILayout.Button("Focus Living Room"))
            thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Living]);
        if (GUILayout.Button("Focus Outside"))
            thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Outdoor]);
        base.OnInspectorGUI();
        /*
        if (GUI.changed)
        {

        }
        */
        serializedObject.ApplyModifiedProperties();
    }
}
