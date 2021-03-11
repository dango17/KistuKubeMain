using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MoveToNextLevel : MonoBehaviour
{
    public void NextLevel()
    {
        Debug.Log("Level Complete");
        SceneManager.LoadScene("Level2");
    }
}
