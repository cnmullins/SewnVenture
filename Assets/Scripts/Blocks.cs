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
    
    public GameObject lockthis;
    public int bugs;

    public Material curMat { get; private set; }
    private LayerMask _noGroundLayers;
    private LayerMask _blockLayer;

    public void Start()
    {
        _noGroundLayers = LayerMask.GetMask("Default", "TransparentFX", "Water", "UI", 
            "Held", "Grabbable", "Unstable", "Unstable2", "NoPlace", "Wreck");
        _blockLayer = LayerMask.GetMask("Default", "Player", "Unstable", "Unstable2", "Wreck");
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
        bool valid = true;
        foreach (var c in children)
        {
            if (Physics.BoxCast(c.transform.position - (Vector3.up * 3),
                                c.transform.lossyScale / 2.1f,
                                Vector3.down, c.transform.rotation,
                                displace - 3f,
                                _noGroundLayers))
            {
                valid = false;
            }
            if (cost != 0 && Physics.BoxCast(c.transform.position,
                                c.transform.lossyScale / 1.9f,
                                Vector3.down, c.transform.rotation,
                                20f, _blockLayer))
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
}