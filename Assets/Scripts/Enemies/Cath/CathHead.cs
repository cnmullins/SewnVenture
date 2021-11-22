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
    //for cath to choose an attack
    public int chosenattack;
    //bite attack and relevent movement distances required for it
    public bool bite;
    public bool biting;
    public float updist;
    public float bitedist;
    //a bool for when cath can do an attack
    public bool canattack;
    //anger grows the more thread the player takes from cath, this makes the battle harder.
    public int anger;
    public GameObject mytoy;
    public bool toyattack;
    public bool toywait;
    public float toyupdist;
    //health is for phase transition
    public int health;
    public GameObject mover;
    //this is for phase transition
    public bool canattack2;
    // Start is called before the first frame update
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //cath will hit the player to the center and start phase 2 if they are attacked.
        if (health <= 0)
        {
            mover.transform.position = player.transform.position;
            health = 100;
            pawL.GetComponent<CathPaw>().attack2 = true;
            pawR.GetComponent<CathPaw>().attack2 = true;
        }
        if (canattack && canattack2)
        {
            if (Random.Range(0,100-10*anger) <= 1)
            {
                chosenattack = (Random.Range(0, 5));
                if (chosenattack == 1)
                {
                    bite = true;
                    canattack = false;
                }
                if (chosenattack == 2)
                {
                    if (Random.Range(0, 2 + anger) == 1)
                    {
                        toyattack = true;
                        canattack = false;
                    }
                }
                if (chosenattack == 3)
                {
                    if (Random.Range(0, 10 - anger) > 2)
                    {
                        tail.GetComponent<CathTail>().sweep = true;
                        canattack = false;
                    }
                    if (Random.Range(0, 10 - anger) <= 2)
                    {
                        tail.GetComponent<CathTail>().sweep2 = true;
                        canattack = false;
                    }
                }
                if (chosenattack == 4)
                {
                    if (Random.Range(0, 10 - anger) < 2)
                    {
                        pawL.GetComponent<CathPaw>().attack = true;
                        pawR.GetComponent<CathPaw>().attack = true;
                    }
                    if (player.transform.position.x < -2)
                    {
                        canattack = false;
                        pawL.GetComponent<CathPaw>().attack = true;
                    }
                    else if (player.transform.position.z > -4)
                    {
                        canattack = false;
                        pawR.GetComponent<CathPaw>().attack = true;
                    }
                    else
                    {
                        canattack = true;
                    }
                    
                    
                }
            }
           
        }
        //toyattack is less an attack and more a way to give the player thread tokens
        if (toyattack)
        {
            canattack = false;
            toyattack = false;
            toywait = true;
            transform.position = new Vector3(-9, -6, 3);
            toyupdist = 11;
            mytoy = Instantiate(Toy, transform.position, Quaternion.identity);
            mytoy.transform.parent = this.transform;
            mytoy.transform.position += new Vector3(1.2f, -1.2f, -1.2f);
        }
        //toywait is when cath will wait until the toy is taken, this lets the player do so with ease.
        if (toywait)
        {
            if (toyupdist > 0)
            {
                transform.position += Vector3.up * Time.fixedDeltaTime * 8f;
                toyupdist -= Time.fixedDeltaTime * 8f;
                if (toyupdist <= 0 && mytoy.transform.childCount != 0)
                {
                    mytoy = mytoy.transform.GetChild(0).gameObject;
                    mytoy.transform.parent = null;
                }
                if (toyupdist <= 0 && mytoy.transform.childCount == 0)
                {
                    mytoy = null;
                }
            }
            
            if (mytoy != null)
            {
                if (Vector3.Distance(transform.position, mytoy.transform.position) > 5)
                {
                    toyupdist = -5;
                    mytoy = null;
                }
            }
            else if (toyupdist > -1)
            {
                toyupdist = -5;
            }
            else if (toyupdist < -4 && toyupdist > -16)
            {
                transform.position -= Vector3.up * Time.fixedDeltaTime * 8f;
                toyupdist -= Time.fixedDeltaTime * 8f;
            }
            else if (toyupdist <= -16)
            {
                //canattack = true;
                toyattack = false;
                toywait = false;
                toyupdist = 0;
                anger += 1;
                canattack = true;
            }
        }
        if (bite)
        {
            canattack = false;
            biting = true;
            bite = false;
            if (Random.Range(0, 3) == 1)
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
                bitedist = 5;
                updist = 5;
            }
            else
            {
                canattack = true;
            }

        }
        if (biting)
        {
            if (updist > 0)
            {
                transform.position += Vector3.up * Time.fixedDeltaTime * 6f * (1 + anger / 10);
                updist -= Time.fixedDeltaTime *3f * (1 + anger / 10);
            }
            else if (updist <= 0 && updist > -1)
            {
                updist -= Time.fixedDeltaTime * (1 + anger / 10);
            }
            else if (bitedist > 0)
            {
                transform.position += (Vector3.right + Vector3.back + (Vector3.down * 0.75f)) * Time.fixedDeltaTime * 6f * (1 + anger / 10);
                bitedist -= Time.fixedDeltaTime * 6 * (1 + anger / 10);
            }
            else if (bitedist <= 0 && bitedist > -2)
            {
                
                bitedist -= Time.fixedDeltaTime * 4 * (1 + anger / 10);
            }
            else if (bitedist <= -2 && bitedist > -6)
            {
                transform.position -= (Vector3.right + Vector3.back + (Vector3.down * 0.75f)) * Time.fixedDeltaTime * 6f*(1+anger/10);
                bitedist -= Time.fixedDeltaTime * 6f * (1 + anger / 10);
            }
            else if (updist <= -1 && updist > -8)
            {
                transform.position -= Vector3.up * Time.fixedDeltaTime * 6f * (1 + anger / 10);
                updist -= Time.fixedDeltaTime * 3f * (1 + anger / 10);
            }
            else if (updist <= -8)
            {
                updist = 0;
                biting = false;
                canattack = true;
            }
        }
    }

    //when cath's head hits a block, the block will be destroyed to emulate the cat tearing it up
    //this cannot happen to blocks being held.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable" && other.gameObject.layer != 6 && !toywait)
        {
            Destroy(other.gameObject);
            anger -= 1;
        }
    }
}
