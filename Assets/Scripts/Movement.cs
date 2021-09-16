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
    public bool holdblock = false;

    public GameObject detecter;

    public LayerMask laymask;
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
        Ray ray = (mycam.ScreenPointToRay(Input.mousePosition));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, laymask))
        {
            if (!holdblock)
            {
                if (myhit != hit.transform.gameObject && myhit != null)
                {

                    myhit.GetComponent<MeshRenderer>().material = mat2;
                    myhit = null;
                }
                if (hit.transform.tag == "Moveable" && myhit == null)
                {
                    myhit = hit.transform.gameObject;
                    mat2 = myhit.GetComponent<MeshRenderer>().material;
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material = mat;

                }
                if (myhit != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        myhit.layer = 6;
                        holdblock = true;
                        detecter = myhit.transform.GetChild(0).gameObject;
                    }
                }
            }
            else if (myhit != null)
            {
                if (hit.transform.gameObject.layer != 7)
                {
                    myhit.transform.position = new Vector3(Mathf.Round(hit.point.x), (hit.point.y), Mathf.Round(hit.point.z)) + (myhit.transform.localScale.y / 2 * Vector3.up);
                    if (Input.GetMouseButtonDown(0))
                    {
                        myhit.layer = 7;
                        holdblock = false;
                       
                    }
                }
            }


            // Do something with the object that was hit by the raycast.
        }
        else if (myhit != null && !holdblock)
        {
            myhit.GetComponent<MeshRenderer>().material = mat2;
            myhit = null;
        }


    }
}
