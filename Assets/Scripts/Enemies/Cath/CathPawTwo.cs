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
    public bool evil;
    //these two detect how close you are to the pacifist ending.
    public int sewn;
    public bool cut;
    public GameObject thread1;
    public GameObject thread2;
    public GameObject thread3;
    private void Start()
    {
        Sewy = GameObject.FindWithTag("Player");
        holder.transform.position -= Vector3.up * 10;
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
            if (Sewy.transform.position.x > 31)
            {
                cut = false;
                attack = false;
            attacking = true;
            attacktime = 11;
            waittime = 1.5f;
                holder.transform.position += Vector3.up * 10;

                holder.transform.position = new Vector3(holder.transform.position.x, holder.transform.position.y, Sewy.transform.position.z);
                }
            else
            {
                attack = false;
                attacking = false;
                Head.GetComponent<CathHeadTwo>().canattack = true;
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
            if (attacktime < 0 && (evil || cut))
            {
                holder.transform.position -= Vector3.up * 10;
                attacking = false;
                Head.GetComponent<CathHeadTwo>().canattack = true;
                if (cut && sewn == 0)
                {
                    Head.GetComponent<CathHeadTwo>().canattack2 = false;
                }
            }
            if (attacktime < 0 && (!evil && !cut))
            {
                if (thread1 != null)
                {
                    thread1.SetActive(true);
                }
                if (thread2 != null)
                {
                    thread2.SetActive(true);
                }
                if (thread3 != null)
                {
                    thread3.SetActive(true);
                }
            }


        }
    }

}
