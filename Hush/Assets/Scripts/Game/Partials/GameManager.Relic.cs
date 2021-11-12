namespace Game
{
    public partial class GameManager
    {
        public void CollectRelic()
        {
            _state.playerHasRelic = true;
        }

        public void ResetRelic()
        {
            _state.playerHasRelic = false;
        }
    }
}