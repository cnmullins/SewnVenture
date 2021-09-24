/*
DialogueEvent.cs
Author: Christian Mullins
Date: 9/23/21
Summary: Same as HintEvent, just allows for speaker parameter.
*/
using UnityEngine;

public class DialogueEvent : HintEvent
{
    public string speaker;

    protected override void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && _pressed)
        {
            _controller.SetDialogue(speaker, textStr); // only change
            _controller.hintGO.SetActive(false);
        }
    }
}