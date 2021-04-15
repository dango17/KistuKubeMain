//Daniel Oldham 
//S1903729
//Manages collision with coin, will update coin & score amount 
//back inside the CollectManager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CoinCollect : MonoBehaviour
{
    public int coinAmount = 1;
    public GameObject collectEffect;
    public GameObject TheCoin; 

    public AudioSource CoinCollection; 
    
    public void OnTriggerEnter(Collider other)
    {
        //Cannot destory gameobject, doesnt allow audio clip tp play, disable instead
        CoinCollection.Play();

        if (other.tag == "Player")
        {   
            //Update UI
            CollectManager.instance.UpdateCoinsUI(coinAmount);
            CollectManager.instance.currentCoins += coinAmount;
            //Instantiate particle effects
            Instantiate(collectEffect, transform.position, Quaternion.identity);
            //Disable the coin rather than destroy
            TheCoin.SetActive(false);
            //Disable Coins box collider to prevent more than on collision
            TheCoin.GetComponent<BoxCollider>().enabled = false; 
        }
    }
     
}
