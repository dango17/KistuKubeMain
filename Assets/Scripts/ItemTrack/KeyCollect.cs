//Daniel Oldham 
//S1903729
//Manages collision with Key, will turn on well box collider to allow
//transition to next level, also disables well particles & destroys key object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    public int KeysAmount = 1;
    public ParticleSystem ps;

    public GameObject collectEffect;
    public GameObject WellUnlock;

    public GameObject KeyOff;
    public AudioSource Keycollect; 

    public void OnTriggerEnter(Collider other)
    {
        Keycollect.Play(); 

        //Update UI with Value just collected
        CollectManager.instance.UpdateKeysUI(KeysAmount);
        CollectManager.instance.currentkeys += KeysAmount;

        //Enabled collider on well to move to next scene
        WellUnlock.GetComponent<BoxCollider>().enabled = true;

        //Disable loop on particle system 
        var main = ps.main;
        main.loop = false;

        //Instantiate collection effect
        Instantiate(collectEffect, transform.position, Quaternion.identity);

        //Disable the key model, allows sound to still play
        KeyOff.SetActive(false);
        //Disable box collider to stop several collisions 
        KeyOff.GetComponent<BoxCollider>().enabled = false;
    }
}
