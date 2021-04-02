//Daniel Oldham 
//S1903729
//Manages what the player has collected within the game, will then update 
//UI elements as well as player score, also manages addition challenges in pause menu 
//UI bug with coin collection stems somewhere within this script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CollectManager : MonoBehaviour
{ 
    //Updating ints across scripts, singlton structure needed
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

    [Header("Challenge 2 icons")]
    //Challenge Icon ints 
    public GameObject Challenge2True;
    public GameObject Challenge2False;

    [Header("Challenge 3 icons")]
    //Challenge Icon ints 
    public GameObject Challenge3True;
    public GameObject Challenge3False;

    private void Awake()
    { 
        //Instance this, ints of values can be called in back into update functions 
        //from collsion scripts
        instance = this;

        //Challenge 2 starts false, will turn true once player has collected 3 coins
        Challenge2True.GetComponent<RawImage>().enabled = false;
        Challenge2False.GetComponent<RawImage>().enabled = true;

        //Challenge 3 starts false, will turn true once player has collected the key & a score of 150
        Challenge3True.GetComponent<RawImage>().enabled = false;
        Challenge3False.GetComponent<RawImage>().enabled = true;
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
        //Challenge 2, player has collected 3 coins, enable this as true 
        if(currentCoins > 3)
        {
            Challenge2True.GetComponent<RawImage>().enabled = true;
            Challenge2False.GetComponent<RawImage>().enabled = false;
        }
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

        //Player has the correct key & score amount, check challenge true
        if(KeysAmount > 1 && currentScore > 150)
        {
            Challenge3True.GetComponent<RawImage>().enabled = true;
            Challenge3False.GetComponent<RawImage>().enabled = false;
        }
    }
}
