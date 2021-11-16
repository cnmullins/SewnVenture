using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathMove : MonoBehaviour
{
    public GameObject center;
    public GameObject player;
    public Vector3 movethisdist;
    public int timesmoved = 0;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < 10)
        {
            if (timesmoved == -100)
            {
                timesmoved = 0;
                movethisdist = (center.transform.position- transform.position);
            }
            player.transform.position = transform.position;
            transform.position += movethisdist/20f;
            timesmoved += 1;
            if (timesmoved == 20)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
