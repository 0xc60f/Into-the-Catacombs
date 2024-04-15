using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject _healthCanvas;
    public void HomeMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayClick()
    {
        //Set the time scale to 1
        Time.timeScale = 1;
        //Hide the pause menu
        //Get the game object called Pause Menu
        GameObject pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        //Get the main camera's audio source
        AudioSource audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        audioSource.Play();
        _healthCanvas.SetActive(true);
    }
    public void RestartClick()
    {
        //Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Set the time scale to 1
        Time.timeScale = 1;
    }
}
