//Daniel Oldham 
//S1903729
//Adds score to CollectManager to whatever obj this script 
//is attached too, typically coins & the key

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollect : MonoBehaviour
{
    public int ScoreAmount = 100;

    public void OnTriggerEnter(Collider other)
    {
        CollectManager.instance.UpdateScoreUI(ScoreAmount);
        CollectManager.instance.currentCoins += ScoreAmount;
    }
}
