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
    public Image blackoutImage;
    private Movement _player;
    private static Image _blackout;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        _blackout = blackoutImage;
    }

    private void LateUpdate()
    {
        _threadText.text = "x " + _player.thread;
    }

    public static IEnumerator FadeScreen()
    {
        _blackout.enabled = true;
        for (float i = 0f; i < 1f; i += 0.1f)
        {
            var tempColor = _blackout.color;
            tempColor.a = i;
            _blackout.color = tempColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
    }
}
