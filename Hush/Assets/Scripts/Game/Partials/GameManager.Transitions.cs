namespace Game
{
    public partial class GameManager
    {
        /// <summary>
        /// Call this to clean up at the end of a run
        /// </summary>
        public void OnRunCompleted()
        {
            Instance.ResetTemporaryBuffs();
            Instance.RestorePlayerHealth();
            Instance.ResetSceneProgress();
            SaveGameManager.Instance.OnSave();
        }

        private void LoadGameOver()
        {
            SceneManager.Instance.LoadGameOverScene();
        }
        
        
    }
}