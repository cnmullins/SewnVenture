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
    public bool peck;
    public bool pecking;
    public bool returning;
    //target position of player and distance
    public Vector3 targetpos;
    public Vector3 targetdist;
    //for distance tracking
    public int distance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (peck)
        {
            pecking = true;
            peck = false;
            targetpos = player.transform.position;
            targetdist = player.transform.position - transform.position;
            distance = 100;
        }
        if (pecking)
        {
            transform.position += targetdist/100;
            distance -= 1;
            if (distance == 0)
            {
                Instantiate(Spawner, transform.position-Vector3.up, Quaternion.identity);
                pecking = false;
                returning = true;
            }
        }
        else if (returning)
        {
            transform.position -= targetdist / 100;
            distance += 1;
            if (distance == 100)
            {
                
                
                returning = false;
            }
        }
    }
}
