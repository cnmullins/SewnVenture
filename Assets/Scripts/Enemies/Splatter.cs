/*
Splatter.cs
Author: Christian Mullins
Date: 11/2/2021
Summary: Triggers bug splatter.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour
{
    public Color[] colors;
    public Material[] splatMats;

    public void Splat()
    {
        int rand = Random.Range(0, splatMats.Length);
        if (transform.parent.transform.parent != null)
        {
            var blockPar = transform.parent.transform.parent;
            transform.SetParent(blockPar, true);
        }
        GetComponent<MeshRenderer>().material = splatMats[rand];
        var tempMat = GetComponent<MeshRenderer>().material;
        rand = Random.Range(0, colors.Length);
        tempMat.color = colors[rand];
    }
}
