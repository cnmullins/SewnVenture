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
    //for when mori is down, what phase is it in?
    public int down;
    //mori's block is part of the battle
    public GameObject moriblock;
    //tornado is used to pull in sewy and delete silverfish;
    public GameObject tornado;
    public float waiting;
    //for defeat
    public GameObject standon;
    public Vector3 blockpos;
    public GameObject swing;
    // Start is called before the first frame update
    void Start()
    {
        blockpos = standon.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waiting > 0)
        {
            waiting -= Time.fixedDeltaTime;
        }
        if (Phealth > 0 && health > 0 && canpeck && canflap == 0 && waiting <= 0)
        {
            Debug.Log("b" + ((Random.Range(0, peckchance - (peckchance * ((-freefeet * 2 + 4 + ((6 - (health + Phealth)) / 2)) / 10)))) < 1));
            Debug.Log("a" + (Random.Range(0, flapchance - (flapchance * ((6 - (health + Phealth)) / 10))) < 1));
            if ((Random.Range(0, peckchance - (peckchance * ((-freefeet * 2 + 4 + ((6 - (health + Phealth)) / 2)) / 10)))) < 1)
            {
                canpeck = false;
                head.GetComponent<MoriHead>().peck = true;
                Debug.Log("peck");
            }
            else if (Random.Range(0, flapchance - (flapchance * ((6 - (health + Phealth)) / 10))) < 1)
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
        //this checks if the player has moved the block
        if (Vector3.Distance(blockpos, standon.transform.position) > 1)
        {
            fly();
        }
        
    }
    //taking damage resets a lot of stuff, such as putting the feet back and such.
    public void Damaged()
    {
        //release the feet and head from being sewn so mori can attack again.
        head.GetComponent<MoriHead>().sewn = false;
        freefeet = 2;
        footR.gameObject.layer = 8;
        footL.gameObject.layer = 8;
        //remove all sewn objects to prevent the player from using them.
        Destroy(footR.GetComponent<Morifeet>().sewme.transform.parent.gameObject);
        Destroy(footL.GetComponent<Morifeet>().sewme.transform.parent.gameObject);
        Destroy(head.GetComponent<MoriHead>().sewme.transform.parent.gameObject);
        footR.GetComponent<Morifeet>().sewn = false;
        footL.GetComponent<Morifeet>().sewn = false;
        foreach (GameObject sewd in GameObject.FindGameObjectsWithTag("HeldDown"))
        {
            Destroy(sewd.transform.parent.gameObject);
        }
        //have the block leave so the player cannot use it anymore.
        moriblock.layer = 2;
        moriblock.GetComponent<MoriCube>().falldist = -20;

        //increase the wind attack's power
        wingR.GetComponent<Moriwing>().windset += 1;
        wingL.GetComponent<Moriwing>().windset += 1;
        wingR.GetComponent<Moriwing>().windchance -= 5;
        wingR.GetComponent<Moriwing>().windchance -= 5;
        tornado.GetComponent<Tornado>().sucking = 4;
        waiting = 5;
        if (health == 0)
        {
            fly();
        }
        if (Phealth == 0)
        {
            standon.GetComponent<Blocks>().sewn = 0;
        }
    }
    public void fly()
    {
        swing.SetActive(true);
    }
}
