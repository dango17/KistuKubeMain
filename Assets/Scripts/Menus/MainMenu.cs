//Daniel Oldham 
//S1903729
//Allows maviagtion between scenes from the main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("CubeRotationMechanic");
    } 

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Qutting game...");
    }
}
