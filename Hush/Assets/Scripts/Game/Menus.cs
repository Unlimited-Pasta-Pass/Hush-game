using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {
    
    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Credits() {
        // POP-UP FOR CREDITS
    }
    
    public void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("BACK TO MAIN MENU");
    }

    public void Quit() {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
