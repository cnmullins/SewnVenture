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
    // stat for hp of mori
    public int health = 3;
    // stat for mori's special threads
    public int Phealth = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canpeck && canflap == 0)
        {
            if ((Random.Range(0, peckchance-(peckchance*((-freefeet*2+4+ ((6 - (health + Phealth))/2)) /10)))) < 1)
            {
                canpeck = false;
                head.GetComponent<MoriHead>().peck = true;
                Debug.Log("peck");
            }
            else if (Random.Range(0, flapchance- (flapchance * ((6-(health+Phealth)) / 10))) < 1)
            {
                canflap = 2;
                wingL.GetComponent<Moriwing>().doflap = true;
                wingR.GetComponent<Moriwing>().doflap = true;
                Debug.Log("flap");
            }
        }
       if (freefeet == 0)
        {
            head.GetComponent<MoriHead>().sewable = true;
        }


    }
}
