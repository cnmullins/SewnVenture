using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAggregator : MonoBehaviour
{
    public MeshRenderer[] meshRnds { get; private set; }

    void OnEnable() 
    {
        //get meshes
        //meshRnds = GetComponensInChildren<MeshRenderer>();
    }

    public void SetColorTint(in Color tint)
    {

    }

    public IEnumerator FadeOutMesh(float timer)
    {
        return null;
    }
}
