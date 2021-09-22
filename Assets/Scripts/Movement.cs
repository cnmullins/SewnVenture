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
    public LayerMask onlyground;
    public LayerMask currentlayermask;
    public LayerMask noground;

    public bool sewing;
    public GameObject overlay;

    public int thread;
    

    // Start is called before the first frame update
    void Start()
    {
        currentlayermask = laymask;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position,transform.forward);
        if (!sewing)
        {
            //basic movement, it checks if you can move and also checks if you will fall off.
            //if you wont then you move.
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
        //holding space lets you cut red strings.
        if (Input.GetKeyDown("space"))
        {
            transform.tag = "Cut";
        }
        if (Input.GetKeyUp("space"))
        {
            transform.tag = "Untagged";
        }
        //presing R toggles sewing mode.
        //you can only exit sewing mode if you arent holding any blocks.
        //you can only enter sewing mode while on a stable surface
        if (Input.GetKeyDown("r") && !sewing)
        {
            if (!Physics.BoxCast(transform.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, transform.rotation, 2f, currentlayermask))
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
        //this is the main raycast, it goes towards the mouse.
        //it detects what blocks are being hit and lets you move them
        //the layermask will swap when a block is being held for better movement.

        RaycastHit hit;
        Ray ray = (mycam.ScreenPointToRay(Input.mousePosition));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentlayermask) && sewing)
        {
            if (!holdblock)
            {
                //mousing on or off of a block will make it red and set it up as myhit
                //this allows you to pick it up.
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
                if (myhit != null )
                {
                    if (Input.GetMouseButtonDown(0) && myhit.tag == "Moveable")
                    {
                        //when you click on a movable object in sewing mode
                        //the game will change it to being moved
                        //it resets the material, change the layer
                        //and moves the object to the mouses position.
                        //it then sets the layermask for easier movement
                        if (myhit.transform.GetChild(0).gameObject != null)
                        {
                            detector = myhit.transform.GetChild(0).gameObject;
                            myhit.GetComponent<MeshRenderer>().material = mat2;
                            savedpos = myhit.transform.position;
                            myhit.layer = 6;
                            holdblock = true;
                            myhit.transform.position += (myhit.transform.position - detector.transform.position);
                            currentlayermask = onlyground;
                        }
                        else
                        {
                            Debug.Log("You need to add a child to the block!");
                           
                        }

                    }
                    //clicking a shopitem you can afford is interesting
                    //it removes the currency then just set the object to the one being held
                    //the game will change it to being moved
                    //it resets the material, change the layer
                    //and moves the object to the mouses position.
                    //it then changes the layermask for easier movement.
                    if ((Input.GetMouseButtonDown(0) && myhit.tag == "ShopItem") && thread >= myhit.GetComponent<ShopItem>().cost)
                    {
                            thread -= myhit.GetComponent<ShopItem>().cost;
                            myhit.GetComponent<MeshRenderer>().material = mat2;
                            myhit = Instantiate(myhit.GetComponent<ShopItem>().Purchase,transform.position,transform.rotation);
                            detector = myhit.transform.GetChild(0).gameObject;
                            myhit.GetComponent<MeshRenderer>().material = mat2;
                            savedpos = myhit.transform.position;
                            myhit.layer = 6;
                            holdblock = true;
                            myhit.transform.position += (myhit.transform.position - detector.transform.position);
                            currentlayermask = onlyground;
                    }
                }
                //if you mouse over a shopitem it becomes red
                //it also sets it up as "myhit" so clicking will work.
                if (hit.transform.tag == "ShopItem" && myhit == null)
                {
                    myhit = hit.transform.gameObject;
                    mat2 = myhit.GetComponent<MeshRenderer>().material;
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material = mat;

                }
            }
            //if your holding an object, clicking will try to put it down
            //if it wont fit it wont be put down
            //if it is successfully put down the layermask will update.
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
                            currentlayermask = laymask;
                        }
                    }
                }

            }
           
        }
        //right clicking will do one of two things.
        //if its a shop item, it will be sold back to the shop.
        //if its a non-shop item it will be put back where it used to be.
        if (Input.GetMouseButtonDown(1) && myhit != null && holdblock)
        {
            if (myhit.GetComponent<Blocks>().cost == 0)
            {
                myhit.transform.position = savedpos;
                detector = null;
                myhit.layer = 7;
                holdblock = false;
            }
            else
            {
                thread += myhit.GetComponent<Blocks>().cost;
                detector = null;
                holdblock = false;
                Destroy(myhit.gameObject);
            }
            currentlayermask = laymask;
        }
        /*else if (myhit != null && !holdblock)
        {
            myhit.GetComponent<MeshRenderer>().material = mat2;
            myhit = null;
        }*/


    }
    //picking up a token gives you the tokens.
    //simple.
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Token")
        {
            thread += other.GetComponent<Token>().worth;
            Destroy(other.gameObject);
        }
    }

}