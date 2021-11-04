using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public GameObject Player;
    public float sucking;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sucking >= 1)
        {
            transform.localScale = new Vector3(transform.localScale.x,10,transform.localScale.y);
            Player.transform.position += (transform.position-Player.transform.position) / 40;
            sucking -= Time.deltaTime;
            Player.GetComponent<Movement>().thread = 0;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.y);
        }
    }
}
