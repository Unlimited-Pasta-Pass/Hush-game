using Random = UnityEngine.Random;

namespace Game
{
    public partial class GameManager
    {
        public int CurrentlyLoadedScene => _state.currentlyLoadedScene;

        public int RandomSceneSeed => _state.randomSceneSeed;
        
        public int SceneProgression => _state.sceneProgress;

        public void SetLoadedScene(int currentlyLoadedScene)
        {
            _state.currentlyLoadedScene = currentlyLoadedScene;
        }

        public void GenerateRandomSceneSeed()
        {
            _state.randomSceneSeed = (int)(Random.value * int.MaxValue);
        }

        public void IncreaseSceneProgress()
        {
            _state.sceneProgress++;
        }

        public void ResetSceneProgress()
        {
            _state.sceneProgress = 0;
        }

        private void ApplySceneState()
        {
            SceneManager.Instance.LoadScene(SaveGameManager.Instance.SavedGameScene);
        }
    }
}