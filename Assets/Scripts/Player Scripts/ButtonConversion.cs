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
        if (Application.platform == RuntimePlatform.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftPressed()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ButtonDown = true;
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
