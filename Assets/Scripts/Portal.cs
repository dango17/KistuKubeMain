using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject Player;
    public GameObject TeleportDestination;
   // public GameObject TeleportStart; 

    public void OnTriggerEnter(Collider collision)
    { 
        if(collision.gameObject.CompareTag("Teleporter 1"))
        {
            Player.transform.position = TeleportDestination.transform.position;
        }       
       // if(collision.gameObject.CompareTag("Teleporter 2"))
     //   {
       //     Player.transform.position = TeleportStart.transform.position; 
     //   }
    } 
}
