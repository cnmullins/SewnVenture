/*
DialogueController.cs
Author: Christian Mullins
Date: 9/21/21
Summary: Controls dialogue UI that will be manipulate for text.
*/
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance; //singleton
    [SerializeField]
    private GameObject _hintGO;
    [SerializeField]
    private GameObject _dialogueGO;

    private Text _textObj;

    public GameObject dialogueGO => _dialogueGO;
    public GameObject hintGO => _hintGO;
    public bool isTriggered { get; private set; }

    private void Awake()
    {
        instance = this;
        isTriggered = false;
        _textObj = _dialogueGO.GetComponentInChildren<Text>();
    }

    //use for displaying hint/thought text
    public void SetText(string textStr)
    {
        _dialogueGO.SetActive(true);
        _textObj.text = textStr;
    }

    //use for displaying dialogue
    public void SetDialogue(string speakerName, string dialogueStr)
    {
        _dialogueGO.SetActive(true);
        _textObj.text = "<b>" + speakerName + "</b>: " + dialogueStr;
    }
}
