using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject levelSelectUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused == true && levelSelectUI.activeSelf == true)
            {
                GoBack();
            }
            else if (gamePaused == true && levelSelectUI.activeSelf == false)
            {
                Resume();
            } 
            else 
            {
                Pause();
            }
        }
    }

    public void GoBack()
    {
        pauseMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        levelSelectUI.SetActive(false);

        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
