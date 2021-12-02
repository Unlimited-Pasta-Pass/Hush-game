using Game.Enums;
using UnityEngine;
using Weapon.Enums;

namespace Game
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [Header("Scene Build Indexes")] [SerializeField]
        private Scenes[] levelSceneIndexes;

        [Header("New Game Parameters")] [SerializeField]
        private SpellType initialLightSpell = SpellType.FireballSpell;

        [SerializeField] private Scenes firstFloor = Scenes.FloorA;

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
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != (int) Scenes.MainMenu)
            {
                ReinitializeSceneState();
            }

            // Done with regular levels & Boss => Endgame
            // Num rooms + boss
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

        public void LoadScene(Scenes sceneIndex)
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
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) Scenes.GameOver);
            GameManager.Instance.OnRunCompleted();
        }

        public void LoadFirstFloor()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) firstFloor);
        }

        public void LoadTempPowerScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) Scenes.PowerUp);
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
            GameManager.Instance.SetLoadedScene( Instance.firstFloor);
            
            // Hack to set initial spells
            GameManager.Instance.SetActiveLightSpell(initialLightSpell);
            GameManager.Instance.SetActiveHeavySpell(initialHeavySpell);
        }

        private void TransitionToScene(Scenes sceneIndex)
        {
            // TODO Transition between scenes
            UnityEngine.SceneManagement.SceneManager.LoadScene( (int) sceneIndex);

            GameManager.Instance.SetLoadedScene(sceneIndex);

            // Don't save moving to the menu
            if (sceneIndex != Scenes.MainMenu)
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