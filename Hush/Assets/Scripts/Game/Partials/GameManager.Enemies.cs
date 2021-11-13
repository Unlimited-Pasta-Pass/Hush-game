using System;
using System.Collections;
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
        [SerializeField] private float enemyCleanupDelay = 3f;
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

        public bool AttackEnemy(Guid id, float damage)
        {
            if (!_state.enemiesHitPoints.ContainsKey(id))
            {
                _state.enemiesHitPoints.Add(id, enemyHitPoints);
            }

            _state.enemiesHitPoints[id] -= damage;

            if (_state.enemiesHitPoints[id] > 0) 
                return true;
            
            // Remove the enemy from the dictionaries when they die
            StartCoroutine(CleanEnemy(id, enemyCleanupDelay));
            return false;
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
            if (!_state.enemiesHitPoints.ContainsKey(id))
                _state.enemiesHitPoints.Add(id, enemyHitPoints);
            
            return _state.enemiesHitPoints[id];
        }

        public void ResetEnemies()
        {
            _state.enemiesHitPoints.Clear();
            _state.enemiesAttacking.Clear();
            _state.enemiesTransforms.Clear();
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
            var hitPoints = _state.enemiesHitPoints[oldId];
            _state.enemiesHitPoints.Remove(oldId);
            _state.enemiesHitPoints.Add(newId, hitPoints);

            var enemyTransform = _state.enemiesTransforms[oldId];
            _state.enemiesTransforms.Remove(oldId);
            _state.enemiesTransforms.Add(newId, enemyTransform);

            if (_state.enemiesAttacking.Contains(oldId))
            {
                _state.enemiesAttacking.Remove(oldId);
                _state.enemiesAttacking.Add(newId);
            }
        }

        private IEnumerator CleanEnemy(Guid id, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _state.enemiesHitPoints.Remove(id);
            _state.enemiesTransforms.Remove(id);
        }
    }
}