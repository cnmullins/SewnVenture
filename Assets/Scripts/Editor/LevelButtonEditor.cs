using System.Collections;
using System.Collections.Generic;
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
        base.OnInspectorGUI();

        if (serializedObject.hasModifiedProperties)
        {
            thisInstance.RefreshValues();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
