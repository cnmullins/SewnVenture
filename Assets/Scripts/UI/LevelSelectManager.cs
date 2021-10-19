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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Room
{
    NULL = -1, Sewing, Living, Porch, Kitchen, Outdoor
}

public class LevelSelectManager : MonoBehaviour
{
    [Header("Customize color of level UI.")]
    public Color levelCompleteColor;
    public Color levelInCompleteColor;
    [Header("Assign GameObjects to List")]
    [Tooltip("Menu order:\n-SewingRoom\n-LivingRoom\n-Porch\n-Kitchen\n-Outdoor")]
    [SerializeField]
    private GameObject[] _roomMenus;
    public GameObject curMenu { get; private set; }

    private List<GameObject> _nextRoomGO;

    /*
        Below acts as the main driver to initialize all visual feedback for the level select menu.
    */
    private IEnumerator Start()
    {
        curMenu = _roomMenus[0];
        _nextRoomGO = new List<GameObject>(GameObject.FindGameObjectsWithTag("MoveRoom"));
        print("0");
        //_UpdateRoomValues();
        yield return new WaitWhile(delegate 
        {
            if (SaveManager.doesSaveFileExist)
                return SaveManager.IsSaveFileOpen();
            return false;
        });
        //yield return new WaitForSeconds(1f);
        print("1");
        _UpdateRoomValues();
    }

    /// <summary>
    /// Getter for the current room as an enum.
    /// </summary>
    /// <returns>Current room.</returns>
    public Room GetCurrentRoom()
    {
        for (int i = 0; i < _roomMenus.Length; ++i) 
            if (_roomMenus[i] == curMenu)
                return (Room)i;
        return Room.NULL;
    }

    /// <summary>
    /// While in Main Menu, this is a way to toggle between menus.
    /// </summary>
    /// <param name="menuGO">Root empty GO that holds a menu.</param>
    public void FocusMenu(GameObject menuGO)
    {
        if (curMenu.Equals(menuGO)) return;
        curMenu.SetActive(false);
        menuGO.SetActive(true);
        curMenu = menuGO;
        _UpdateRoomValues();
    }

    /// <summary>
    /// Go back to Main Menu and save necessary data.
    /// </summary>
    public void ReturnToMainMenu()
    {
        //save data here if necessary
        //TODO: Look for changes and apply if changes found.
        //var roomLevels = GameObject.FindObjectsOfType<LevelButton>(false);
        //SaveManager.RetrieveProgress()
        //SaveManager.SaveProgress(progress);
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Refresh values of all the UI buttons in this room by checking SaveData.
    /// </summary>
    private void _UpdateRoomValues()
    {
        var roomLevels = GameObject.FindObjectsOfType<LevelButton>(false);
        print("rrromLev: " + roomLevels.Length);
        //construct level paths as LinkedLists (start with "NoNextLevelers")
        var noNextLevels = Array.FindAll(roomLevels, level => level.GetNextLevelButtons().Length == 0);
        var levelPaths = new List<LinkedList<LevelButton>>();
        int pathCounter = 0;
        foreach (LevelButton nnl in noNextLevels)
        {

            levelPaths.Add(new LinkedList<LevelButton>());
            levelPaths[pathCounter].AddFirst(nnl); 
            LevelButton prevLevel = nnl;
            while (prevLevel != null)
            {
                levelPaths[pathCounter].AddFirst(prevLevel);
                prevLevel = Array.Find(
                    roomLevels, level => Array.Find(
                        level.GetNextLevelButtons(), l => l.Equals(prevLevel)));
            }
            ++pathCounter;
        }

        //check SaveData and refresh "completed" levels
        var levelHT = SaveManager.RetrieveProgress().levelHashTables[(int)GetCurrentRoom()];
        var completedLevels = new List<LevelButton>();
        bool levelCompleted = false;
        foreach (LevelButton l in roomLevels)
        {
            if (l.sceneAsset.sceneHash == -1) continue;
            if (levelHT.TryGetValue(l.sceneAsset.sceneHash, out var data))
            {
                //l.RefreshValues(data); //try
                completedLevels.Add(l);
                levelCompleted = true;
            }
        }
        //if SaveData is empty for said room, find LevelButton with NO nextLevel
        LevelButton noNext;
        if (!levelCompleted)
        {
            foreach (var l in roomLevels)
                if (l.nextLevels.Length == 0)
                {
                    print("ferp---------------------");
                    noNext = l;
                    break;
                }
        }

        bool roomStarted = false;
        //construct a list of completed and next levels and refresh all values
            //else if level is NOT in list, SetActive(false)/deactivate them from reach
        foreach (LevelButton level in roomLevels)
        {
            if (completedLevels.Contains(level))
            {
                level.RefreshValues(levelHT[level.sceneAsset.sceneHash]);
                //print(level.levelName+ " has remained ACTIVE.");
                //display appropriate next levels
                for (int i = 0; i < level.nextLevels.Length; ++i)
                {
                    if (level.nextLevels[i].TryGetComponent<LevelButton>(out var lb))
                        lb.RefreshValues(new LevelData());
                    else if (level.nextLevels[i].TryGetComponent<LevelButton>(out var b))
                        b.gameObject.SetActive(false);
                    //print("refreshing: " + level.nextLevels[i].name);
                }
            }
            else if (!completedLevels.Find(l => l.nextLevels.Contains(level.GetComponent<RectTransform>())))
            {
                level.gameObject.SetActive(false);
                //print(level.levelName + " has be DEACTIVATED.");
            }
            //hide access to new rooms if appropriate
            if (!level.isCompleted)
            {
                foreach (var nLevel in level.nextLevels)
                    if (nLevel.CompareTag("MoveRoom"))
                        nLevel.gameObject.SetActive(false);
            }
            else 
                roomStarted = true;
        }
        //completely new to room
        if (!roomStarted)
        {
            foreach (var path in levelPaths)
            {
                path.First.Value.gameObject.SetActive(true);
            }
            //print(levelPaths[0].First.Value.name + " is first in the levPath");
        }
    }//end _UpdateRoomValues()
}
