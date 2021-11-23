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
            ReinitializeSceneState();
            
            // real code for incrementing which level we're at
            // var updatedLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
            // PlayerPrefs.SetInt("CurrentLevel", updatedLevel);
        
            // For dev scene
            LoadDevScene();
        }

        public void LoadDevScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void LoadDemoScene()
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
