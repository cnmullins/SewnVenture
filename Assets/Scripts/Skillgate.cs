/*
Skillgate.cs
Author: Christian Mullins
Date: 11/21/2021
Summary: Dynamically adjusts scenes based on game progress for the purpose
    of skillgating the player.
*/
using UnityEngine;

public class Skillgate : MonoBehaviour
{
    void Start()
    {
        //check save manager
        if (!SaveManager.WasSwingLearned())
        {
            //player cannot swing on this
            gameObject.layer = 0;
        }
    }
}
