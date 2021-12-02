namespace Game
{
    public partial class GameManager
    {
        public void OnPlayerDeath()
        {
            ResetTemporaryBuffs();
            ResetPlayer();
            RestorePlayerHealth();
            ResetSceneProgress();
            ResetRelic();
            ResetEnemies();
            ResetKeys();
            SaveGameManager.Instance.OnSave();
        }
    }
}