using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morifeet : MonoBehaviour
{
    //reference to the body
    public GameObject mori;
    //if the foot is sewn down
    public bool sewn;
    public GameObject sewme;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeldDown" && !sewn)
        {
            Debug.Log("ow");
            other.transform.parent.gameObject.layer = 2;
            sewn = true;
            mori.GetComponent<MoriBody>().freefeet -= 1;
            gameObject.layer = 0;
            sewme = other.gameObject;
        }

    }
}
