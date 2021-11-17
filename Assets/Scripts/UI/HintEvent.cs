/*
HintEvent.cs
Author: Christian Mullins
Date: 9/21/21
Summary: Listens for input from collisions and coordinates logic to 
    DialogueController.
*/
using System;
using UnityEngine;

public class HintEvent : MonoBehaviour
{
    [TextArea(5, 10)]
    public string textStr;

    protected static DialogueController _controller;
    protected static bool _pressed;

    protected void Start()
    {
        _pressed = false;
        //throw error if scene format is lacking the appropriate prefab
        try
        {
            _controller = DialogueController.instance;
        } 
        catch (NullReferenceException)
        {
            Debug.LogError("DialogueCanvas prefab was not found in scene.");
        }
    }

    //gathering input in Update instead of running input in physics
    protected void Update() 
    {
        if (Input.GetKeyDown("q"))
            _pressed = true;
        if (Input.GetKeyUp("q"))
            _pressed = false;
    }

    //collisions should only account for logic of the player
    protected void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals("Player"))
        {
            _controller.hintGO.SetActive(true);
            print("ferp");
        }
    }

    protected virtual void OnTriggerStay(Collider other) 
    {
        if (other.tag.Equals("Player") && _pressed)
        {
            _controller.SetText(textStr);
            _controller.hintGO.SetActive(false);
        }
    }

    protected void OnTriggerExit(Collider other) 
    {
        if (other.tag.Equals("Player"))
        {
            _controller.hintGO.SetActive(false);

            if (_controller.dialogueGO.activeInHierarchy)
                _controller.dialogueGO.SetActive(false);
        }
    }
}
