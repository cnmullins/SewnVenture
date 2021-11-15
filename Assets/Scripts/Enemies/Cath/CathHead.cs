using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathHead : MonoBehaviour
{
    //three parts of cath
    public GameObject tail;
    public GameObject pawR;
    public GameObject pawL;
    //cath's toy
    public GameObject Toy;
    //detects what cath is attacking with its bite attack.
    public GameObject player;
    public GameObject target;
    public bool targetblock;
    //if cath has a toy cath cannot attack.
    public bool hastoy;
    //bite attack
    public bool bite;
    public bool biting;

    public float updist;
    public float bitedist;

    public bool canattack;
    // Start is called before the first frame update
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hastoy && !canattack)
        {

        }
        if (bite)
        {
            canattack = false;
            biting = true;
            bite = false;
            if (Random.Range(0, 3) == 10)
            {
                target = player;
                targetblock = false;
            }
            else
            {
                if (FindObjectOfType<Blocks>() != null)
                {
                    target = FindObjectOfType<Blocks>().gameObject;
                    targetblock = true;
                    if ((target.transform.position.x > -2.1f && target.transform.position.z < -3.1) || target.layer == 6)
                    {
                        target = player;
                        targetblock = false;
                    }
                }
                else
                {
                    target = player;
                    targetblock = false;
                }
            }
            if (target.transform.position.x < -2.1f || target.transform.position.z > -3.1)
            {
                transform.position = target.transform.position + new Vector3(-6, -5, 6);
                bitedist = 6;
                updist = 5;
            }
            else
            {
                canattack = true;
            }

        }
        if (biting)
        {
            
        }
    }
}
