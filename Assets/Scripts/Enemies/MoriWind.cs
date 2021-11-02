using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriWind : MonoBehaviour
{
    public float strength;
    // Start is called before the first frame update
    void Start()
    {
        strength *= Random.Range(0.5f, 1.75f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,strength);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * strength * (Time.deltaTime * 50);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            other.transform.position += (Time.deltaTime*50) * transform.forward * strength;
        }
        if (other.tag == "Token")
        {

            other.transform.position += (Time.deltaTime * 50) * transform.forward * strength/1.5f;
        }
    }
}
