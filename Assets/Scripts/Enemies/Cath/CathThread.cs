using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathThread : MonoBehaviour
{
    public GameObject threadnext;
    public GameObject threadlast;
    public bool canbecut;
    public void Start()
    {
        transform.parent.GetComponent<CathPawTwo>().sewn += 1;
    }
    private void OnTriggerStay(Collider other)
    {
        if (canbecut)
        {
            if (other.tag == "Cut")
            {

                transform.parent.GetComponent<CathPawTwo>().sewn -= 1;
                transform.parent.GetComponent<CathPawTwo>().cut = true;
                Destroy(gameObject);
                if (threadnext != null)
                {
                    threadnext.GetComponent<CathThread>().canbecut = true;
                    threadnext.SetActive(false);
                }
                if (threadlast != null)
                {
                    threadlast.SetActive(false);
                }
            }
        }
    }
}
