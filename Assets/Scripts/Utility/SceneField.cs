/*
SceneField.cs
Author: Someone on Unity Forums
Summary: Used for dragging and drop scene assets.
*/
using System;
 using UnityEngine;
 using Object = UnityEngine.Object;
 #if UNITY_EDITOR
 using UnityEditor;
 #endif
 
 namespace Utilities
 {
    [Serializable]
    public class SceneField
    {
        [SerializeField] private Object _sceneAsset;
        [SerializeField] private string _sceneName = "";

        public string sceneName => _sceneName;

        public int sceneHash => _sceneName.GetHashCode();

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.sceneName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            var sceneAsset = property.FindPropertyRelative("_sceneAsset");
            var sceneName = property.FindPropertyRelative("_sceneName");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (sceneAsset != null)
            {
                EditorGUI.BeginChangeCheck();
                var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                if (EditorGUI.EndChangeCheck())
                {
                    sceneAsset.objectReferenceValue = value;
                    if (sceneAsset.objectReferenceValue != null)
                    {
                        var scenePath = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                        var assetsIndex = scenePath.IndexOf("Assets", StringComparison.Ordinal) + 7;
                        var extensionIndex = scenePath.LastIndexOf(".unity", StringComparison.Ordinal);
                        scenePath = scenePath.Substring(assetsIndex, extensionIndex - assetsIndex);
                        sceneName.stringValue = scenePath;
                    }
                }
            }
            EditorGUI.EndProperty();
        }
    }
#endif
}