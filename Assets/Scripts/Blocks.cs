using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
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
    private Vector3[] _detectorExtents;

    
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

        _detectorExtents = new Vector3[detectorMeshRends.Length];
        for (int i = 0; i < detectorMeshRends.Length; ++i)
        {
            detectorMeshRends[i].GetComponent<BoxCollider>().enabled = true;
            _detectorExtents[i] = detectorMeshRends[i].GetComponent<BoxCollider>().bounds.extents;
            detectorMeshRends[i].GetComponent<BoxCollider>().enabled = false;
        }
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
        for (int i = 0; i < detectorMeshRends.Length; ++i)
        {
            //var myBC = mRend.GetComponent<Collider>();
            Vector3 extents = _detectorExtents[i];
            if (cost != 0) extents *= 1.1f;
            //swap axis if rotated
            if (rotated) extents = new Vector3(extents.z, extents.y, extents.x);
            //print("extents: " + extents);
            //var hits = Physics.BoxCastAll(detectorMeshRends[i].transform.position, extents, Vector3.down, mRend.transform.rotation);
            var hits = Physics.OverlapBox(detectorMeshRends[i].transform.position, extents, detectorMeshRends[i].transform.rotation);
            for (int ii = 0; ii < hits.Length; ++ii)
            {
                switch (hits[ii].transform.gameObject.layer)
                {
                    
                    case 6: //Held layer
                    case 8: if (cost == 0) valid = true; //Ground layer
                        continue;
                    case 11: //Grabbable2 layer
                    case 14://NoPlace layer
                    case 16: //Wreck layer
                    case 0: //Default layer
                        
                        //make sure if this object has a cost that it influences feedback
                        if (cost != 0)
                        {
                            valid = true;
                        }
                        else
                        {
                            if (_IsColliderPenetrated(detectorMeshRends[i].GetComponent<Collider>(), hits[ii]))
                                valid = true;
                            else
                                return false;
                        }
                        continue;
                    default: continue;
                }
            }
            if (hits.Length == 0 && cost == 0)
            {
                valid = true;
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

    //not implemented
    [System.Obsolete]
    public IEnumerator FeedbackUntilButtonUp(Material feedbackMat)
    {
        foreach (var mRend in detectorMeshRends)
            mRend.material = feedbackMat;
        yield return new WaitUntil(delegate { return !Input.GetMouseButton(0); });
        foreach (var mRend in detectorMeshRends)
            mRend.material = curMat;
        //detectorMeshRend.material = curMat;
    }

    private bool _IsColliderPenetrated(Collider col1, Collider col2)
    {
        Physics.ComputePenetration(
            col1, col1.transform.position, col1.transform.rotation,
            col2, col2.transform.position, col2.transform.rotation,
            out var dir, out var distancePenetrated);
        //print("distacePenetrated: " + distancePenetrated);
        return distancePenetrated < 1f;
    }

}
