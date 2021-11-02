using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriHead : MonoBehaviour
{
    //reference to mori's body
    public GameObject Mori;
    //prefab for a silverfish hole
    public GameObject Spawner;
    // player ref
    public GameObject player;
    //tells mori to attack
    //these are the phases of the attack;
    public bool peck;
    public bool pecking;
    public bool returning;
    public bool waiting;
    //target position of player and distance
    public Vector3 targetpos;
    public Vector3 targetdist;
    //for distance tracking
    public int distance;
    //this is a warning for the player
    public GameObject warn;
    public float waittime = 1.5f;
    public bool sewn;
    public bool sewable; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (returning)
        {
            transform.position -= targetdist / 100;
            distance += 1;
            if (distance == 100)
            {
                returning = false;
                Mori.GetComponent<MoriBody>().canpeck = true;

            }
        }
        else if (waiting)
        {
            waittime -= 1 * Time.deltaTime;
            if (waittime <= 0 && !sewn)
            {
                waittime = 1.5f;
                waiting = false;
                returning = true;
                Instantiate(Spawner, transform.position - Vector3.up, Quaternion.identity);
                if (sewable)
                {
                    gameObject.layer = 0;
                }
            }
        }
        else if (pecking)
        {
            transform.position += targetdist / 100;
            distance -= 1;
            if (distance == 0)
            {

                pecking = false;
                waiting = true;
                if (sewable)
                {
                    gameObject.layer = 8;
                }
            }
        }
        else if (peck)
        {
            Instantiate(warn, player.transform.position,player.transform.rotation);
            pecking = true;
            peck = false;
            targetpos = player.transform.position;
            targetdist = player.transform.position - transform.position;
            distance = 100;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeldDown" && this.gameObject.layer == 8)
        {
            Debug.Log("ow");
            other.transform.parent.gameObject.layer = 2;
            other.transform.parent.position = transform.position + Vector3.forward + Vector3.left;
            sewn = true;
            Mori.GetComponent<MoriBody>().freefeet -= 1;
            //gameObject.layer = 0;
            //gameObject.tag = "Untagged";
        }

    }
}
