using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    //for debugging purposes
#if UNITY_EDITOR
    public bool debugDraw = false;
    //private RaycastHit[] hits;

    private void OnDrawGizmos() 
    {
        if (debugDraw)
        {
            foreach (var mRend in detectorMeshRends)
            {
                Gizmos.DrawWireCube(mRend.transform.position, mRend.GetComponent<BoxCollider>().bounds.extents);
            }
        }
    }
#endif

    public int sewn = 0;
    public bool stuck;
    public int cost;
    public bool grab2;
    public bool rotated;
    public string Upwards;
    public float displace;
    public List<GameObject> children;
    public List<GameObject> sewnToMe;
    //public List<Material> mats;
    public MeshRenderer[] detectorMeshRends;
    
    public GameObject lockthis;
    public int bugs;

    public Material curMat { get; private set; }


    public void Start()
    {
        if (detectorMeshRends.Length == 0)
            Debug.LogError("DetectorMeshRend was not assigned for " + name);

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
        curMat = detectorMeshRends[0].material;
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

    /// <summary>
    /// Uses detector's positioning to see if this place is valid to place this block.
    /// </summary>
    /// <returns>Validity of placement.</returns>
    public bool IsPlacementValid()
    {
        bool valid = false;
        foreach (var mRend in detectorMeshRends)
        {
            Vector3 extents = GetComponent<Collider>().bounds.extents * 0.99f;
            //swap axis if rotated
            if (rotated) extents = new Vector3(extents.z, extents.y, extents.x);
            print("extents: " + extents);
            //DebugDrawBox(transform.position, extents);
            var hits = Physics.BoxCastAll(transform.position, extents, Vector3.down, transform.rotation);
            for (int i = 0; i < hits.Length; ++i)
            {
                switch (hits[i].transform.gameObject.layer)
                {
                    case 14: return false;
                    case 0: return false;
                    case 6: valid = true;
                        continue;
                    default: continue;
                }
            }    
        }
        return valid;
    }

    /// <summary>
    /// Sets current material and adjust values.
    /// </summary>
    /// <param name="newMat">Material to be applied.</param>
    public void SetMaterialFeedback(in Material newMat)
    {
        foreach (var mRend in detectorMeshRends)
        {
            mRend.material = newMat;
        }
        curMat = newMat;
    }

    /// <summary>
    /// Christian's lame attempt to debug boxcasts
    /// </summary>
    /// <param name="center">Position of center of box cast.</param>
    /// <param name="size"></param>
    public static void DebugDrawBox()
    {
        /*
        var boxColor = Color.magenta;
        //top left
        //top right
        //bottom left
        //bottom right
        for (int x = 0; x < 3; ++x)
        {
            for (int y = 0; y < 3; ++y)
            {
                float posNegX = (x % 2 == 0) ? 1f : -1f;
                float posNegY = (y % 2 == 0) ? 1f : -1f;
                Debug.DrawLine(center + (posNegX * size), center - (posNegY * size), Color.magenta);
            }
        }
        */
    }

    public IEnumerator FeedbackUntilButtonUp(Material feedbackMat)
    {
        foreach (var mRend in detectorMeshRends)
            mRend.material = feedbackMat;
        yield return new WaitUntil(delegate { return !Input.GetMouseButton(0); });
        foreach (var mRend in detectorMeshRends)
            mRend.material = curMat;
        //detectorMeshRend.material = curMat;
    }

}
