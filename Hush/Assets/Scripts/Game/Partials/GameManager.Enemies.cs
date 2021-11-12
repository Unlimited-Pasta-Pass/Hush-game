using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        public bool IsPlayerInCombat => _state.enemiesAttacking.Count > 0;
        
        public void EnemyStartedAttacking(int id)
        {
            _state.enemiesAttacking.Add(id);
        }

        public void EnemyStoppedAttacking(int id)
        {
            _state.enemiesAttacking.Remove(id);
        }

        public void MoveEnemy(int instanceId, Transform enemyTransform)
        {
            _state.enemiesTransforms[instanceId] = enemyTransform;
        }
    }
}