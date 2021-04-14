using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeManager : MonoBehaviour
{
    bool m_challenge1;
    bool m_challenge2;
    bool m_challenge3;

    public void Challenge1Complete(bool a_result)
    {
        m_challenge1 = a_result;
    }

    public void Challenge2Complete()
    {
        m_challenge2 = true;
    }

    public void Challenge3Complete()
    {
        m_challenge3 = true;
    }

    public void SaveChallengeStatus()
    {        
        if(m_challenge1)
        {
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
