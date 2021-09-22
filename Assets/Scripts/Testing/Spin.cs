using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed = 10f;
    public Vector3 outRot;

    void Update()
    {
        transform.Rotate(outRot.normalized * spinSpeed * Time.deltaTime);
    }
}
