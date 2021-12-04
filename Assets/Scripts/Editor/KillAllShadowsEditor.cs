using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KillAllShadows))]
public class KillAllShadowsEditor : Editor
{
    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Kill All Shadows"))
        {
            foreach (var rends in GameObject.FindObjectsOfType<MeshRenderer>(true))
            {
                rends.receiveShadows = false;
            }
        }
    }
}
