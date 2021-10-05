using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int cost = 0;
    public GameObject Purchase;
    public TextMeshPro mytext;
    public void Start()
    {
        mytext.text = "" + cost;
    }
}
