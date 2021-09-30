/*
LevelButton.cs
Author: Christian Mullins
Date: 9/28/21
Summary: Button 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string levelName;
    [Range(0, 3)]
    public int stars = 0;
    //[Range(0, 5)]
    //public int redThread = 0;
    public bool hasGoldThread = false;

    //UI that will be altered internally by code
    [SerializeField] private Text _titleText;
    [SerializeField] private GameObject _starGroup;
    //[SerializeField] private GameObject _redThreadGroup;
    [SerializeField] private GameObject _goldThreadObject;

    private void Start()
    {
        
    }

    /// <summary>
    /// Update UI to correspond with inspector and save data.
    /// </summary>
    public void RefreshValues()
    {
        //update level text
        _titleText.text = levelName;
        //update stars
        var starImages = GetComponentsInChildren<Image>();
        for (int i = 0; i < stars; ++i)
            starImages[i].enabled = true;
        //TODO: red thread
        //update if has gold thread
        if (hasGoldThread)
        {

        }
    }

    //public LevelData GetLevelData()
    //{
    //    return new LevelData();
    //}
}
