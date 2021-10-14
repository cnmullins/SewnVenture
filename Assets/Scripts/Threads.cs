using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threads : MonoBehaviour
{
    public void Start()
    {
        transform.parent.GetComponent<Blocks>().sewn += 1;
        transform.parent.GetComponent<Blocks>().sewnToMe.Add(this.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cut")
        {
            transform.parent.GetComponent<Blocks>().sewnToMe.Remove(this.gameObject);
            transform.parent.GetComponent<Blocks>().sewn -= 1;
            Destroy(gameObject);
        }
    }
}
