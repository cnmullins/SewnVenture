/*
SaveManager.cs
Author: Christian Mullins
Date: 9/23/21
Summary: Static class that interacts with PlayerPrefs for saving purposes.
*/
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    private const string _saveKey = "SaveData";

    /// <summary>
    /// Save progress to PlayerPrefs as Json.
    /// </summary>
    /// <param name="newData">New data to be saved.</param>
    public static void SaveProgress(in SaveData newData)
    {
        PlayerPrefs.SetString(_saveKey, JsonUtility.ToJson(newData));
    }

    /// <summary>
    /// Clear SaveData to overwrite empty data value to start over.
    /// </summary>
    public static void ClearSaveData() 
    {
        SaveProgress(new SaveData());
    }

    /// <summary>
    /// Statically grab progress that has been made.
    /// </summary>
    /// <returns>SavedData class with saved progress.</returns>
    public static SaveData RetrieveProgress()
    {
        string progress = PlayerPrefs.GetString(_saveKey, JsonUtility.ToJson(new SaveData()));
        return JsonUtility.FromJson<SaveData>(progress);
    }

    public static void InitializeLevelData(Room room)
    {
        
    }
}

public enum InputAction
{
    MoveForward, 
    MoveBack, 
    MoveRight, 
    MoveLeft, 
    Cut, 
    EnterSew,
    HintToggle
}

//to be turned into a JSON file
[Serializable]
public class SaveData
{
    public string[] customInput { get; private set; }
    public List<LevelData>[] levelDatas { get; private set; }

    public SaveData()
    {
        customInput = new string[]
        {//input corresponds to InputAction enum
            "w", "s", "d", "a", "space", "r", "q"
        };
        var numOfRooms = (int)Enum.GetValues(typeof(Room)).Cast<Room>().Last() + 1;
        levelDatas = new List<LevelData>[numOfRooms];
        for(int i = 0; i < numOfRooms; ++i)
            levelDatas[i] = new List<LevelData>(new LevelData[] { new LevelData() });
    }
}

[Serializable]
public class LevelData
{
    public Room room;
    public int buildIndex;
    public string name;
    /// <summary>
    /// (collected, possible)
    /// </summary>
    public Tuple<int, int> starsCollected;
    /// <summary>
    /// (collected, possible)
    /// </summary>
    public Tuple<int, int> redThreadCollected;
    /// <summary>
    /// (was collected, is in the level)
    /// </summary>
    public Tuple<bool, bool> goldThreadCollected;

    public LevelData()
    {
        buildIndex = -1;
        name = String.Empty;
        starsCollected = new Tuple<int, int>(0, 3);
        redThreadCollected = new Tuple<int, int>(0, 5);
        goldThreadCollected = new Tuple<bool, bool>(false, false);
    }
}

