using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(Button button)
    {
        string sceneName = button.name; 

        // if sceneName starts with "Level" then load the scene
        if (sceneName.StartsWith("Level"))
        {
            // load player mode
            SceneManager.LoadScene("LoadPlayerMode");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    public void QuitGame()
    {       
        Application.Quit();
    }
}
