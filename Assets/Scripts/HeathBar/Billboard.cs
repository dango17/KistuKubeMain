//BILLBOARD SCRIPT
//Use: Keeps healthbar looking at player
//Created By: Iain Farlow
//Created On: 07/02/2021
//Last Edited: 07/02/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //late update as to prevent gitter when moving
    void LateUpdate()
    {
        //Mkae the healthbar face the main camera
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
