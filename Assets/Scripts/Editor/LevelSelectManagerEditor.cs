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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Save Data"))
                SaveManager.ClearSaveData();
            if (GUILayout.Button("Set Complete Data"))
                SaveManager.SaveProgress(_GetCompleteData());
            GUILayout.EndHorizontal();
        }
        if (SaveManager.doesSaveFileExist)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Focus Sewing Room"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Sewing]);
            if (GUILayout.Button("Focus Living Room"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Living]);
            if (GUILayout.Button("Focus Outside"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Outdoor]);
            if (GUILayout.Button("Focus Kitchen"))
                thisInstance.FocusMenu(thisInstance.roomMenus[(int)Room.Kitchen]);
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(10f);
        base.OnInspectorGUI();
        if (GUI.changed)
        {
            progStr = SaveManager.RetrieveProgress().progressToString;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private SaveData _GetCompleteData()
    {
        var newData = new SaveData();
        for (int i = 0; i < thisInstance.roomMenus.Length; ++i)
        {
            var levels = thisInstance.roomMenus[i].GetComponentsInChildren<LevelButton>(true);
            for (int ii = 0; ii < levels.Length; ++ii)
            {
                var ld = levels[ii].GetLevelData();
                if (!newData.levelHashTables[i].ContainsKey(ld.levelHash))
                {
                    ld.completed = true;
                    newData.levelHashTables[i].Add(ld.levelHash, ld);
                }
            }
        }
        return newData;
    }
}
