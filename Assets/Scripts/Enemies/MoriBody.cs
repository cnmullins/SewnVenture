using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriBody : MonoBehaviour
{
    public int freefeet = 2;
    //chances to act
    public int peckchance;
    public int flapchance;
    //these two determine when mori can act
    public bool canpeck;
    public int canflap;
    //parts of mori
    public GameObject head;
    public GameObject footL;
    public GameObject footR;
    public GameObject wingL;
    public GameObject wingR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canpeck && canflap == 0)
        {
            if (Random.Range(0, peckchance) == 0)
            {
                canpeck = false;
                head.GetComponent<MoriHead>().peck = true;
            }
            else if (Random.Range(0, flapchance) == 0)
            {
                canflap = 2;
                wingL.GetComponent<Moriwing>().doflap = true;
                wingR.GetComponent<Moriwing>().doflap = true;
            }
        }
       
            
       
    }
}
