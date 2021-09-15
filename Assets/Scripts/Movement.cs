using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public const int HELD_LAYER = 6;
    public const int GRABBABLE_LAYER = 7;
    public const int GROUND_LAYER = 8;

    public int speed;
    public Camera mycam;
    public Material mat; //feedback mat
    public Material mat2; // tempground
    public GameObject myhit;
    public bool holdblock = false;

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
        else if (Input.GetKey("a"))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else if (Input.GetKey("d"))
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
                        myhit.layer = HELD_LAYER;
                        holdblock = true;
                    }
                }
            }
            else if (myhit != null)
            {
                if (hit.transform.gameObject.layer != GRABBABLE_LAYER)
                {
                    //block placement
                    myhit.transform.position = _CalculatePlacement(hit);
                    /*
                    myhit.transform.position = new Vector3(Mathf.Round(hit.point.x), 
                        (transform.position.y - 1.5f),//(hit.point.y),
                        Mathf.Round(hit.point.z)) + (myhit.transform.localScale.y / 2 * Vector3.up);
                        */
                    if (Input.GetMouseButtonDown(0))
                    {
                        myhit.layer = GRABBABLE_LAYER;
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

    private Vector3 _CalculatePlacement(in RaycastHit hit)
    {
        //float newY = go.transform.position.y;// + hit.transform.GetComponent<Collider>().bounds.size.y/2f;
        return new Vector3(Mathf.Round(hit.point.x), 
            (hit.transform.localPosition.y),
            Mathf.Round(hit.point.z)) + (myhit.transform.localScale.y / 2 * Vector3.up);
    }

}
