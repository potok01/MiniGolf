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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            SceneManager.LoadSceneAsync("Empty Level");
        }  
   }

   public void QuitGame()
   {       
       Application.Quit();
   }
}
