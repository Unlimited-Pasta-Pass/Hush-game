using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void LoadNextScene()
    {
        // real code for incrementing which level we're at
        // var updatedLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        // PlayerPrefs.SetInt("CurrentLevel", updatedLevel);
        
        // For demo
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoRespawn");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
