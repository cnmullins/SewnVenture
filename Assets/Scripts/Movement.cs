using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Movement : MonoBehaviour
{
    [Range(4f, 6f)]
    public float speed;
    public Camera mycam;
    public Material mat;
    public Material mat2;
    public GameObject myhit;
    public bool holdblock = false;
    [Tooltip("Feedback object for block placement.")]
    public GameObject detector;
  
    public Vector3 savedpos;

    public LayerMask laymask;
    public LayerMask onlyground;
    public LayerMask currentlayermask;
    public LayerMask noground;
    public LayerMask sewmask;
    public LayerMask blockmask;
    public LayerMask walkmask;

    public bool sewing;
    public GameObject overlay;

    public int thread;

    public Vector3 aimpoint;

    public Vector3 savedeulers;
    public string savedup;
    //used for casting 

    public bool canfall;
    public bool canconnect;
    public float vertdisplace;

    public bool hassew;

    //current scene used for reloads;
    //public Scene reloadscene;

    //swinging below
    public bool swinging;
    public bool swingX;
    public Animator swinganim;
    public float swingduration;
    public Vector3 mydest;

    public GameObject highlight;
    public int counthighlight;

    [Tooltip("Parent of object that holds the particle system.")]
    public GameObject deathParticles;
    [Tooltip("Transform of the model GameObject")]
    public Transform modelTrans;
    [Header("Block color material feedback")]
    public Material greenTransMat;
    public Material redTransMat;
    private bool _isDead;
    private MeshRenderer _detectorMeshRend;
    private GameObject _gridGO;

    // Start is called before the first frame update
    void Start()
    {
        currentlayermask = laymask;
        _isDead = false;
        _gridGO = GameObject.FindGameObjectWithTag("Grid");
        _gridGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //don't execute Update if dead
        if (_isDead) return;
        if (!swinging)
        {
            if (Input.GetKey(KeyCode.UpArrow) && mycam.orthographicSize < 10f)
            {
                mycam.orthographicSize += 0.05f * Time.deltaTime * 60;
                overlay.transform.localScale += new Vector3(0.01f * Time.deltaTime * 60, 0.01f * Time.deltaTime * 60, 0.01f * Time.deltaTime * 60);
            }
            if (Input.GetKey(KeyCode.DownArrow) && mycam.orthographicSize > 2.5f)
            {
                mycam.orthographicSize -= 0.05f * Time.deltaTime * 60;
                overlay.transform.localScale -= new Vector3(0.01f * Time.deltaTime * 60, 0.01f * Time.deltaTime * 60, 0.01f * Time.deltaTime * 60);
            }
            Debug.DrawLine(transform.position, transform.forward);
            if (!sewing)
            {
                //basic movement, it checks if you can move and also checks if you will fall off.
                //if you wont then you move.
                var outDir = Vector3.zero;
                if (Input.GetKey("w") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.forward, transform.rotation, 0.6f, blockmask) && Physics.Raycast(transform.position + Vector3.forward * 0.4f, Vector3.down, 1.9f, walkmask))
                {
                    outDir += Vector3.forward;
                }
                if (Input.GetKey("s") && !Physics.BoxCast(transform.position, new Vector3(0.5f, 0, 0), Vector3.back, transform.rotation, 0.6f, blockmask) && Physics.Raycast(transform.position + Vector3.back * 0.4f, Vector3.down, 1.9f, walkmask))
                {
                    outDir += Vector3.back;
                }
                if (Input.GetKey("a") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.left, transform.rotation, 0.6f, blockmask) && Physics.Raycast(transform.position + Vector3.left * 0.4f, Vector3.down, 1.9f, walkmask))
                {
                    outDir += Vector3.left;
                }
                if (Input.GetKey("d") && !Physics.BoxCast(transform.position, new Vector3(0, 0, 0.5f), Vector3.right, transform.rotation, 0.6f, blockmask) && Physics.Raycast(transform.position + Vector3.right * 0.4f, Vector3.down, 1.9f, walkmask))
                {
                    outDir += Vector3.right;
                }
                transform.position += outDir.normalized * speed * Time.deltaTime;
                //rotate to movement direction
                if (outDir != Vector3.zero)
                {
                    modelTrans.LookAt(modelTrans.position + outDir, Vector3.up);
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
                    _gridGO.SetActive(true);
                }
            }
            else if (Input.GetKeyDown("r") && sewing && !holdblock)
            {
                sewing = false;
                overlay.gameObject.SetActive(false);
                _gridGO.SetActive(false);
                if (myhit != null)
                {
                    myhit.GetComponent<MeshRenderer>().material = mat2;
                    myhit = null;
                }
            }
        }
        //this is the main raycast, it goes towards the mouse.
        //it detects what blocks are being hit and lets you move them
        //the layermask will swap when a block is being held for better movement.
        RaycastHit hit;
        Ray ray = (mycam.ScreenPointToRay(Input.mousePosition));
        if (!sewing && !swinging)
        {
            RaycastHit Sew;
            Physics.Raycast(ray, out Sew, Mathf.Infinity);
            if (Sew.transform != null && Input.GetMouseButtonDown(0))
            {
                if (Sew.transform.gameObject.layer == 15 && Vector3.Distance(transform.position,new Vector3(Sew.transform.position.x,transform.position.y,Sew.transform.position.z)) < 10)
                {
                    swinging = true;
                    mydest = (new Vector3(((Sew.transform.position.x - transform.position.x) * 2) + transform.position.x, transform.position.y, ((Sew.transform.position.z - transform.position.z) * 2) + transform.position.z));
                    mydest -= transform.position;
                    GetComponent<Rigidbody>().useGravity = false;
                    //animateholder.transform.position = transform.position;
                    //transform.SetParent(animateholder.transform);
                    swinganim.SetTrigger("Swing");
                    swingduration = 50;
                }
            }
        }
        if (swinging)
        {
            //transform.position += (mydest / 100);
            swingduration -= Time.deltaTime * 50;
            if (swingduration > 10 && swingduration < 40)
            {
                transform.position += mydest / 30 * Time.deltaTime * 50;
            }
            if (swingduration <= 0)
            {
                swinging = false;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentlayermask) && sewing)
        {
            if (holdblock)
            {
                //Set color feedback of projected block
                //print("holding!!!!!!!!");
                Blocks detectBlock = detector.transform.root.GetComponentInChildren<Blocks>();
                var outMat = (detectBlock.IsPlacementValid()) ? greenTransMat : redTransMat;
                if (detectBlock.curMat != outMat)
                {
                    detectBlock.SetMaterialFeedback(outMat);
                }
                

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
                    
                    //myhit.transform.eulerAngles += new Vector3(0, 90, 0);
                    myhit.transform.RotateAround(myhit.transform.position, Vector3.up, 90);
                    myhit.transform.GetChild(0).transform.position = myhit.transform.position - Vector3.up * myhit.GetComponent<Blocks>().displace;
                }
                //the secondary rotation is very confusing, essentially it rotates the block on the X axis rather than the Y
                //the main point of confusion is keeping the block's height logged so it can snap back into place correctly.
                //it uses both the fact if the block has been rotated normally as WELL as editing it when an X->Z or a Z->X rotation is done 
                //with all of this it works correctly.
                if (Input.GetKeyDown("e"))
                {
                    if (myhit.GetComponent<Blocks>().Upwards == "x")
                    {
                        //myhit.GetComponent<Blocks>().Upwards = "y";
                        if (myhit.GetComponent<Blocks>().rotated == false)
                        {
                            vertdisplace = myhit.transform.localScale.z;
                            myhit.GetComponent<Blocks>().Upwards = "z";
                            myhit.GetComponent<Blocks>().rotated = true;
                        }
                        else
                        {
                            vertdisplace = myhit.transform.localScale.y;
                            myhit.GetComponent<Blocks>().Upwards = "y";
                        }
                    }
                    else if (myhit.GetComponent<Blocks>().Upwards == "y")
                    {
                        //myhit.GetComponent<Blocks>().Upwards = "y";
                        if (myhit.GetComponent<Blocks>().rotated == false)
                        {
                            vertdisplace = myhit.transform.localScale.z;
                            myhit.GetComponent<Blocks>().Upwards = "z";
                        }
                        else
                        {
                            vertdisplace = myhit.transform.localScale.x;
                            myhit.GetComponent<Blocks>().Upwards = "x";
                        }
                    }
                    else
                    {
                        //myhit.GetComponent<Blocks>().Upwards = "y";
                        if (myhit.GetComponent<Blocks>().rotated == false)
                        {
                            vertdisplace = myhit.transform.localScale.y;
                            myhit.GetComponent<Blocks>().Upwards = "y";
                        }
                        else
                        {
                            vertdisplace = myhit.transform.localScale.x;
                            myhit.GetComponent<Blocks>().Upwards = "x";
                            myhit.GetComponent<Blocks>().rotated = false;
                        }
                    }

                    myhit.transform.RotateAround( myhit.transform.position,Vector3.right, 90);
                    myhit.transform.GetChild(0).transform.position = myhit.transform.position - Vector3.up * myhit.GetComponent<Blocks>().displace;
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
                    /*if (myhit.tag != "SewnDown" && highlight != null)
                    {
                        counthighlight = 0;
                        foreach (GameObject Sewer in myhit.GetComponent<Blocks>().sewnToMe)
                        {
                            highlight = null;
                            myhit.GetComponent<Blocks>().mats.Add(Sewer.GetComponent<MeshRenderer>().material);
                            myhit.GetComponent<MeshRenderer>().material = mat;
                            counthighlight += 1;
                        }
                    }
                    if (myhit.tag == "SewnDown" && highlight != myhit) 
                    {
                        foreach (GameObject Sewer in myhit.GetComponent<Blocks>().sewnToMe)
                        {
                            highlight = myhit;
                            myhit.GetComponent<Blocks>().mats.Add(Sewer.GetComponent<MeshRenderer>().material);
                            myhit.GetComponent<MeshRenderer>().material = mat;
                        }
                    }*/
                    if (Input.GetMouseButtonDown(0) && myhit.tag == "Moveable")
                    {
                        //when you click on a movable object in sewing mode
                        //the game will change it to being moved
                        //it resets the material, change the layer
                        //and moves the object to the mouses position.
                        //it then sets the layermask for easier movement
                        if (myhit.transform.GetChild(0).gameObject != null)
                        {
                            if (myhit.GetComponent<Blocks>().Upwards == "")
                            {
                                myhit.GetComponent<Blocks>().Upwards = "y";
                            }
                            savedeulers = myhit.transform.eulerAngles;
                            if (myhit.GetComponent<Blocks>().Upwards == "x")
                            {
                                vertdisplace = myhit.transform.localScale.x;
                            }
                            else if (myhit.GetComponent<Blocks>().Upwards == "y")
                            {
                                vertdisplace = myhit.transform.localScale.y;
                            }
                            else
                            {
                                vertdisplace = myhit.transform.localScale.z;
                            }
                            //if an upwards hasnt been set, set it to Y.
                           
                            detector = myhit.transform.GetChild(0).gameObject;
                            _detectorMeshRend = detector.GetComponent<MeshRenderer>();
                            myhit.GetComponent<MeshRenderer>().material = mat2;
                            savedpos = myhit.transform.position;
                            savedup = myhit.GetComponent<Blocks>().Upwards;
                            myhit.layer = 6;
                            holdblock = true;
                            myhit.transform.position += (myhit.transform.position - detector.transform.position);
                            currentlayermask = onlyground;
                            //if this item is currently sewing something down (as its a shop item)
                            //free the object.
                            if (myhit.GetComponent<Blocks>().lockthis != null)
                            {
                                myhit.GetComponent<Blocks>().lockthis.GetComponent<Blocks>().sewnToMe.Remove(myhit.gameObject);
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
                        myhit.GetComponent<Blocks>().Upwards = "y"; //this specific line is for rotation
                        detector = myhit.transform.GetChild(0).gameObject;
                        _detectorMeshRend = detector.GetComponent<MeshRenderer>();
                        myhit.GetComponent<MeshRenderer>().material = mat2;
                        savedpos = myhit.transform.position;
                        myhit.layer = 6;
                        vertdisplace = myhit.transform.localScale.y;
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
                    aimpoint.x = Mathf.Round(aimpoint.x*2);
                    aimpoint.z = Mathf.Round(aimpoint.z*2);
                    aimpoint.x /= 2;
                    aimpoint.z /= 2;
                    //Debug.Log(hit.transform.localScale);
                    /*if ((myhit.transform.localScale.x % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == false) || (myhit.transform.localScale.z % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == true))
                    {
                        aimpoint.x += 0.5f;
                    }
                    if ((myhit.transform.localScale.z % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == false) || (myhit.transform.localScale.x % 2 >= 1 && myhit.GetComponent<Blocks>().rotated == true))
                    {
                        aimpoint.z += 0.5f;
                    }*/

                    myhit.transform.position = new Vector3(aimpoint.x, (aimpoint.y) + (vertdisplace / 2), aimpoint.z)  + (myhit.transform.position-detector.transform.position); 
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
                        hassew = true;
                        foreach (GameObject child in myhit.GetComponent<Blocks>().children)
                        {
                            if (Physics.BoxCast(child.transform.position-(new Vector3(0, 3, 0)), child.transform.lossyScale / 2.1f, Vector3.down, child.transform.rotation, (myhit.GetComponent<Blocks>().displace - 3f), noground))
                            {
                                canfall = false;
                                Debug.Log(child.name + " Failed");
                                //StartCoroutine(child.GetComponentInParent<Blocks>().FeedbackUntilButtonUp(greenTransMat));
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
                                    if (canfall && hassew)
                                    {
                                        //when a shoptem lands next to another block, it will lock it down.
                                        if (hit2.transform.parent != null)
                                        {
                                            if (hit2.transform.parent.GetComponent<Blocks>() != null)
                                            {
                                                hit2.transform.parent.GetComponent<Blocks>().sewn += 1;
                                                hit2.transform.parent.GetComponent<Blocks>().sewnToMe.Add(myhit.gameObject);
                                                myhit.GetComponent<Blocks>().lockthis = hit2.transform.parent.gameObject;
                                            }
                                        }
                                        else
                                        {
                                            if (hit2.transform.GetComponent<Blocks>() != null)
                                            {
                                                hit2.transform.GetComponent<Blocks>().sewnToMe.Add(myhit.gameObject);
                                                hit2.transform.GetComponent<Blocks>().sewn += 1;
                                                myhit.GetComponent<Blocks>().lockthis = hit2.transform.gameObject;
                                            }
                                        }
                                        hassew = false;
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
                            myhit = null;
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
            if (myhit.GetComponent<Blocks>().cost == 0 || myhit.GetComponent<Blocks>().bugs != 0)
            {
                myhit.transform.position = savedpos;
                myhit.transform.eulerAngles = savedeulers;
                myhit.GetComponent<Blocks>().Upwards = savedup;
                myhit.transform.GetChild(0).transform.position = myhit.transform.position - Vector3.up * myhit.GetComponent<Blocks>().displace;
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
        if (other.tag == "Star")
        {
            //star
            Destroy(other.gameObject);
            DataObserver.instance.IncrementStar();
        }
        if (other.tag == "Finish")
        {
            SceneManager.LoadScene(other.GetComponent<NextLevel>().level);
        }
        if (other.tag == "Enemy")
        {
            StartCoroutine(Die());
        }
    }
    
    public IEnumerator Die()
    {
        _isDead = true;
        deathParticles.SetActive(true);
        modelTrans.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        deathParticles.GetComponentInChildren<ParticleSystem>().Stop();
        yield return UIListener.FadeScreen();
        deathParticles.SetActive(false);
        Scene reloadscene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(reloadscene.name);
    }

}
