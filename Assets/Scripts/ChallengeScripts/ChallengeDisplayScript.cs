//CHALLENGE DISPLAY SCRIPT
//Use: Attached to challenges in pause screen (alters display based on previous success with challenge)
//Created By: Iain Farlow
//Created On: 02/04/2021
//Last Edited: 16/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeDisplayScript : MonoBehaviour
{
    [SerializeField]
    int m_challengeNum;
    // Start is called before the first frame update
    void Start()
    {
        //Get level data
        GameData gameData = SaveSystem.LoadLevelData();

        //Check there were no issues when loading level data (see Save System Script)
        if (gameData != null)
        {
            //Check currently level data does exist
            if (gameData.levels[SceneManager.GetActiveScene().buildIndex - 1] != null)
            {
                //check if current challenge is complete
                if (gameData.levels[SceneManager.GetActiveScene().buildIndex - 1].challenage[m_challengeNum - 1])
                {
                    //make challenge text green - complete
                    this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    //keep challenge text white - not complete 
                    this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
            }
        }
        else
        {
            //keep challenge text white - not complete 
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
