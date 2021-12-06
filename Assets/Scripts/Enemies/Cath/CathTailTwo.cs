using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathTailTwo : MonoBehaviour
{
    public GameObject Cath;
    //these are bools for the attacks
    public bool sweep;
    public bool sweep2;
    public bool sweeping;
    public bool sweeping2;
    //these two determine the direction of the attacks;
    public bool sweeptop;
    public bool left;
    //these are distances for how far the tail needs to go to move
    public float sweepdist;
    public float sweepdist2;
    public int sweepdistmax = 20;
    public float waitdist;
    public GameObject token;
    // Start is called before the first frame update
    public bool evil;
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
                sweeptop = false;
            }
            else
            {
                sweeptop = true;
            }
            if (Random.Range(0, 2) == 1)
            {
                left = false;
            }
            else
            {
                left = true;

            }
            transform.position = new Vector3(33, -6.5f, -27);
            if (sweeptop)
            {
                transform.position += Vector3.left * (12);
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 270 , 0);
            }
            
            if (!left && sweeptop)
            {
                transform.position += transform.right * (sweepdistmax - 2);
            }
            if (!left && !sweeptop)
            {
                transform.position -= transform.right * (sweepdistmax - 2);
            }
            if (left && !sweeptop)
            {
                transform.position += transform.right * (2);
            }
            if (left && sweeptop)
            {
                
                transform.position -= transform.right * (2);
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
                if ((left && sweeptop) || (!left && !sweeptop))
                {
                    transform.position += transform.right * 10 * Time.fixedDeltaTime;
                }
                else
                {
                    transform.position -= transform.right * 10 * Time.fixedDeltaTime;
                }
                sweepdist -= 10 * Time.fixedDeltaTime;
                if (Random.Range(0,30) == 1 && evil)
                {
                    Instantiate(token, transform.position, transform.rotation);
                }

            }
            else if (waitdist > -10)
            {
                transform.position -= transform.up * 10 * Time.fixedDeltaTime;
                waitdist -= 10 * Time.fixedDeltaTime;
            }
            else
            {
                sweeping = false;
                Cath.GetComponent<CathHeadTwo>().canattack = true;
            }
        }

        //when a block is dropped on the tail, cath will take damage.
        //this is for the evil route.
    }
   
}


