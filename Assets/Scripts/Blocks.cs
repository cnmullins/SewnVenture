using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public int sewn = 0;
    public bool stuck;
    public int cost;
    
    public void Update()
    {
        if (stuck && sewn == 0)
        {
            stuck = false;
            gameObject.tag = "Moveable";
        }
        if (!stuck && sewn > 0)
        {
            stuck = true;
            gameObject.tag = "Untagged";
        }
    }
}
