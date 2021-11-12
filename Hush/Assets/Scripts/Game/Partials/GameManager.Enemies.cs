namespace Game
{
    public partial class GameManager
    {
        public bool IsPlayerInCombat => _state.enemiesAttacking.Count > 0;
        
        public void AddToEnemyList(int id)
        {
            _state.enemiesAttacking.Add(id);
        }

        public void RemoveFromEnemyList(int id)
        {
            _state.enemiesAttacking.Remove(id);
        }
    }
}