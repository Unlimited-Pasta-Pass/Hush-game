using System;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseFirstButton;

    private void OnEnable()
    {
        PlayerInputManager.Instance.reference.actions[Actions.Pause].performed += TogglePauseMenu;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.reference.actions[Actions.Pause].performed -= TogglePauseMenu;
    }

    private void TogglePauseMenu(InputAction.CallbackContext obj)
    {
        if (TimeManager.Instance.GameIsPaused)
            Resume();
        else
            Pause();
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