//GAME DATA SCRIPT
//Use: Classes for storing data in save system
//Created By: Iain Farlow
//Created On: 27/03/2021
//Last Edited: 01/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public LevelData[] levels = new LevelData[3];
    //add settings variables here
}

[System.Serializable]
public class LevelData
{
    //Add variables to save here
    //Does not accept unity variables like vector, only base c++
    public bool[] challenage = new bool[3];

    public LevelData(int a_challenage) //Pass in data here
    {
        //reduced by one as call 1 is 0 in array
        challenage[a_challenage - 1] = true;
    }
}
