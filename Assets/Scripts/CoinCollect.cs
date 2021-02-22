using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinAmount = 1;
    
    public void OnTriggerEnter(Collider col)
    {
        CollectManager.instance.UpdateCoinsUI(coinAmount);
        CollectManager.instance.currentCoins += coinAmount;
        Destroy(gameObject);      
    }
}
