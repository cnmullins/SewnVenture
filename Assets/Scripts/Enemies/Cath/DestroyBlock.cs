using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public GameObject cathone;
    public GameObject cathtwo;
    public GameObject shop1;
    public GameObject shop2;
    public GameObject shop3;
    public GameObject curtain;
    public bool evil = false;
    public bool savedevil;
    // Start is called before the first frame update
    public void Update()
    {
        if (evil)
        {
            shop3.SetActive(true);
            evil = false;
            savedevil = true;
        }
    }
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
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
