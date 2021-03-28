﻿using System.Collections;
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
    public int levelScore;
    public int levelStars;

    public LevelData(int a_levelScore, int a_levelStars) //Pass in data here
    {
        levelScore = a_levelScore;
        levelStars = a_levelStars;
    }
}
