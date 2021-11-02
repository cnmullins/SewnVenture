using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silverfish : MonoBehaviour
{
    public int speed = 1;
    public LayerMask blockmask;
    public LayerMask walkmask;
    public bool clockwise;
    public bool canturn;
    public Vector3 myscale;
    public GameObject tempparent;
    // Update is called once per frame
    private void Start()
    {
        myscale = transform.lossyScale;
    }
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.up * -1, out hit, 1f, walkmask);
        if (hit.transform != null)
        {
            if (transform.parent != null)
            {
                if (hit.transform.gameObject != transform.parent)
                {
                    if (transform.parent.transform.parent != null)
                    {

                        if (transform.parent.transform.parent.GetComponent<Blocks>() != null)
                        {
                            transform.parent.transform.parent.GetComponent<Blocks>().bugs -= 1;
                        }
                    }
                }
            }
                    transform.parent = hit.transform;
                    if (transform.parent.transform.parent != null)
                    {
                        if (transform.parent.transform.parent.GetComponent<Blocks>() != null)
                        {
                            transform.parent.transform.parent.GetComponent<Blocks>().bugs += 1;
                        }
                    }
                
            
        }
        if (!Physics.BoxCast(transform.position, new Vector3(0.25f, 0.01f, 0.01f), transform.forward, transform.rotation, Time.deltaTime * speed+0.5f, blockmask) && (Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up*-0.1f, 0.3f, walkmask)|| (Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.45f), transform.up * -0.1f, 0.3f, walkmask))))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else if (Physics.Raycast(transform.position, transform.up * -0.1f, 0.27f, walkmask))
        {


                if (clockwise)
                {
                    transform.Rotate(0, 90, 0);
                    //Debug.Log(transform.position);
                    if (Physics.BoxCast(transform.position, new Vector3(0.25f, 0, 0), transform.forward, transform.rotation, Time.deltaTime * speed + 0f, blockmask))
                    {
                        Debug.Log("Wall");
                    }
                    if (!Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up * -1, 0.3f, walkmask))
                    {
                        Debug.Log("NoGround");
                    }
                }
                else
                {
                    transform.Rotate(0, -90, 0);
                    if (Physics.BoxCast(transform.position, new Vector3(0.25f, 0, 0), transform.forward, transform.rotation, Time.deltaTime * speed + 0f, blockmask))
                    {
                        Debug.Log("Wall");
                    }
                    if (!Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up * -1, 0.3f, walkmask))
                    {
                        Debug.Log("NoGround");
                    }
                }
            tempparent = transform.parent.gameObject;
            transform.parent = null;
            transform.localScale = myscale;
            transform.parent = tempparent.transform;


        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" || other.tag == "Block")
        {
            if (transform.parent.transform.parent != null)
            {
                if (transform.parent.transform.parent.GetComponent<Blocks>() != null)
                {
                    transform.parent.transform.parent.GetComponent<Blocks>().bugs -= 1;
                }
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Wind")
        {
            if (Physics.Raycast(transform.position + other.transform.forward * other.transform.eulerAngles.z/2, transform.up * -0.1f, 0.3f, walkmask))
            {
                transform.position += other.transform.forward * other.transform.eulerAngles.z/2;
            }
        }
    }
}
