using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int speed;
    public Camera mycam;
    public Material mat;
    public Material mat2;
    public GameObject myhit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        else if (Input.GetKey("s"))
        {
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        else if(Input.GetKey("a"))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else if(Input.GetKey("d"))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }

        RaycastHit hit;
        Ray ray = mycam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (myhit != hit.transform.gameObject && myhit != null)
            {
                myhit.GetComponent<MeshRenderer>().material = mat2;
                myhit = null;
            }
            if (hit.transform.tag == "Moveable" && myhit == null)
            {
                myhit = hit.transform.gameObject;
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = mat;
            }
            

            // Do something with the object that was hit by the raycast.
        }
        else if (myhit != null)
        {
            myhit.GetComponent<MeshRenderer>().material = mat2;
            myhit = null;
        }
    }
}
