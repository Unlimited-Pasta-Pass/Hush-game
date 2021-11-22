using Common.Enums;
using Game;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public GameObject pauseMenuUI;
    
        private static InputManager Input => InputManager.Instance;

        private void OnEnable()
        {
            Input.reference.actions[Actions.Pause].performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            Input.reference.actions[Actions.Pause].performed -= TogglePauseMenu;
        }

        private void Start()
        {
            pauseMenuUI.SetActive(false);
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
            pauseMenuUI.SetActive(true);
        
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    
        private void Resume()
        {
            TimeManager.Instance.Resume();
            pauseMenuUI.SetActive(false);
        
            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}