using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinAmount = 1;
    public GameObject collectEffect;
    
    public void OnTriggerEnter(Collider other)
    {     
        CollectManager.instance.UpdateCoinsUI(coinAmount);
        CollectManager.instance.currentCoins += coinAmount;     
        
        Instantiate(collectEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);      
    }
}
