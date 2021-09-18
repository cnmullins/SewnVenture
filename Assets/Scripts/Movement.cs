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

    public GameObject detector;
    public Vector3 savedpos;

    public LayerMask laymask;
    public LayerMask noground;

    public bool sewing;
    public GameObject overlay;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position,transform.forward);
        if (!sewing)
        {
            if (Input.GetKey("w") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.forward, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.forward * 0.4f, Vector3.down, 1.5f))
            {
                transform.position += Vector3.forward * Time.deltaTime * speed;
            }
            else if (Input.GetKey("s") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.back, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.back * 0.4f, Vector3.down, 1.5f))
            {
                transform.position += Vector3.back * Time.deltaTime * speed;
            }
            else if (Input.GetKey("a") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.left, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.left * 0.4f, Vector3.down, 1.5f))
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
            }
            else if (Input.GetKey("d") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.right, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.right * 0.4f, Vector3.down, 1.5f))
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }
        }
        if (Input.GetKeyDown("space"))
        {
            transform.tag = "Cut";
        }
        if (Input.GetKeyUp("space"))
        {
            transform.tag = "Untagged";
        }

        if (Input.GetKeyDown("r") && !sewing)
        {
            if (!Physics.BoxCast(transform.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, transform.rotation, 2f, laymask))
            {
                sewing = true;
                overlay.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown("r") && sewing && !holdblock)
        {
            sewing = false;
            overlay.gameObject.SetActive(false);
            if (myhit != null)
            {
                myhit.GetComponent<MeshRenderer>().material = mat2;
                myhit = null;
            }    
        }
        RaycastHit hit;
        Ray ray = (mycam.ScreenPointToRay(Input.mousePosition));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, laymask) && sewing)
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
                       
                        if (myhit.transform.GetChild(0).gameObject != null)
                        {
                            detector = myhit.transform.GetChild(0).gameObject;
                            myhit.GetComponent<MeshRenderer>().material = mat2;
                            savedpos = myhit.transform.position;
                            myhit.layer = 6;
                            holdblock = true;
                            myhit.transform.position += (myhit.transform.position - detector.transform.position);

                        }
                        else
                        {
                            Debug.Log("You need to add a child to the block!");
                           
                        }

                    }
                }
            }
            else if (myhit != null)
            {
                if (hit.transform.gameObject.layer != 7)
                {
                    myhit.transform.position = new Vector3(Mathf.Round(hit.point.x), (hit.point.y), Mathf.Round(hit.point.z)) + ((myhit.transform.localScale.y / 2) * Vector3.up) + (myhit.transform.position-detector.transform.position); 
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!Physics.BoxCast(myhit.transform.position, myhit.transform.localScale / 2.1f, Vector3.down, myhit.transform.rotation, 20, noground))
                        {
                            myhit.layer = 7;
                            holdblock = false;
                            myhit.transform.position = detector.transform.position;
                            detector = null;
                        }
                    }
                }

            }
            // Do something with the object that was hit by the raycast.
        }
        if (Input.GetMouseButtonDown(1) && myhit != null && holdblock)
        {
            myhit.transform.position = savedpos;
            detector = null;
            myhit.layer = 7;
            holdblock = false;
        }
        /*else if (myhit != null && !holdblock)
        {
            myhit.GetComponent<MeshRenderer>().material = mat2;
            myhit = null;
        }*/


    }
   
}
