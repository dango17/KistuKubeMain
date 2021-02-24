using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CollectManager : MonoBehaviour
{
    public static CollectManager instance; 
    public int currentCoins;
    public Text coinValueTxt; 

    private void Awake()
    {
        instance = this;    
    }
 
    void Start()
    {     
        coinValueTxt.text = currentCoins.ToString() + "/3";
    } 

    public void UpdateCoinsUI(int coinAmount)
    {
        currentCoins += coinAmount; 
        coinValueTxt.text = currentCoins.ToString() + "/3"; 
    }
}
