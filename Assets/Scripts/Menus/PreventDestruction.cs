//PREVENT DESTRUCTION SCRIPT
//Use: Allows music to keep playing between levels
//Created By: Iain Farlow
//Created On: 12/04/2021
//Last Edited: 12/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDestruction : MonoBehaviour
{
    private void Awake()
    {
        //Get gameobject music is attached to
        GameObject[] musicTagged = GameObject.FindGameObjectsWithTag("Music");
        //if there is moe than one destory
        if (musicTagged.Length > 1)
        {
            Destroy(this.gameObject);
        }

        //prevent the msuic being destroyed between levels
        DontDestroyOnLoad(this.gameObject);
    }
}
