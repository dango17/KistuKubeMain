using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuScreen;

    public void PausedGame()
    {
        if(GameIsPaused)
        {
            Resume();
        } 
        else
        {
            Pause();
        }
    } 

    public void Resume()
    {
        PauseMenuScreen.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    } 

    void Pause()
    {
        PauseMenuScreen.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true; 
    } 

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    } 

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApplication()
    {
        Debug.Log("Qutting game...");
        Application.Quit();
    }
}
