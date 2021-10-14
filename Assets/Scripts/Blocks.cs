using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public int sewn = 0;
    public bool stuck;
    public int cost;
    public bool grab2;
    public bool rotated;
    public string Upwards;
    public float displace;
    public List<GameObject> children;
    public List<GameObject> sewnToMe;
    public List<Material> mats;
    
    public GameObject lockthis;

    public void Start()
    {
        displace = Vector3.Distance(transform.position, transform.GetChild(0).transform.position);
        if (transform.gameObject.layer == 11)
        {
            grab2 = true;
        }
        foreach (Transform child in this.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.tag == "Block")
            {
                children.Add(child.gameObject);
            }
        }
        if (children.Count == 0)
        {
            children.Add(this.gameObject);
        }
    }
    public void Update()
    {
        if (stuck && sewn == 0)
        {
            stuck = false;

                gameObject.tag = "Moveable";
            
           
        }
        if (!stuck && sewn > 0)
        {
            stuck = true;
            gameObject.tag = "SewnDown";
        }
    }
}
