//SPIKES SCRIPT
//Use: Classes for storing data in save system
//Created By: Iain Farlow
//Created On: 10/03/2021
//Last Edited: 20/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private GameObject spikes;

    private GameObject levelControler;
    private int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        //get the level controller
        levelControler = GameObject.Find("LevelController");
        //Start spikes as up
        spikes.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //get the current turn from the turn manager
        currentTurn = levelControler.GetComponent<TurnManagerScript>().GetCurrentTurn();
        //every 3 turns
        if (currentTurn % 3 == 0)
        {
            //spikes set active
            spikes.SetActive(true);
        }
        else
        {
            //spikes are down
            spikes.SetActive(false);
        }
        //if the spikes are up
        if (spikes.activeInHierarchy)
        {
            RaycastHit hit;
            Debug.DrawRay(spikes.transform.position + (Vector3.up * 0.1f), transform.forward, Color.red);
            //raycast up
            if (Physics.Raycast(spikes.transform.position + (transform.forward * 0.2f), transform.forward, out hit, 1.0f))
            {
                //Debug.Log(hit.transform.gameObject.name);
                //If it hits the player or hostile kill it
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    hit.transform.gameObject.GetComponent<PlayerController>().DealDamage();
                }
                if (hit.transform.gameObject.CompareTag("Hostile"))
                {
                    hit.transform.gameObject.GetComponent<HostileMoveScript>().DealDamage();
                }
            }
        }
    }
}
