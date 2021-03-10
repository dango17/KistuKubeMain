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
        hostileGameObjects = GameObject.FindGameObjectsWithTag("Hostile");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTurn)
        {
            if (player == null)
            {
                Debug.Log("Player Dead");
            }
            else if (!player.GetComponent<PlayerController>().PlayerMove())
            {
                currentHostile = 0;
                
                PlayerTurn = false;
            }
        }
        else
        {
            if (currentHostile < hostileGameObjects.Length)
            {
                if (hostileGameObjects[currentHostile] == null)
                {
                    currentHostile++;
                }
                else if (!hostileGameObjects[currentHostile].GetComponent<HostileMoveScript>().HostileMove())
                {
                    currentHostile++;
                }
            }
            else
            {
                currentTurn++;
                PlayerTurn = true;
            }
        }
    }
}
