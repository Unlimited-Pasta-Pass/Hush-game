namespace Game
{
    public partial class GameManager
    {
        public bool PlayerHasRelic => _state.playerHasRelic;
        
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