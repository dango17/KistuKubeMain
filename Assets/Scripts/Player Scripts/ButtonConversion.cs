//BUTTON CONVERSION SCRIPT
//Use: For mobile use buttons are used to control the player
//Created By: Iain Farlow
//Created On: 14/04/2021
//Last Edited: 16/04/2021
//Edited By: Iain Farlow
//Due to limitations when controlling for mobile alternative method for player control had to be used
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ButtonConversion : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //Check if current platform is android
        if (Application.platform == RuntimePlatform.Android)// || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            //get the player
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            //if not android hide buttons
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //button presses
    public void LeftPressed()
    {
        //get the player controller script from the player
        PlayerController playerController = player.GetComponent<PlayerController>();
        //let the player controller buttons have been pressed
        playerController.ButtonDown = true;
        //let the player controller what button has been pressed
        playerController.ButtonPressed = "Left";
    }
    public void UpPressed()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ButtonDown = true;
        playerController.ButtonPressed = "Up";

    }
    public void DownPressed()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ButtonDown = true;
        playerController.ButtonPressed = "Down";

    }
    public void RightPressed()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ButtonDown = true;
        playerController.ButtonPressed = "Right";

    }
}
