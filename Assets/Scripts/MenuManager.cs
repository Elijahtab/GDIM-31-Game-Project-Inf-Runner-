using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// By: 
public class MenuManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
        //quit the application
    }
    public void Restart()
    {
        //Add code here to restart the game
        
        SceneManager.LoadScene(0);
        
      
    }
}
