using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathTail : MonoBehaviour
{
    public GameObject Cath;
    //these are bools for the attacks
    public bool sweep;
    public bool sweep2;
    public bool sweeping;
    public bool sweeping2;
    //these two determine the direction of the attacks;
    public bool sweepright;
    public bool away;
    //these are distances for how far the tail needs to go to move
    public float sweepdist;
    public float sweepdist2;
    public int sweepdistmax = 20;
    public float waitdist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //sweep will set up the first sweep attack
        //it chooses a random direction and side of the block, using that it will move the tail
        //lastly it tells the tail to sweep attack and sets up timers.
        if (sweep)
        {
            sweeping = true;
            sweep = false;
            if (Random.Range(0, 2) == 1)
            {
                sweepright = false;
            }
            else
            {
                sweepright = true;
            }
            if (Random.Range(0, 2) == 1)
            {
                away = false;
            }
            else
            {
                away = true;
            }
            if (sweepright)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 270, 0);
            }
            transform.position = new Vector3(-5, -6.5f, -1);
            if (!away && sweepright)
            {
                transform.position += transform.right * (sweepdistmax - 2);
            }
            if (!away && !sweepright)
            {
                transform.position -= transform.right * (sweepdistmax - 2);
            }
            if (away && sweepright)
            {
                transform.position -= transform.right * (2);
            }
            if (away && !sweepright)
            {
                transform.position += transform.right * (2);
            }
            sweepdist = sweepdistmax;
            waitdist = 10;
        }
        //this is for the sweeping attack.
        //firstly the tail will go upwards.
        //while sweeping the tail will move based on its direction.
        //after reaching its destination the tail retreats.
        if (sweeping)
        {
            if (waitdist > 0)
            {
                transform.position += transform.up * 10 * Time.fixedDeltaTime;
                waitdist -= 10 * Time.fixedDeltaTime;
            }
            else if (sweepdist > 0)
            {
                if ((away && sweepright) || (!away && !sweepright))
                {
                    transform.position += transform.right * 10 * Time.fixedDeltaTime;
                }
                else
                {
                    transform.position -= transform.right * 10 * Time.fixedDeltaTime;
                }
                sweepdist -= 10 * Time.fixedDeltaTime;

            }
            else if (waitdist > -10)
            {
                transform.position -= transform.up * 10 * Time.fixedDeltaTime;
                waitdist -= 10 * Time.fixedDeltaTime;
            }
            else
            {
                sweeping = false;
                Cath.GetComponent<CathHead>().canattack = true;
            }
        }

        //sweep2 is very similar to sweep 1, but has different attacks
        //it chooses a random side of the block, using that it will move the tail and attack. no direction needed
        //lastly it tells the tail to sweep attack 2 and sets up timers.

        if (sweep2)
        {
            sweeping2 = true;
            sweep2 = false;
            if (Random.Range(0, 2) == 1)
            {
                sweepright = false;
            }
            else
            {
                sweepright = true;
            }
            if (sweepright)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 270, 0);
            }
            transform.position = new Vector3(-5, -6.5f, -1);
            if (sweepright)
            {
                transform.position += transform.right * (sweepdistmax - 2);
            }
            if (!sweepright)
            {
                transform.position -= transform.right * (sweepdistmax - 2);
            }
            sweepdist = sweepdistmax-2;
            waitdist = 10;
        }
        //this is for the sweeping attack V2.
        //firstly the tail will go upwards.
        //while sweeping the tail will move based on its direction.
        //it then changes direction and attacks the other side
        //after reaching its destination the tail retreats.
        //this is a harder attack to dodge.
        if (sweeping2)
        {
            if (waitdist > 0)
            {
                transform.position += transform.up * 10 * Time.deltaTime;
                waitdist -= 10 * Time.deltaTime;
            }
            else if (sweepdist > 0)
            {
                if (!sweepright)
                {
                    transform.position += transform.right * 10 * Time.deltaTime;
                }
                else
                {
                    transform.position -= transform.right * 10 * Time.deltaTime;
                }
                sweepdist -= 10 * Time.deltaTime;

                if (sweepdist <= 0)
                {
                    sweepdist2 = sweepdistmax - 2;
                    if (sweepright)
                    {
                        transform.eulerAngles = new Vector3(0, 270, 0);

                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }

            else if (sweepdist2 > 0)
            {
                if (!sweepright)
                {
                    transform.position += transform.right * 10 * Time.deltaTime;
                }
                else
                {
                    transform.position -= transform.right * 10 * Time.deltaTime;
                }
                sweepdist2 -= 10 * Time.deltaTime;
            }

            else if (waitdist > -10)
            {
                transform.position -= transform.up * 10 * Time.deltaTime;
                waitdist -= 10 * Time.deltaTime;
            }
            else
            {
                sweeping2 = false;
                Cath.GetComponent<CathHead>().canattack = true;
                
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block" && other.gameObject.layer != 6)
        {

            Cath.GetComponent<CathHead>().health -= 1;
            other.tag = "Untagged";
            Destroy(other.transform.parent.gameObject);
        }
    }
}


