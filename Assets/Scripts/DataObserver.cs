/*
DataObserver.cs
Author: Christian Mullins
Date: 10/2/2021
Summary: Records data changes by player and passes it to SaveManager for
    persistant data saving.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataObserver : MonoBehaviour
{
    public static DataObserver instance;
    public LevelData currentPlayData { get; private set; }
    public LevelData savedLevelData { get; private set; }
    public int curLevelHash { get; private set; }
    private GameObject _playerGO;

    private void Start()
    {
        //singleton
        if (DataObserver.instance == null)
        {
            DataObserver.instance = this;
            //initialize save data
            currentPlayData = null;
            savedLevelData = null;
            _playerGO = null;
            curLevelHash = -1;
        }
        else
            Destroy(gameObject);
    }

    #region Incrementor_Functions
    /// <summary>
    /// Increment the Red Thread value while maintaining it as a Tuple.
    /// </summary>
    public void IncrementRedThread() 
    {
        currentPlayData.redThreadCollected[0] += 1;
    }

    /// <summary>
    /// Increment the Star value while maintaining it as a Tuple.
    /// </summary>
    public void IncrementStar() 
    {
        currentPlayData.starsCollected[0] += 1;
    }

    /// <summary>
    /// Set Gold Thread value while maintaining it as a Tuple.
    /// </summary>
    /// <param name="found">Set value</param>
    public void IsGoldThreadFound(in bool found) 
    {
        if (found)
            currentPlayData.goldThreadCollected = new bool[2] { true, true };
        else
            currentPlayData.goldThreadCollected = new bool[2] { false, currentPlayData.goldThreadCollected[1] };
    }
    #endregion

    public void SetCompletion(in bool complete)
    {
        currentPlayData.completed = complete;
        if (complete)
            SubmitOverwriteData();
    }

    public void MigrateToLevel(LevelData levelData)
    {
        //check for new save data
        //throw yield warning if scene is not found in PlayerPrefs
            //then create new save version
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        savedLevelData = levelData;
        currentPlayData = levelData;
        curLevelHash = levelData.levelHash;
        currentPlayData.ClearCollectedValues();
    }

    /// <summary>
    /// Combine existing data and new data for the overwrite data.
    /// </summary>
    /// <returns>New LevelData to be saved.</returns>
    public void SubmitOverwriteData()
    {
        LevelData overwriteData = currentPlayData;
        //overwriteData.ClearCollectedValues();
        overwriteData.starsCollected[0] = Math.Max(currentPlayData.starsCollected[0], savedLevelData.starsCollected[0]);
        overwriteData.redThreadCollected[0] = Math.Max(currentPlayData.redThreadCollected[0], savedLevelData.redThreadCollected[0]);
        overwriteData.completed = true;
        SaveManager.SubmitLevelData(overwriteData.levelHash, overwriteData);
    }
}
