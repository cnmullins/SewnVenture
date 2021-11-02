using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moriwing : MonoBehaviour
{
    //this is to refer to mori itself
    public GameObject mori;
    //these are to tell the wing to flap and determine if iti s flapping
    public bool doflap;
    public bool flapping;
    //these are to increase the amount of winds every time
    //windleft is how much wind will be made.
    public int windleft;
    public int windset;
    //windchance is a chance every frame a wind happens;
    public int windchance;
    //the prefab for wind
    public GameObject wind;
    public GameObject feather;
    //this is the next wind you're going to blow
    public GameObject nextwind;
    // Update is called once per frame
    void Update()
    {
        if (doflap)
        {
            flapping = true;
            windleft = windset;
            doflap = false;
        }
        if (flapping)
        {
            if (Random.Range(0,windchance) == 0)
            {
                nextwind = Instantiate(wind, new Vector3(transform.position.x,3.5f,transform.position.z), transform.rotation);
                nextwind.transform.Rotate(0, Random.Range(-30, 30), 0);
                nextwind.transform.position += transform.right * Random.Range(-3f, 3f);
                windleft -= 1;
                if (windleft == 0)
                {
                    flapping = false;
                    mori.GetComponent<MoriBody>().canflap -= 1;
                }
                if (Random.Range(0, 7) == 0)
                {
                    Instantiate(feather, new Vector3(transform.position.x, 4.5f, transform.position.z), transform.rotation);
                }
            }
        }
    }
}
