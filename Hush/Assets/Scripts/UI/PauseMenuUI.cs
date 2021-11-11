using Common;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseFirstButton;

    private void Awake()
    {
        PlayerInputManager.Instance.reference.actions[Actions.Pause].performed += context =>
        {
            if (TimeManager.Instance.GameIsPaused)
                Resume();
            else
                Pause();
        };
    }

    private void Pause()
    {
        TimeManager.Instance.Pause();
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        
        pauseMenuUI.SetActive(true);
    }
    
    private void Resume()
    {
        TimeManager.Instance.Resume();
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuUI.SetActive(false);
    }
}