using Game.Enums;
using UnityEngine;
using Weapon.Enums;

namespace Game
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [Header("Scene Build Indexes")]
        [SerializeField] private int[] levelSceneIndexes;
        [SerializeField] private int mainMenuIndex;
        [SerializeField] private int finalSceneIndex;
        [SerializeField] private int endgameSceneIndex;
        
        [Header("New Game Parameters")]
        [SerializeField] private SpellType initialLightSpell = SpellType.FireballSpell;
        [SerializeField] private SpellType initialHeavySpell = SpellType.StunSpell;

        // private Dictionary<int, int> _randomScenes;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
        }

        public void LoadNextScene()
        {
            // ignore scene state reinitialization if main menu 
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != Scenes.MainMenu)
            {
                ReinitializeSceneState();
            }

            // Done with regular levels & Boss => Endgame
            if (GameManager.Instance.SceneProgression >= levelSceneIndexes.Length)
            {
                LoadWinScreen();
            }
            // Done with regular levels = Final Level
            else if (GameManager.Instance.SceneProgression >= levelSceneIndexes.Length - 1)
            {
                LoadFinal();
            }
            // Regular levels
            else
            {
                LoadLevel();
            }
        }

        public void LoadScene(int sceneIndex)
        {
            // ignore scene state reinitialization if main menu 
            if (sceneIndex != Scenes.MainMenu)
            {
                ReinitializeSceneState();
            }
            TransitionToScene(sceneIndex);
        }

        public void ReloadScene()
        {
            TransitionToScene(GameManager.Instance.CurrentlyLoadedScene);
        }

        public void LoadMainMenu()
        {
            TransitionToScene(Scenes.MainMenu);
        }

        private void LoadLevel()
        {
            if (GameManager.Instance.SceneProgression < 0)
            {
                StartNewGame();
            }
            
            // Leave before scene transition to save progress
            GameManager.Instance.IncreaseSceneProgress();
            TransitionToScene(levelSceneIndexes[GameManager.Instance.SceneProgression]);
            // TransitionToScene(RandomScenes[GameManager.Instance.SceneProgress]);
        }

        private void LoadFinal()
        {
            // Leave before scene transition to save progress
            GameManager.Instance.IncreaseSceneProgress();
            TransitionToScene(Scenes.FinalFloor);
        }

        private void LoadWinScreen()
        {
            // Leave before scene transition to save progress
            GameManager.Instance.ResetSceneProgress();
            GameManager.Instance.RestorePlayerHealth();
            GameManager.Instance.ResetTemporaryBuffs();
            TransitionToScene(Scenes.Win);
        }
        
        public void LoadGameOverScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GameOver);
            GameManager.Instance.OnRunCompleted();
        }
        
        public void LoadFirstFloor()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.FloorA);
        }

        public void LoadTempPowerScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.PowerUp);
        }
    
        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void StartNewGame()
        {
            // TODO Generate seed & randomize scene order based on seed
            
            // TEMPORARY VALUE REMOVE
            LoadFirstFloor();
            GameManager.Instance.SetLoadedScene(Scenes.FloorA);
            
            // Hack to set initial spells
            GameManager.Instance.SetActiveLightSpell(initialLightSpell);
            GameManager.Instance.SetActiveHeavySpell(initialHeavySpell);
        }

        private void TransitionToScene(int index)
        {
            // TODO Transition between scenes
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
            
            GameManager.Instance.SetLoadedScene(index);
            
            // Don't save moving to the menu
            if (index != Scenes.MainMenu)
            {
                SaveGameManager.Instance.OnSave();
            }
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
