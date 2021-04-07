using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinGame : MonoBehaviour
{
    public void NextLevel()
    {
        Debug.Log("Level Complete");
        SceneManager.LoadScene("MainMenu");
    }
}
