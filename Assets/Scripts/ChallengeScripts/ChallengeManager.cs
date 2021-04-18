//CHALLENGE MANAGER SCRIPT
//Use: Colective space to store challenge status
//Created By: Iain Farlow
//Created On: 02/04/2021
//Last Edited: 16/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeManager : MonoBehaviour
{
    bool m_challenge1;
    bool m_challenge2;
    bool m_challenge3;

    //Public function to set challenge to desired bool value
    public void Challenge1Complete(bool a_result)
    {
        m_challenge1 = a_result;
    }

    //public functions to set challenge bool to true when complete
    public void Challenge2Complete()
    {
        m_challenge2 = true;
    }

    public void Challenge3Complete()
    {
        m_challenge3 = true;
    }

    //Public functions t0 save overall statrus of challenges if they are compelte 
    public void SaveChallengeStatus()
    {        
        if(m_challenge1)
        {
            //save game data based on current level (build index) and current challenge
            SaveSystem.SaveLevelData(SceneManager.GetActiveScene().buildIndex, 1);
        }
        if (m_challenge2)
        {
            SaveSystem.SaveLevelData(SceneManager.GetActiveScene().buildIndex, 2);
        }
        if (m_challenge3)
        {
            SaveSystem.SaveLevelData(SceneManager.GetActiveScene().buildIndex, 3);
        }
    }
}
