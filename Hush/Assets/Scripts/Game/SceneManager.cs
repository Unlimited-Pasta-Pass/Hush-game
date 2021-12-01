using UnityEngine;

namespace Game
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
        }

        public void LoadNextScene()
        {
            // ignore scene state reinitialization if main menu 
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
            {
                ReinitializeSceneState();
            }
            
            // real code for incrementing which level we're at
            // var updatedLevel = GameManager.Instance.IncrementLoadedScene();
        
            // For dev scene
            LoadDevScene();
        }

        public void LoadScene(int sceneIndex)
        {
            // ignore scene state reinitialization if main menu 
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(sceneIndex).name != "MainMenu")
            {
                ReinitializeSceneState();
            }
            
            // For dev scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        public void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(GameManager.Instance.CurrentlyLoadedScene);
        }

        public void LoadMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void LoadDevScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    
        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        
        private void ReinitializeSceneState()
        {
            GameManager.Instance.ResetPlayer();
            GameManager.Instance.ResetKeys();
            GameManager.Instance.ResetRelic();
            GameManager.Instance.ResetEnemies();
        }
    }
}
