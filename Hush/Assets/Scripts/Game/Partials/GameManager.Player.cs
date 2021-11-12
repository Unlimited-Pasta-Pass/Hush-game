namespace Game
{
    public partial class GameManager
    {
        public float PlayerCurrentHitPoints => _state.playerCurrentHitPoints;
        public float PlayerMaxHitPoints => _state.playerMaxHitPoints;
        public bool PlayerHasRelic => _state.playerHasRelic;
        
        public void SetPlayerMaxHitPoints(float hp)
        {
            _state.playerMaxHitPoints = hp;
        }

        public void SetPlayerHitPoints(float hp)
        {
            _state.playerCurrentHitPoints = hp;
        }

        public bool UpdatePlayerHitPoints(float hpDelta)
        {
            _state.playerCurrentHitPoints += hpDelta;

            if (_state.playerCurrentHitPoints > 0)
                return true;

            _state.playerCurrentHitPoints = 0;
            return false;
        }
    }
}