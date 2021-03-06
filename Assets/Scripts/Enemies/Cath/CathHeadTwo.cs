using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathHeadTwo : MonoBehaviour
{
    //three parts of cath
    public GameObject tail;
    
    public GameObject paw;
    //cath's toy
    
    //detects what cath is attacking with its bite attack.
    public GameObject player;
    public GameObject target;
    public bool targetblock;
    
    //for cath to choose an attack
    public int chosenattack;
    //bite attack and relevent movement distances required for it
    public bool bite;
    public bool biting;
    public float updist;
    public float bitedist;
    //this one is used for biting behind cath.
    public int otherbite = 1;
    //a bool for when cath can do an attack
    public bool canattack;

    //delay for attacks
    public float delay = 4;
    
    //this is for phase transition
    public bool canattack2;
    public GameObject ender;
    public bool defeated;
    public float defeatup;
    // Start is called before the first frame update
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canattack2 && delay >= 0)
        {
            delay -= Time.deltaTime;
        }
        //when the boss is defeated it moves.
        if (tag == "Moveable" && !defeated)
        {
            defeated = true;
            transform.position = new Vector3(27,-5,-36);
            defeatup = 10;
            ender.SetActive(true);
        }
        if (defeated)
        {
            if (defeatup > 0)
            {
                transform.position += Vector3.up* 5f * Time.deltaTime;
                defeatup -= 5f * Time.deltaTime;
            }
        }

        //cath will hit the player to the center and start phase 2 if they are attacked.

        if (canattack && canattack2 && delay < 0)
        {
            if (Random.Range(0,100) <= 1)
            {
                chosenattack = (Random.Range(0, 5));
                if (chosenattack == 1)
                {
                    bite = true;
                    canattack = false;
                }
                if (chosenattack == 2)
                {
                    
                        tail.GetComponent<CathTailTwo>().sweep = true;
                        canattack = false;
                    
                
                }
                if (chosenattack >= 3)
                {

                        paw.GetComponent<CathPawTwo>().attack = true;
                    canattack = false;
                }
            }
           
        }
        //cath can bite down on the player
        //this attack is more severe when the player attacked in phase 1
        
        if (bite)
        {
            canattack = false;

                
            
            if (target.transform.position.x > 32)
            {
                transform.position = target.transform.position + new Vector3(-6, -5, 0);
                bitedist = 5;
                updist = 5;
                otherbite = 1;
                biting = true;
                bite = false;
                target = player;
                targetblock = false;
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (target.transform.position.x < 22)
            {
                transform.position = target.transform.position + new Vector3(6, -5, 0);
                bitedist = 5;
                updist = 5;
                otherbite = -1;
                biting = true;
                bite = false;
                target = player;
                targetblock = false;
                transform.eulerAngles = new Vector3 (0,270,0);
            }
            else
            {
                canattack = true;
            }

        }
        //after a bite is set up cath will bite.
        if (biting)
        {
            if (updist > 0)
            {
                transform.position += Vector3.up * Time.fixedDeltaTime * 6f;
                updist -= Time.fixedDeltaTime *3f;
            }
            else if (updist <= 0 && updist > -1)
            {
                updist -= Time.fixedDeltaTime;
            }
            else if (bitedist > 0)
            {
                transform.position += (Vector3.right*otherbite  + (Vector3.down * 0.75f)) * Time.fixedDeltaTime * 6f;
                bitedist -= Time.fixedDeltaTime * 6f ;
            }
            else if (bitedist <= 0 && bitedist > -2)
            {
                
                bitedist -= Time.fixedDeltaTime * 4;
            }
            else if (bitedist <= -2 && bitedist > -6)
            {
                transform.position -= (Vector3.right*otherbite  + (Vector3.down * 0.75f)) * Time.fixedDeltaTime * 6f;
                bitedist -= Time.fixedDeltaTime * 6f;
            }
            else if (updist <= -1 && updist > -8)
            {
                transform.position -= Vector3.up * Time.fixedDeltaTime * 6f;
                updist -= Time.fixedDeltaTime * 3f;
            }
            else if (updist <= -8)
            {
                updist = 0;
                biting = false;
                canattack = true;
            }
        }
    }
    //when hit with a block the fight will end
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackMori")
        {
            if (other.gameObject.transform.parent.gameObject.layer != 6)
            {
                //end fight here
                Destroy(other.transform.parent.gameObject);
                transform.position -= Vector3.up * 100;
                ender.gameObject.SetActive(true);
                canattack2 = false;
            }
        }
    }
}
