using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    // brackeys tutorial: https://youtu.be/JivuXdrIHK0

    public GameObject pauseMenuUI;
    
    // controller nav https://www.youtube.com/watch?v=SXBgBmUcTe0&ab_channel=gamesplusjames
    public GameObject pauseFirstButton;
    
    
    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         if (TimeManager.Instance.GameIsPaused())
    //         {
    //             Resume();
    //         }
    //         else
    //         {
    //             Pause();
    //         }
    //     }
    // }

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
    
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Quitting game/Back to main menu");
        Application.Quit();
    }
}