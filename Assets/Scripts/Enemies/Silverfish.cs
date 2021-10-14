using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silverfish : MonoBehaviour
{
    public int speed = 1;
    public LayerMask blockmask;
    public LayerMask walkmask;
    public bool clockwise;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up * -1, out hit, 1f, walkmask);
        if (hit.transform != null)
        {
            transform.parent = hit.transform;
        }
        if (!Physics.BoxCast(transform.position, new Vector3(0.25f, 0.01f, 0.01f), transform.forward, transform.rotation, Time.deltaTime * speed+0.5f, blockmask) && Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up*-0.1f, 0.3f, walkmask))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else
        {
            if (clockwise)
            {
                transform.Rotate(0, 90, 0);
            }
            else
            {
                transform.Rotate(0, -90, 0);
            }
        }
        if (Physics.BoxCast(transform.position, new Vector3(0.15f, 0, 0), transform.forward, transform.rotation, Time.deltaTime * speed + 0f, blockmask))
            {
            Debug.Log("Wall");
            }
        if (!Physics.Raycast(transform.position + transform.forward * (Time.deltaTime * speed + 0.5f), transform.up * -1, 0.3f, walkmask))
            {
            Debug.Log("NoGround");
        }
    }
}
