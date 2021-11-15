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
    string progStr;

    private void OnEnable() 
    {
        thisInstance = (LevelSelectManager)target;
        targetObj = new SerializedObject(thisInstance);
        progStr = SaveManager.RetrieveProgress().progressToString;
    }

    public override void OnInspectorGUI()
    {
        //update progress string
        if (!thisInstance.debugMode)
        {
            //output progress
            GUILayout.Label("Level Progress:\n\t" + progStr);
            GUILayout.Space(10f);
            if (GUILayout.Button("Clear Save Data"))
                SaveManager.ClearSaveData();
        }
        
        if (SaveManager.doesSaveFileExist)
        {
            if (GUILayout.Button("Focus Sewing Room"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Sewing]);
            if (GUILayout.Button("Focus Living Room"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Living]);
            if (GUILayout.Button("Focus Outside"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Outdoor]);
        }
        GUILayout.Space(10f);
        base.OnInspectorGUI();
        if (GUI.changed)
        {
            progStr = SaveManager.RetrieveProgress().progressToString;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
