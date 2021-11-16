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
    //longblock is for phase transition
    public int longblock;
    // Start is called before the first frame update
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            mover.transform.position = player.transform.position;
            health = 100;
        }
        if (!hastoy && !canattack)
        {

        }
        if (toyattack)
        {
            toyattack = false;
            toywait = true;
            transform.position = new Vector3(-9, -6, 3);
            toyupdist = 11;
            mytoy = Instantiate(Toy, transform.position, Quaternion.identity);
            mytoy.transform.parent = this.transform;
            mytoy.transform.position += new Vector3(1.2f, -1.2f, -1.2f);
        }
        if (toywait)
        {
            if (toyupdist > 0)
            {
                transform.position += Vector3.up * Time.deltaTime * 8f;
                toyupdist -= Time.deltaTime * 8f;
                if (toyupdist <= 0)
                {
                    mytoy = mytoy.transform.GetChild(0).gameObject;
                    mytoy.transform.parent = null;
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
                transform.position -= Vector3.up * Time.deltaTime * 8f;
                toyupdist -= Time.deltaTime * 8f;
            }
            else if (toyupdist <= -16)
            {
                canattack = true;
                toyattack = false;
                toywait = false;
                toyupdist = 0;
                anger += 1;
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
                transform.position += Vector3.up * Time.deltaTime * 6f;
                updist -= Time.deltaTime*3f;
            }
            else if (updist <= 0 && updist > -1)
            {
                updist -= Time.deltaTime;
            }
            else if (bitedist > 0)
            {
                transform.position += (Vector3.right + Vector3.back + (Vector3.down * 0.75f)) * Time.deltaTime * 6f;
                bitedist -= Time.deltaTime * 6;
            }
            else if (bitedist <= 0 && bitedist > -2)
            {
                
                bitedist -= Time.deltaTime * 4;
            }
            else if (bitedist <= -2 && bitedist > -6)
            {
                transform.position -= (Vector3.right + Vector3.back + (Vector3.down * 0.75f)) * Time.deltaTime * 6f;
                bitedist -= Time.deltaTime * 6f;
            }
            else if (updist <= -1 && updist > -8)
            {
                transform.position -= Vector3.up * Time.deltaTime * 6f;
                updist -= Time.deltaTime * 3f;
            }
            else if (updist <= -8)
            {
                updist = 0;
                biting = false;
                canattack = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable" && other.gameObject.layer != 6 && !toywait)
        {
            Destroy(other.gameObject);
            anger -= 1;
        }
    }
}
