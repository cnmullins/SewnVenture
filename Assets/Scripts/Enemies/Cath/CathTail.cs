using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathTail : MonoBehaviour
{
    public GameObject Head;
    public bool sweep;
    public bool sweeping;
    public bool sweepright;
    public bool away;
    public float sweepdist;
    public int sweepdistmax = 20;
    public float waitdist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sweep)
        {
            sweeping = true;
            sweep = false;
            if (Random.Range(0,2) == 1)
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
                transform.position += transform.right * (sweepdistmax-2);
            }
            if (!away && !sweepright)
            {
                transform.position -= transform.right * (sweepdistmax-2);
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
        if (sweeping)
        {
            if (waitdist > 0)
            {
                transform.position += transform.up * 10 * Time.deltaTime;
                waitdist -= 10 * Time.deltaTime;
            }
            else if (sweepdist > 0)
            {
                if ((away && sweepright) || (!away && !sweepright))
                {
                    transform.position += transform.right * 10 * Time.deltaTime;
                }
                else
                {
                    transform.position -= transform.right * 10 * Time.deltaTime;
                }
                sweepdist -= 10 * Time.deltaTime;
            }
            else if (waitdist > -10)
            {
                transform.position -= transform.up * 10 * Time.deltaTime;
                waitdist -= 10 * Time.deltaTime;
            }
        }
    }
}
