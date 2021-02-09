using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    GameObject[] hostileGameObjects;
    GameObject player;

    bool PlayerTurn = true;

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
            Debug.Log("Player: " + player.GetComponent<PlayerController>().PlayerMove());
            if (!player.GetComponent<PlayerController>().PlayerMove())
            {
                PlayerTurn = false;
            }
        }
        else
        {
            foreach (GameObject hostile in hostileGameObjects)
            {
                Debug.Log("Hostile: " + hostile.GetComponent<HostileMoveScript>().HostileMove());
                if (!hostile.GetComponent<HostileMoveScript>().HostileMove())
                {
                    PlayerTurn = true;
                }
            }
        }
    }
}
