using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CollectManager : MonoBehaviour
{
    public static CollectManager instance;  

    public int currentCoins;
    public Text coinValueTxt;

    public int currentScore;
    public Text scoreValueTxt;

    public int currentkeys;
    public int WinKeyValue = 1;
    public Text keyValueTxt; 

    private void Awake()
    {
        instance = this;    
    }
 
    void Start()
    {
        coinValueTxt.text = currentCoins.ToString() + "";
        scoreValueTxt.text = currentScore.ToString() + "";
        keyValueTxt.text = currentkeys.ToString() + "/1";
    } 

    public void UpdateCoinsUI(int coinAmount)
    {
        currentCoins += coinAmount; 
        coinValueTxt.text = currentCoins.ToString() + ""; 
    } 

    public void UpdateScoreUI(int scoreAmount)
    {
        currentScore += scoreAmount;
        scoreValueTxt.text = currentScore.ToString() + "";
    } 

    public void UpdateKeysUI(int KeysAmount)
    {
        currentkeys += KeysAmount;
        keyValueTxt.text = currentkeys.ToString() + "/1";
        if(WinKeyValue > currentkeys)
        {
            Debug.Log("Well Unlocked -- Next Level");
            GameObject.Find("WellFBX_Variant").GetComponent<ParticleSystem>().enableEmission = false; 

        }
    }
}
