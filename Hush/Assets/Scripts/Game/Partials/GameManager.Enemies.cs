using System;
using System.Linq;
using Common.Models;
using Enemies;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        [Header("Enemies")]
        [SerializeField] private float enemyHitPoints = 100f;
        [SerializeField] private GameObject enemyPrefab;
        
        public bool IsPlayerInCombat => _state.enemiesAttacking.Count > 0;
        
        public void EnemyStartedAttacking(Guid id)
        {
            _state.enemiesAttacking.Add(id);
        }

        public void EnemyStoppedAttacking(Guid id)
        {
            _state.enemiesAttacking.Remove(id);
        }

        public void MoveEnemy(Guid id, Transform enemyTransform)
        {
            _state.enemiesTransforms[id] = enemyTransform;
        }

        public void UpdateEnemyHitPoints(Guid id, float hp)
        {
            if (hp <= 0)
            {
                _state.enemiesHitPoints.Remove(id);
                _state.enemiesTransforms.Remove(id);
                return;
            }
            
            _state.enemiesHitPoints[id] = hp;
        }

        public bool IsEnemyAttacking(Guid id)
        {
            return _state.enemiesAttacking.Contains(id);
        }

        public SerializableTransform GetEnemyTransform(Guid id)
        {
            return _state.enemiesTransforms.ContainsKey(id) ? _state.enemiesTransforms[id] : null;
        }

        public float GetEnemyHitPoints(Guid id)
        {
            return _state.enemiesHitPoints.ContainsKey(id) ? _state.enemiesHitPoints[id] : enemyHitPoints;
        }

        private void ApplyEnemiesState()
        {
            // Clear all current enemies
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                Destroy(enemy.gameObject);
            }
            
            // Add Saved State enemies
            foreach (var id in _state.enemiesTransforms.Keys.ToList())
            {
                var enemyGo = Instantiate(enemyPrefab);
                var guid = enemyGo.GetComponent<GuidComponent>().GetGuid();
                    
                UpdateEnemyID(guid, id);

                var enemy = enemyGo.GetComponent<Enemy>();
                
                if (enemy == null)
                    continue;
                
                enemy.InitializeEnemy();
            }
        }

        private void UpdateEnemyID(Guid newId, Guid oldId)
        {
            var enemyHitPoints = _state.enemiesHitPoints[oldId];
            _state.enemiesHitPoints.Remove(oldId);
            _state.enemiesHitPoints.Add(newId, enemyHitPoints);

            var enemyTransform = _state.enemiesTransforms[oldId];
            _state.enemiesTransforms.Remove(oldId);
            _state.enemiesTransforms.Add(newId, enemyTransform);

            if (_state.enemiesAttacking.Contains(oldId))
            {
                _state.enemiesAttacking.Remove(oldId);
                _state.enemiesAttacking.Add(newId);
            }
        }
    }
}