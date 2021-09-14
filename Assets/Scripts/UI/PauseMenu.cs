/*
PauseMenu.cs
Author: Christian Mullins
Date: 9/14/21
Summary: Handles all logic throughout the activation of Pause Menu.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseGO;

    public bool paused { get; private set; }

    private GameObject _curMenu;

    private void Start() 
    {
        paused = false;
    }

    /// <summary>
    /// Used only for input collection of pause action
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu(!paused);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="activate"></param>
    public void TogglePauseMenu(bool activate)
    {
        Time.timeScale = (activate) ? 0f : 1f;
        _pauseGO.SetActive(activate);
        paused = activate;
    }

    /// <summary>
    /// 
    /// </summary>
    public void QuitGame()
    {
        StartCoroutine("_SaveAndQuitCo");
    }

    //WIP
    //may need to change to "async" because of Time.timeScale usage.
    private IEnumerator _SaveAndQuitCo()
    {
        //Time.timeScale = 1f; //add for Coroutine only
        SceneManager.sceneUnloaded += delegate 
        {
            //save here?
        };
        print("Saving...");
        //wait till sure saving operation is completed.
        yield return new WaitForSeconds(1f);
        MenuManager.MoveToScene("MainMenu");
    }
}
