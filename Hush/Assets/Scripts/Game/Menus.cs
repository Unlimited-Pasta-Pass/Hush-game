using Game;
using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject loadGameButton;

    private void Start()
    {
        loadGameButton.SetActive(SaveGameManager.Instance.HasSavedGame);
    }

    public void NewGame()
    {
        SceneManager.Instance.LoadFirstFloor();
    }
    
    public void LoadGame()
    {
        SaveGameManager.Instance.OnLoad();
    }
    
    public void Replay()
    {
        SceneManager.Instance.ReloadScene();
    }

    public void Credits() {
        // POP-UP FOR CREDITS
        Debug.Log("CREDITS");
    }
    
    public void MainMenu() {
        SceneManager.Instance.LoadMainMenu();
    }

    public void Quit()
    {
        SceneManager.Instance.QuitGame();
    }
}
