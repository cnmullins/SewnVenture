/*
MenuManager.cs
Author: Christian Mullins
Date: 9/11/21
Summary: Handles all UI logic throughout the Main Menu scene.
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuUI;
    [SerializeField]
    private Dropdown _aspectRatioDD;
    [SerializeField]
    private RawImage _windowButton;
    [SerializeField]
    private Texture _windowedImage;
    [SerializeField]
    private Texture _fullscreenImage;


    private GameObject _curMenu;

    private void Start()
    {
        _curMenu = _mainMenuUI;
    }

    /// <summary>
    /// While in Main Menu, this is a way to toggle between menus.
    /// </summary>
    /// <param name="menuGO">Root empty GO that holds a menu.</param>
    public void FocusMenu(GameObject menuGO)
    {
        if (_curMenu.Equals(menuGO)) return;

        _curMenu.SetActive(false);
        menuGO.SetActive(true);
        _curMenu = menuGO;
        
        if (_curMenu.name.StartsWith("Options"))
        {
            //_windowButtonText.text = (Screen.fullScreen) ? "Window" : "Fullscreen";
            _windowButton.texture = (Screen.fullScreen) ? _windowedImage : _fullscreenImage;
            _SetAspectRatioDropDown();
        }
    }

    /// <summary>
    /// Simple way to quit out of the game.
    /// </summary>
    public void QuitGame()
    {
        //save data if necessary
        print("Quitting game...");
        Application.Quit();
    }

    /// <summary>
    /// Move to a scene and leave the previous scene to be destroyed.
    /// </summary>
    /// <param name="sceneName">Name of scene in build index.</param>
    /// <param name="additive">Defaulted off for singular scene control</param>
    public static void MoveToScene(string sceneName, bool additive=false)
    {
        SceneManager.LoadScene(sceneName, 
            (additive) ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    /// <summary>
    /// Non-static version of MoveToScene so that it can be used for UI.
    /// </summary>
    /// <param name="sceneName">Name of scene in build index.</param>
    /// <param name="additive">Defaulted off for singular scene control</param>
    public void MoveToScene_NS(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    //TODO:
        //Make an async function so that you can read the data and then display progress.

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
        #region OptionsFunctions

    /// <summary>
    /// Any saveable data that has been changed in the options, apply them here
    /// </summary>
    public void ApplyOptions()
    {
        //resolution
        string setResStr = _aspectRatioDD.captionText.text;
        //parse string to ints so you can actually set the resolution
        var oldRes = Screen.currentResolution;
        if (!setResStr.Equals(oldRes.width + " x "  + oldRes.height))
        {
            int xIndex, width, height;
            xIndex = setResStr.IndexOf('x');
            width = Convert.ToInt32(setResStr.Substring(0, xIndex).Trim());
            height = Convert.ToInt32(setResStr.Substring(xIndex + 1).Trim());
            Screen.SetResolution(width, height, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        }
        //sound
        //possible button remapping
        //etc.
        //TODO:
            //Pack options into object and submit to SaveManager
        
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        //change font text
        //_windowButtonText.text = (Screen.fullScreen) ? "Window" : "Fullscreen";
        _windowButton.texture = (Screen.fullScreen) ? _windowedImage : _fullscreenImage;
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SetAspectRatioDropDown()
    {
        _aspectRatioDD.ClearOptions();
        var arList = new List<string>();
        foreach (var res in Screen.resolutions)
        {
            arList.Add(res.width + " x " + res.height);
        }
        _aspectRatioDD.AddOptions(arList);
    }
    #endregion
}
