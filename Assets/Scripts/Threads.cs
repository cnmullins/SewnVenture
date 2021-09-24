using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threads : MonoBehaviour
{
    public void Start()
    {
        transform.parent.GetComponent<Blocks>().sewn += 1;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cut")
        {
            transform.parent.GetComponent<Blocks>().sewn -= 1;
            Destroy(gameObject);
        }
    }
}
