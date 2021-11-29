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
        Game.SceneManager.Instance.LoadNextScene();
    }
    
    public void Replay()
    {
        Game.SceneManager.Instance.LoadPreviousScene();
    }

    void Pause()
    {
        TimeManager.Instance.Pause();
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        TimeManager.Instance.Resume();
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuUI.SetActive(false);
    }

    public void Credits() {
        // POP-UP FOR CREDITS
        Debug.Log("CREDITS");
    }
    
    public void MainMenu() {
        Game.SceneManager.Instance.LoadMainMenu();
        Debug.Log("BACK TO MAIN MENU");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Game.SceneManager.Instance.QuitGame();
    }
}
