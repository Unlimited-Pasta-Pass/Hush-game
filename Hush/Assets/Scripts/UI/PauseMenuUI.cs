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
            if (Input != null && Input.reference != null)
                Input.reference.actions[Actions.Pause].performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
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
        }
    
        public void Resume()
        {
            TimeManager.Instance.Resume();
            pauseMenuUI.SetActive(false);
        }
    }
}
