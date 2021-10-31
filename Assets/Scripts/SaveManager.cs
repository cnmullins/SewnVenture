/*
SaveManager.cs
Author: Christian Mullins
Date: 9/23/21
Summary: Static class that interacts with PlayerPrefs for saving purposes.
*/
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InputAction
{
    MoveForward, 
    MoveBack, 
    MoveRight, 
    MoveLeft, 
    Cut, 
    EnterSew,
    HintToggle,
    ZoomIn,
    ZoomOut
}

public static class SaveManager
{
    public static bool doesSaveFileExist { get {
        return File.Exists(_savePath);
    } }
    
    private const string _saveKey = "SaveData";
    private const string _saveFile = "SaveData";
    private static string _savePath { get { 
        return Application.persistentDataPath + "/savedata.json";
    } }
    private static string _debugPath(in string fileType=".txt") 
    {
        return Application.persistentDataPath + "/debug" + fileType;
    }
    /// <summary>
    /// Save progress to PlayerPrefs as Json.
    /// </summary>
    /// <param name="newData">New data to be saved.</param>
    public static void SaveProgress(in SaveData newData)
    {
        string newSave = JsonConvert.SerializeObject(newData, Formatting.Indented);
        File.WriteAllText(_savePath, newSave);
    }

    /// <summary>
    /// Statically grab progress that has been made.
    /// </summary>
    /// <returns>SavedData class with saved progress.</returns>
    public static SaveData RetrieveProgress()
    {
        //string progress = PlayerPrefs.GetString(_saveKey, JsonUtility.ToJson(new SaveData()));
        //return JsonUtility.FromJson<SaveData>(progress);

        if (File.Exists(_savePath))
        {
            //Debug.Log("ferp: " + Application.persistentDataPath);
            
            //return JsonUtility.FromJson<SaveData>(File.ReadAllText(_savePath));
            //Debug.Log("File found!");
            return JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(_savePath));
        }
        return new SaveData();
    }

    public static bool IsSaveFileOpen()
    {
        try
        {
            using (var fStream = File.Open(_savePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                fStream.Close();
            }
        } catch (IOException){ return true; }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static Dictionary<int, LevelData> SavedRoomData(in Room room)
    {
        return RetrieveProgress().levelHashTables[(int)room];
    }

    public static void SubmitLevelData(int hash, LevelData levelData)
    {
        var curProg = RetrieveProgress();
        int roomIndex = (int)levelData.levelRoom;
        if (curProg.levelHashTables[roomIndex].ContainsKey(hash))
        {
            curProg.levelHashTables[roomIndex][hash] = levelData;
            //Debug.Log("overwritting level data at: " + hash);
        }
        else
        {
            curProg.levelHashTables[roomIndex].Add(hash, levelData);
            //Debug.Log("creating new save data at: " + hash);
        }
        SaveProgress(curProg);
    }
}

//to be turned into a JSON file
[Serializable]
public class SaveData
{
    public OptionsData options;
    public Dictionary<int, LevelData>[] levelHashTables;

    public SaveData()
    {
        options = new OptionsData();
        var numOfRooms = 5;
        levelHashTables = new Dictionary<int, LevelData>[numOfRooms];
        for(int i = 0; i < numOfRooms; ++i)
            levelHashTables[i] = new Dictionary<int, LevelData>();
    }
}

[Serializable]
public class LevelData
{
    public string name;
    /// <summary>
    /// (collected, possible)
    /// </summary>
    public int[] starsCollected;
    /// <summary>
    /// (collected, possible)
    /// </summary>
    public int[] redThreadCollected;
    /// <summary>
    /// (was collected, is in the level)
    /// </summary>
    public bool[] goldThreadCollected;
    public bool completed;
    public int levelHash;
    public Room levelRoom;

    public LevelData()
    {
        levelHash = -1;
        levelRoom = Room.NULL;
        name = String.Empty;
        completed = false;
        starsCollected = new int[2] { 0, 3 };
        redThreadCollected = new int[2] { 0, 5 };
        goldThreadCollected = new bool[2] { false, false };
    }

    public LevelData(Room room, int hash)
    {
        levelHash = hash;
        levelRoom = room;
        name = String.Empty;
        completed = false;
        starsCollected = new int[2] { 0, 3 };
        redThreadCollected = new int[2] { 0, 5 };
        goldThreadCollected = new bool[2] { false, false };
    }

    /// <summary>
    /// Take two argument data and return whichever object is calculated to be more complete.
    /// </summary>
    /// <returns>Most complete object of the two parameter datas.</returns>
    public static LevelData ReturnMoreComplete(LevelData data1, LevelData data2)
    {
        //not implemented
        return null;
    }

    /// <summary>
    /// Reset values for resources without overwriting scene related values.
    /// </summary>
    public void ClearCollectedValues()
    {
        starsCollected = new int[2] { 0, starsCollected[1] };
        redThreadCollected = new int[2] { 0, redThreadCollected[1] };
        goldThreadCollected = new bool[2] { false, goldThreadCollected[1] };
        completed = false;
    }
}

[Serializable]
public class OptionsData {
    public string[] customInput;
    public Resolution currentResolution;
    public float musicVol;
    public float soundFXVol;

    public OptionsData() {
        customInput = new string[]
        {//input index corresponds to InputAction enum
            "w", "s", "d", "a", "space", "r", "q", "up", "down"
        };
        currentResolution = Screen.currentResolution;
        musicVol = 1f;
        soundFXVol = 1f;
    }
}
