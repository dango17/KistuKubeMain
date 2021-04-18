//TURN MANAGER SCRIPT
//Use: Controls when the player and hostiles are active
//Created By: Iain Farlow
//Created On: 06/01/2021
//Last Edited: 19/03/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    GameObject[] hostileGameObjects;
    GameObject player;

    bool PlayerTurn = true;

    private int currentHostile = 0;
    private int currentTurn = 0;

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    // Start is called before the first frame update
    void Start()
    {
        //collect all instances of the hostile in the level
        hostileGameObjects = GameObject.FindGameObjectsWithTag("Hostile");
        //get the player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if its the players turn
        if (PlayerTurn)
        {
            //there shoudl always be a player in the scene. Just for testing
            if (player == null)
            {
                Debug.Log("Player Dead");
            }
            //If the player is finsihed with their turn
            else if (!player.GetComponent<PlayerController>().PlayerMove()) //Calls playerMove in PlayerController
            {
                //set to first hostile
                currentHostile = 0;
                //set turn to not player's (hostiles')
                PlayerTurn = false;
            }
        }
        else
        {
            //while current hostile is within the amount of hostiles
            if (currentHostile < hostileGameObjects.Length)
            {
                //if hostile is missing
                if (hostileGameObjects[currentHostile] == null)
                {
                    //next hostile
                    currentHostile++;
                }
                //if the hostile has finished its turn
                else if (!hostileGameObjects[currentHostile].GetComponent<HostileMoveScript>().HostileMove()) //calls hostile move in hostileMoveScirpt
                {
                    //next hostile
                    currentHostile++;
                }
            }
            //if all hostiles has done
            else
            {
                //up current turn
                currentTurn++;
                //set players turn to true
                PlayerTurn = true;
            }
        }
    }
}
