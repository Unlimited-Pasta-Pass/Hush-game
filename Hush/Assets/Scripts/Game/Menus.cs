using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Game;

public class Menus : MonoBehaviour
{
    
    // brackeys tutorial: https://youtu.be/JivuXdrIHK0
    public GameObject pauseMenuUI;
    // controller nav https://www.youtube.com/watch?v=SXBgBmUcTe0&ab_channel=gamesplusjames
    public GameObject pauseFirstButton;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Pause()
    {
        TimeManager.Instance.Pause();
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        Debug.Log("Resume");
        TimeManager.Instance.Resume();
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuUI.SetActive(false);
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
