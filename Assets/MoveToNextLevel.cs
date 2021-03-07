using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MoveToNextLevel : MonoBehaviour
{
    public void OnTriggerEnter()
    {
        SceneManager.LoadScene("Level2");
    }
}
