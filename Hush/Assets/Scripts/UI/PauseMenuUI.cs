using Common.Enums;
using Game;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public GameObject pauseMenuUI;
        // [SerializeField] private GameObject savingFeedbackText;
    
        private static InputManager Input => InputManager.Instance;

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
                Input.reference.actions[Actions.Pause].performed += TogglePauseMenu;
            
            pauseMenuUI.SetActive(false);
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
                Input.reference.actions[Actions.Pause].performed -= TogglePauseMenu;
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

        public void Save()
        {
            bool saved = SaveGameManager.Instance.OnSave();
            ShowSavingFeedback(saved);
        }
        
        public void LoadMainMenu()
        {
            SceneManager.Instance.LoadMainMenu();
        }

        private void ShowSavingFeedback(bool saved)
        {
            if(saved)
                Debug.Log("Saved.");
            else
                Debug.Log("Save Failed.");
            
            // Keeping this here in case it becomes worth it to do this again
            // TextMeshProUGUI saving = savingFeedbackText.GetComponentInChildren<TextMeshProUGUI>();
            //
            // if (saved)
            //     saving.text = "Saved.";
            // else
            //     saving.text = "Save Failed.";
            //
            // savingFeedbackText.SetActive(true);
            // Invoke(nameof(HideSavingFeedback), 1f);   
        }
        
        // private void HideSavingFeedback()
        // {
        //     Debug.Log("Hide");
        //     savingFeedbackText.SetActive(false);
        // }
    }
}
