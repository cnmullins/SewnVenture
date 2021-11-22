using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathPawTwo : MonoBehaviour
{
    public GameObject Head;
    //is it the leftmost paw or the rightmost paw?
    public bool right;
    public GameObject Sewy;
    public GameObject holder;
    //used for determining when to attack
    public bool attack2;
    public bool attack;
    public bool attacking;
    public float attacktime;
    public float waittime;
    //used for animation of the attack.
    public Animator myanim;

    private void Start()
    {
        Sewy = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
       /* if (!attacking && !attack)
        {
            if (right)
            {
                if (Sewy.transform.position.x > -5)
                {
                    holder.transform.position = new Vector3(Sewy.transform.position.x, holder.transform.position.y, holder.transform.position.z);
                }
            }
            if (!right)
            {
                if (Sewy.transform.position.z < -1)
                {
                    holder.transform.position = new Vector3(holder.transform.position.x, holder.transform.position.y, Sewy.transform.position.z);
                }
            }
        }*/
        if (attack)
        {
            attack = false;
            attacking = true;
            attacktime = 11;
            waittime = 1.5f;
            if (right)
            {
                if (Sewy.transform.position.x > 31)
                {
                    holder.transform.position = new Vector3(Sewy.transform.position.x, holder.transform.position.y, holder.transform.position.z);
                }
            }
           
        }
        if (attack2)
        {
            attack2 = false;
            attacking = true;
            attacktime = 11;
            waittime = 0f;
            transform.tag = "HeldDown";
            if (right)
            {
                if (Sewy.transform.position.x > -5)
                {
                    holder.transform.position = new Vector3(Sewy.transform.position.x, holder.transform.position.y, holder.transform.position.z);
                }
            }
           
        }
        //the cat will slap once and then not anymore
        //it will wait a specific amount of time
        //the 11 and 10s are used to prevent timers from running
        if (attacking)
        {
            if (waittime < 10)
            {
                waittime -= Time.deltaTime;
            }
            if (attacktime < 10)
            {
                attacktime -= Time.deltaTime;
            }
            if (waittime < 0)
            {
                waittime = 11;
                attacktime = 1.5f;
                myanim.SetTrigger("Slap");
            }
            if (attacktime < 0)
            {
                transform.position -= transform.up *= 10;
                attacking = false;
                Head.GetComponent<CathHead>().canattack = true;
            }

        }
    }

}
