using Common.Enums;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player.PlayerInputManager;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public GameObject pauseMenuUI;
    
        private static PlayerInputManager Input => PlayerInputManager.Instance;

        private void OnEnable()
        {
            Input.reference.actions[Actions.Pause].performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            if (Input != null) 
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