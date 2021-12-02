namespace Game
{
    public partial class GameManager
    {
        public void OnPlayerDeath()
        {
            CurrentGameState.OnPlayerDeath();
            SaveGameManager.Instance.OnSave();
        }
    }
}