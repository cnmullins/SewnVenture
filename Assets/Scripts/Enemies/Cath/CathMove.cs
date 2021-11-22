using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathMove : MonoBehaviour
{
    //the center is he center of the arena
    public GameObject center;
    public GameObject player;
    //these are meant to detect how close the mover is to the center
    public Vector3 movethisdist;
    public int timesmoved = 0;
    public float debugtimer = 5;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if the mover is touching the player, the player will be stuck.
        //once the player is hit by cath's paw OR some time passes (to prevent a softlock)
        //the player will be moved to the middle with mover.
        //this is to transition phases if the player is going the evil route.
        if (Vector3.Distance(transform.position, player.transform.position) < 10)
        {

            player.transform.position = transform.position;
            debugtimer -= Time.fixedDeltaTime;
            if (debugtimer <= 0)
            {
                timesmoved = 0;
                movethisdist = (center.transform.position - transform.position);
            }
        }
            if (timesmoved > -1)
        {
            debugtimer = 999;
            if (Vector3.Distance(transform.position, player.transform.position) < 10)
            {

                player.transform.position = transform.position;
                transform.position += movethisdist / 20f;
                timesmoved += 1;
                if (timesmoved == 20)
                {
                    center.GetComponent<DestroyBlock>().evil = true;
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeldDown")
        {
            timesmoved = 0;
            movethisdist = (center.transform.position - transform.position);
        }
    }
}
