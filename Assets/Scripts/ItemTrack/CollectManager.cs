using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CollectManager : MonoBehaviour
{
    public static CollectManager instance;  

    //Coin Values
    public int currentCoins;
    public Text coinValueTxt;

    //Current score values
    public int currentScore;
    public Text scoreValueTxt;

    //Current Keys Value
    public int currentkeys;
    public Text keyValueTxt; 

    private void Awake()
    { 
        //Instance this, ints of values can be called in back into update functions 
        //from collsion scripts
        instance = this;    
    }
 
    void Start()
    { 
        //Print all strings to the UI
        coinValueTxt.text = currentCoins.ToString() + "";
        scoreValueTxt.text = currentScore.ToString() + "";
        keyValueTxt.text = currentkeys.ToString() + "/1";
    } 

    public void UpdateCoinsUI(int coinAmount)
    {
        //Collection detected, add and update value to UI
        currentCoins += coinAmount; 
        coinValueTxt.text = currentCoins.ToString() + ""; 
    } 

    public void UpdateScoreUI(int scoreAmount)
    {
        //Collection detected, add and update value to UI
        currentScore += scoreAmount;
        scoreValueTxt.text = currentScore.ToString() + "";
    } 

    public void UpdateKeysUI(int KeysAmount)
    {
        //Collection detected, add and update value to UI
        currentkeys += KeysAmount;
        keyValueTxt.text = currentkeys.ToString() + "/1";
    }
}
