using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    //the caths are the phases of cath, one is enabled and the other is disabled at all times
    public GameObject cathone;
    public GameObject cathtwo;
    //the 3 shop items are for different phases. 1 and 2 are for phase 1 whereas 3 is only for the 2nd phase evil route.
    public GameObject shop1;
    public GameObject shop2;
    public GameObject shop3;
    //opens up phase 2
    public GameObject curtain;

    //stores if the player is evil
    public bool evil = false;
    public bool savedevil;
    //connects the object to specific triggers to make the boss attack.
    public GameObject paw;
    public GameObject cath2;
    public GameObject tail2;
    // Start is called before the first frame update
    public void Update()
    {
        //if you're evil when transitioning phases the game triggers that
        //it will tell the future fight to  use evil attacks.
        if (evil)
        {
            shop3.SetActive(true);
            evil = false;
            savedevil = true;
            paw.GetComponent<CathPawTwo>().evil = true;
            tail2.GetComponent<CathTailTwo>().evil = true;
        }
    }
    //when the player touches this trigger, the phase changes.
    //all blocks go away, the shop changes and cath 1 is disabled.
    //if you're evil it brings that info forward aswell.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject destroyme in GameObject.FindGameObjectsWithTag("Moveable"))
            {
                if (destroyme.layer == 11 || destroyme.layer == 7)
                {
                    Destroy(destroyme.gameObject);
                }
                else
                {
                    destroyme.GetComponent<Blocks>().cost = 0;
                }
            }
            shop1.SetActive(false);
            shop2.SetActive(false);
            curtain.SetActive(true);
            if (evil)
            {
                shop3.SetActive(true);
            }
            other.GetComponent<Movement>().thread = 0;
            cathone.GetComponent<CathHead>().canattack2 = false;
        }
    }
    //when you leave the objects area you will enable cath 2, as to not start the fight too early.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("cathgomeow");
            cath2.GetComponent<CathHeadTwo>().canattack2 = true;
        }
    }
}
