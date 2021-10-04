using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public LayerMask sewmask;
    public LayerMask blockmask;

    public bool sewing;
    public GameObject overlay;

    public int thread;

    public Vector3 aimpoint;

    //used for casting

    public bool canfall;
    public bool canconnect;

    //current scene used for reloads;
    //public Scene reloadscene;

    // Start is called before the first frame update
    void Start()
    {
        currentlayermask = laymask;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && mycam.orthographicSize < 10f)
            {
            mycam.orthographicSize += 0.05f;
            overlay.transform.localScale += new Vector3(0.01f,0.01f,0.01f);
            }
        if (Input.GetKey(KeyCode.DownArrow) && mycam.orthographicSize > 2.5f)
        {
            mycam.orthographicSize -= 0.05f;
            overlay.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        }
        Debug.DrawLine(transform.position,transform.forward);
        if (!sewing)
        {
            //basic movement, it checks if you can move and also checks if you will fall off.
            //if you wont then you move.
            if (Input.GetKey("w") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.forward, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.forward * 0.4f, Vector3.down, 1.5f, noground))
            {
                transform.position += Vector3.forward * Time.deltaTime * speed;
            }
            else if (Input.GetKey("s") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.back, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.back * 0.4f, Vector3.down, 1.5f, noground))
            {
                transform.position += Vector3.back * Time.deltaTime * speed;
            }
            else if (Input.GetKey("a") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.left, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.left * 0.4f, Vector3.down, 1.5f, noground))
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
            }
            else if (Input.GetKey("d") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.right, transform.rotation, 0.6f) && Physics.Raycast(transform.position + Vector3.right * 0.4f, Vector3.down, 1.5f, noground))
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
            if (!Physics.BoxCast(transform.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, transform.rotation, 2f, sewmask))
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
            if (holdblock)
            {
                if (Input.GetKeyDown("q"))
                {
                    if (myhit.GetComponent<Blocks>().rotated == false)
                    {
                        myhit.GetComponent<Blocks>().rotated = true;
                    }
                    else
                    {
                        myhit.GetComponent<Blocks>().rotated = false;
                    }
                    myhit.transform.localEulerAngles += transform.up * 90;
                }
            }
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
                if (hit.transform.tag == "ShopItem" && myhit == null)
                {
                    myhit = hit.transform.gameObject;
                    mat2 = myhit.GetComponent<MeshRenderer>().material;
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material = mat;

                }
                if (myhit != null)
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
                            //if this item is currently sewing something down (as its a shop item)
                            //free the object.
                            if (myhit.GetComponent<Blocks>().lockthis != null)
                            {
                                myhit.GetComponent<Blocks>().lockthis.GetComponent<Blocks>().sewn -= 1;
                                myhit.GetComponent<Blocks>().lockthis = null;
                            }
                        }
                        else
                        {
                            Debug.Log("You need to add a child to the block!");

                        }
                        //this line is for shop item connection.
                        
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
                        myhit = Instantiate(myhit.GetComponent<ShopItem>().Purchase, transform.position, transform.rotation);
                        detector = myhit.transform.GetChild(0).gameObject;
                        myhit.GetComponent<MeshRenderer>().material = mat2;
                        savedpos = myhit.transform.position;
                        myhit.layer = 6;
                        holdblock = true;
                        myhit.transform.position += (myhit.transform.position - detector.transform.position);
                        currentlayermask = onlyground;

                    }

                    //if you mouse over a shopitem it becomes red
                    //it also sets it up as "myhit" so clicking will work.
                   
                }
            }
            //if your holding an object, clicking will try to put it down
            //if it wont fit it wont be put down
            //if it is successfully put down the layermask will update.
            else if (myhit != null)
            {
                if (hit.transform.gameObject.layer != 7)
                {
                    aimpoint = hit.point;
                    aimpoint.x = Mathf.Round(aimpoint.x);
                    aimpoint.z = Mathf.Round(aimpoint.z);
                    //Debug.Log(hit.transform.localScale);
                    if ((myhit.transform.localScale.x % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == false) || (myhit.transform.localScale.z % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == true))
                    {
                        aimpoint.x += 0.5f;
                    }
                    if ((myhit.transform.localScale.z % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == false) || (myhit.transform.localScale.x % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == true))
                    {
                        aimpoint.z += 0.5f;
                    }
                    myhit.transform.position = new Vector3(aimpoint.x, (aimpoint.y), aimpoint.z) + ((myhit.transform.localScale.y / 2) * Vector3.up) + (myhit.transform.position-detector.transform.position); 
                    if (Input.GetMouseButtonDown(0))
                    {
                        canfall = true;
                        if (myhit.GetComponent<Blocks>().cost != 0)
                        {
                            canconnect = false;
                        }
                        else
                        {
                            canconnect = true;
                        }
                        foreach (GameObject child in myhit.GetComponent<Blocks>().children)
                        {
                            if (Physics.BoxCast(child.transform.position, child.transform.lossyScale / 2.1f, Vector3.down, child.transform.rotation, 20, noground))
                            {
                                canfall = false;
                                Debug.Log(child.name + " Failed");
                            }
                            if (myhit.GetComponent<Blocks>().cost != 0)
                            {
                                //a shopitem will sew a block down as it must be connected to something stable
                                //shop items need to be connected if not, they wont place.
                                //shop items cannot be attached to other shopitems.
                                RaycastHit hit2;
                                if (Physics.BoxCast(child.transform.position, child.transform.lossyScale / 1.9f,  Vector3.down, out hit2, child.transform.rotation, 20, blockmask))
                                {
                                    canconnect = true;

                                    //when a shoptem lands next to another block, it will lock it down.
                                    if (hit2.transform.parent != null)
                                    {
                                        if (hit2.transform.parent.GetComponent<Blocks>() != null)
                                        {
                                            hit2.transform.parent.GetComponent<Blocks>().sewn += 1;
                                            myhit.GetComponent<Blocks>().lockthis = hit2.transform.parent.gameObject;
                                        }
                                    }
                                    else
                                    {
                                        if (hit2.transform.GetComponent<Blocks>() != null)
                                        {
                                            hit2.transform.GetComponent<Blocks>().sewn += 1;
                                            myhit.GetComponent<Blocks>().lockthis = hit2.transform.gameObject;
                                        }
                                    }
                                }
                            }
                        }
                        if (!canconnect)
                        {
                            Debug.Log("The shop item couldnt connect!");
                        }

                        if (canfall && canconnect)
                        {
                            if (myhit.GetComponent<Blocks>().grab2 == false)
                            {
                                myhit.layer = 7;
                            }
                            else
                            {
                                myhit.layer = 11;
                            }
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
                if (myhit.GetComponent<Blocks>().grab2 == false)
                {
                    myhit.layer = 7;
                }
                else
                {
                    myhit.layer = 11;
                }
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
        if (other.tag == "Finish")
        {
            SceneManager.LoadScene(other.GetComponent<NextLevel>().level);
        }
        if (other.tag == "Enemy")
        {
            Die();
        }
    }
    public void Die()
    {
        Scene reloadscene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(reloadscene.name);
    }

}
