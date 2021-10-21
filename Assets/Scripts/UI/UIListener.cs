/*
UIListener.cs
Author: Christian Mullins
Date: 10/2/2021
Summary: Transfers all relevant value changes to the StandardCanvas.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListener : MonoBehaviour
{
    [SerializeField]
    private Text _threadText;

    private Movement _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    private void LateUpdate()
    {
        _threadText.text = "x " + _player.thread;
    }
}
