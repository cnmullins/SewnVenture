using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriWind : MonoBehaviour
{
    public float strength;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * strength * (Time.deltaTime * 50);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Token")
        {
            
            other.transform.position += (Time.deltaTime*50) * transform.forward * strength;
        }
    }
}
