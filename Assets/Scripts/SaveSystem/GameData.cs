using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public LevelData[] levels;
    //add settings variables here
}

[System.Serializable]
public class LevelData
{
    //Add variables to save here
    //Does not accept unity variables like vector, only base c++
    public bool[] challenage;

    public LevelData(int a_challenage) //Pass in data here
    {
        challenage[a_challenage - 1] = true;
    }
}
