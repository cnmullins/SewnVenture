using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriSilverfishSpawner : MonoBehaviour
{
    public float lifetime;
    public int spawnChance = 100;
    public int maxspawns = 10;
    public GameObject silverfish;
    public GameObject lastSilverfish;
    
    void Start()
    {
        if (Random.Range(1, 3) == 2)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime -= 1 * Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
        if (Random.Range(0, spawnChance) == 0 && maxspawns > 0)
        {

            maxspawns -= 1;
            if (Random.Range(1, 3) == 2)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
                lastSilverfish = Instantiate(silverfish, transform.position + (transform.up * 0.25f), transform.rotation);

            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                lastSilverfish = Instantiate(silverfish, transform.position + (transform.up * 0.25f), transform.rotation);
                lastSilverfish.GetComponent<Silverfish>().clockwise = false;
            }


        }
    }
   
}
