/*
MenuManager.cs
Author: Christian Mullins
Date: 9/11/21
Summary: Handles all UI logic throughout the Main Menu scene.
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuUI;

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
}
