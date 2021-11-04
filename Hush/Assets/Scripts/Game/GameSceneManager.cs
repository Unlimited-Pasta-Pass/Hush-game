using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        // real code for incrementing which level we're at
        // var updatedLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        // PlayerPrefs.SetInt("CurrentLevel", updatedLevel);
        
        // For demo
        SceneManager.LoadScene("DemoRespawn");
    }
}
