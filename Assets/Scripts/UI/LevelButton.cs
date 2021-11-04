/*
LevelButton.cs
Author: Christian Mullins
Date: 9/28/21
Summary: Button 
*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public const float inactiveAlpha = 0.3f;
    public Utilities.SceneField sceneAsset;
    public string levelSceneName;
    
    [Header("Values that will manipulate visuals")]
    public string levelName;
    [Range(0, 3)]
    public int stars = 0;
    [Range(0, 7)]
    public int redThread = 0;
    public bool hasGoldThread = false;
    public bool isCompleted = false;

    [Header("Level Path Connections")]
    public RectTransform[] nextLevels;

    //UI that will be altered internally by code
    [Header("LevelButton child assets.")]
    [SerializeField] private Text _titleText;
    [SerializeField] private GameObject _starGroup;
    [SerializeField] private GameObject _redThreadGroup;
    [SerializeField] private GameObject _goldThreadObject;

    //properties
    public Room room { get {
        return transform.root.GetComponent<LevelSelectManager>().GetCurrentRoom();
    } }

    public void EditorRefresh()
    {
        /* Initialize Canvas */
        //update level text
        _titleText.text = levelName;
        //update stars, red thread, and gold thread
        var starImages = _starGroup.GetComponentsInChildren<Image>();
        var rThreadImages = _redThreadGroup.GetComponentsInChildren<Image>();
        Image gThreadImage = _goldThreadObject.GetComponentInChildren<Image>();
        
        _goldThreadObject.SetActive(hasGoldThread);
        /* Update Canvas Values from SaveData */
        //get save data or if not found
        var levelManager = transform.root.GetComponent<LevelSelectManager>();
        Room thisRoom = room;
        //apply them
        for (int i = 0; i < starImages.Length; ++i)
        {
            //only display if visible
            starImages[i].enabled = (i < stars);
            //set collected stars
            Color tempColor = starImages[i].color;
            tempColor.a = (i < stars) ? 1f : inactiveAlpha;
            starImages[i].color = tempColor;
        }
        for (int i = 0; i < rThreadImages.Length; ++i)
        {
            //only display if visible
            rThreadImages[i].enabled = (i < redThread);
            //set collected thread
            Color tempColor = rThreadImages[i].color;
            tempColor.a = (i < redThread) ? 1f : inactiveAlpha;
            rThreadImages[i].color = tempColor;
        }
        //SetActive(false) if these achievments are not in the level
        _starGroup.SetActive(!(stars == 0));
        _redThreadGroup.SetActive(!(redThread == 0));

        UpdateLevelPath();
    }

    /// <summary>
    /// Update UI to correspond with inspector and save data.
    /// </summary>
    public void RefreshValues(in LevelData data)
    {
        //empty level data, don't bother initializing
        if (data.levelHash == -1) return;

        /* Initialize Canvas */
        //update level text
        _titleText.text = levelName;
        //update stars, red thread, and gold thread
        var starImages = _starGroup.GetComponentsInChildren<Image>();
        var rThreadImages = _redThreadGroup.GetComponentsInChildren<Image>();
        //Image gThreadImage = _goldThreadObject.GetComponentInChildren<Image>();
        
        //_goldThreadObject.SetActive(hasGoldThread);
        /* Update Canvas Values from SaveData */
        //get save data or if not found
        var levelManager = transform.root.GetComponent<LevelSelectManager>();
        Room thisRoom = room;
        //apply them
        for (int i = 0; i < starImages.Length; ++i)
        {
            //only display if visible
            starImages[i].enabled = (i < stars);
            //set collected stars
            Color tempColor = starImages[i].color;
            tempColor.a = (i < data.starsCollected[0]) ? 1f : inactiveAlpha;
            starImages[i].color = tempColor;
        }
        for (int i = 0; i < rThreadImages.Length; ++i)
        {
            //only display if visible
            rThreadImages[i].enabled = (i < redThread);
            //set collected thread
            Color tempColor = rThreadImages[i].color;
            tempColor.a = (i < data.redThreadCollected[0]) ? 1f : inactiveAlpha;
            rThreadImages[i].color = tempColor;
        }
        //SetActive(false) if these achievments are not in the level
        _starGroup.SetActive(!(stars == 0));
        _redThreadGroup.SetActive(!(redThread == 0));

        /* UPDATE COLOR IF COMPLETED */
        Color notComplete = levelManager.levelInCompleteColor;
        Color complete = levelManager.levelCompleteColor;
        transform.GetChild(0).GetComponent<Image>().color = (data.completed) ? complete : notComplete;
        isCompleted = data.completed;

        UpdateLevelPath();
    }

    /// <summary>
    /// Updates Line Renderer to new values that have been input in the inspector.
    /// </summary>
    public void UpdateLevelPath()
    {
        var levelPath = GetComponent<LineRenderer>();
        levelPath.alignment = LineAlignment.View;
        int positions = nextLevels.Length * 2;
        levelPath.positionCount = positions;

        //make all available paths
        var levelStack = new Stack<RectTransform>(nextLevels);
        for (int i = 0; i < positions; ++i)
        {
            //set every other position as self and the other as the next level
            if (i % 2 == 1)
            {
                //if this "nextLevel" is just a button to the next room skip drawing it
                var nextButton = levelStack.Peek().GetComponentInChildren<Button>();
                if (!nextButton.onClick.GetPersistentMethodName(0).Equals("LoadLevel") 
                && !nextButton.onClick.GetPersistentMethodName(0).Equals("FocusMenu"))
                    ++i;
                else
                    levelPath.SetPosition(i, levelStack.Pop().position);
            }
            else
                levelPath.SetPosition(i, transform.position);
        }
        
    }

    /// <summary>
    /// Get the next levels from this current level as a LevelButton type.
    /// </summary>
    /// <returns>The next levels as a LevelButton array.</returns>
    public LevelButton[] GetNextLevelButtons()
    {
        var nlButtons = new List<LevelButton>();
        foreach (var l in nextLevels)
        {
            var lButton = l.GetComponentInChildren<Button>();
            if (lButton.onClick.GetPersistentMethodName(0).Equals("LoadLevel"))
                nlButtons.Add(l.GetComponent<LevelButton>());
        }
        return nlButtons.ToArray();
    }

    /// <summary>
    /// Loads level and maintains DataObserver for saving progress.
    /// </summary>
    public void LoadLevel()
    {
        //set up DataObserver
        //Start migrating
        var myData = GetLevelData();
        DontDestroyOnLoad(DataObserver.instance.gameObject);
        SceneManager.LoadSceneAsync((string)sceneAsset, LoadSceneMode.Single).completed += delegate 
        {
            DataObserver.instance.MigrateToLevel(myData);
        };
    }

    //getters
    /// <summary>
    /// Pack button as Level Data struct
    /// </summary>
    /// <returns>Class as a data struct.</returns>
    public LevelData GetLevelData()
    {
        var data = new LevelData(room, sceneAsset.sceneHash);
        data.levelRoom = room;
        data.name = levelName;
        data.starsCollected = new int[2] { 0, stars };
        data.redThreadCollected = new int[2] { 0, redThread };
        data.goldThreadCollected = new bool[2] { false, hasGoldThread };
        return data;
    }

    private void _IfThisPrint(string thisName, string msg)
    {
        if (name.Equals(thisName))
            print(msg);
    }
}
