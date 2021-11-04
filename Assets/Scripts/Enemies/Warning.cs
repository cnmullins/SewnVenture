using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public float lifetime = 2;
    public MeshRenderer thismesh;
    public void Start()
    {
        thismesh = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= 1 * Time.deltaTime;
         thismesh.material.color = new Color(127, 0, 0,lifetime/2);
        if (lifetime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
