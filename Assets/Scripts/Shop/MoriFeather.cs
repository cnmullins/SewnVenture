using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriFeather : MonoBehaviour
{
    public LayerMask checkground;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5, checkground))
        {
            //feathers are destroyed by silverfish or the ground.
            if (hit.transform.gameObject.tag == "Enemy")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
