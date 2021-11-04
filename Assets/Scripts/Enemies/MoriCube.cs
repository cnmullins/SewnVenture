using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCube : MonoBehaviour
{
    public float falldist;
    public GameObject myson;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //the cube will move up or down if it isnt needed.
        //it can attack the player while falling down.
        if (falldist > 0)
        {
            myson.SetActive(true);
            transform.position -= Vector3.up * 10 * Time.deltaTime;
            falldist -= 10 * Time.deltaTime;
        }
        if (falldist < -1)
        {
            myson.SetActive(false);
            transform.position += Vector3.up * 10 * Time.deltaTime;
            falldist += 10 * Time.deltaTime;
        }
    }
}
