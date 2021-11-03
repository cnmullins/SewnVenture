using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCube : MonoBehaviour
{
    public float falldist;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (falldist > 0)
        {
            transform.position -= Vector3.up * 10 * Time.deltaTime;
            falldist -= 10 * Time.deltaTime;
        }
        if (falldist < -1)
        {
            transform.position += Vector3.up * 10 * Time.deltaTime;
            falldist += 10 * Time.deltaTime;
        }
    }
}
