/*
Sewing.cs
Author: Christian Mullins
Date: 9/14/21
Summary: Main script that will handle sewing and unsewing for the player.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sewing : MonoBehaviour
{
    [SerializeField]
    private Text feedbackText;
    //[SerializeField]
    //private Image reticle

    public bool isSewing { get; private set; }
    public bool isUnsewing { get; private set; }


    private void Start()
    {
        feedbackText.gameObject.SetActive(false);
    }

    private void Update()
    {
        /*
            TODO:
                -use a mouse/target reticle to differentiate between
                -walking, sewing, and unsewing modes?
        */
        //sew
        if (Input.GetKeyDown("q"))
            ToggleSewMode(true);
        else if (Input.GetKeyUp("q"))
            ToggleSewMode(false);
        //unsew
        else if (Input.GetKeyDown("e"))
            ToggleUnsewMode(true);
        else if (Input.GetKeyUp("e"))
            ToggleUnsewMode(false);
    }

    public void ToggleSewMode(bool activate)
    {

    }

    public void ToggleUnsewMode(bool activate)
    {

    }

    private IEnumerator _DisplayFadeText()
    {
        return null;
    }

}
