using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public int sewn = 0;
    public bool stuck;
    
    public void Update()
    {
        if (stuck && sewn == 0)
        {
            stuck = false;
            gameObject.layer = 7;
        }
        if (!stuck && sewn > 0)
        {
            stuck = true;
            gameObject.layer = 0;
        }
    }
}
