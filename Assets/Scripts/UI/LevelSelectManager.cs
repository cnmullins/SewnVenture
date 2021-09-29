/*
MenuManager.cs
Author: Christian Mullins
Date: 9/28/21
Summary: Handles all UI logic throughout the Level Select
*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Room
{
    Sewing, Living, Porch, Kitchen, Outdoor
}

public class LevelSelectManager : MonoBehaviour
{
    [Tooltip("Menu order:\n-SewingRoom\n-LivingRoom\n-Porch\n-Kitchen\n-Outdoor")]
    [SerializeField]
    private GameObject[] _roomMenus;
    public GameObject curMenu { get; private set; }

    private void Start()
    {

    }

    /// <summary>
    /// While in Main Menu, this is a way to toggle between menus.
    /// </summary>
    /// <param name="menuGO">Root empty GO that holds a menu.</param>
    public void FocusMenu(GameObject menuGO)
    {
        if (curMenu.Equals(menuGO)) return;
        /*
            TODO
                When loading a given menu find all LevelButtons and update values
                based on save data.
        */
        curMenu.SetActive(false);
        menuGO.SetActive(true);
        curMenu = menuGO;
    }

    /// <summary>
    /// Go back to Main Menu and save necessary data.
    /// </summary>
    public void ReturnToMainMenu()
    {
        //save data here if necessary
        MenuManager.MoveToScene("MainMenu");
    }

    /// <summary>
    /// Check if the player should logically be able to move to the next room.
    /// </summary>
    /// <param name="newRoomGO">GameObject of room in question.</param>
    /// <returns>True if the player can access room.</returns>
    private bool _CanPlayerProgressTo(in GameObject newRoomGO) 
    {
        var foundIndex = new List<GameObject>(_roomMenus).IndexOf(newRoomGO);
        switch ((Room)foundIndex) 
        {
            case Room.Sewing: break;
            case Room.Living:
                break;
            case Room.Porch:
                break;
            case Room.Outdoor:
                break;
            default: 
                Debug.LogError("GameObject: " + newRoomGO.name + " is not a valid menu.");
                return false;
        }
        return true;
    }
}