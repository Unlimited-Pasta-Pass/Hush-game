using Common.Models;
using Player;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        public bool PlayerIsAlive => PlayerCurrentHitPoints > 0f;

        public float PlayerCurrentHitPoints
        {
            get
            {
                // if less than 0, that player HP not initialized, initialize it
                if (_state.playerCurrentHitPoints < 0)
                    _state.playerCurrentHitPoints = _state.BaseVitalityPermanent;
                
                return _state.playerCurrentHitPoints;
            }
        }

        public float PlayerMaxHitPoints
        {
            get
            {
                // if less than 0, that player HP not initialized, initialize it
                if (_state.playerMaxHitPoints < 0)
                    _state.playerMaxHitPoints = _state.BaseVitalityPermanent;
                
                return _state.playerMaxHitPoints;
            }
        }

        public SerializableTransform PlayerTransform => _state.playerTransform;

        public void SetPlayerMaxHitPoints(float hp, float vitalityBoost)
        {
            _state.playerMaxHitPoints = hp + vitalityBoost;
        }

        public void SetPlayerHitPoints(float hp, float vitalityBoost)
        {
            _state.playerCurrentHitPoints = hp + vitalityBoost;
        }

        public void AddToPlayerSpeedBoost(float speedBoost)
        {
            _state.speedBoost += speedBoost;
        }

        public void AddToPlayerDamageBoost(float damageBoost)
        {
            _state.damageBoost += damageBoost;
        }

        public float GetPlayerSpeedBoost()
        {
            return _state.speedBoost;
        }
        
        public float GetPlayerVitalityBoost()
        {
            return _state.vitalityBoost;
        }
        
        public float GetPlayerDamageBoost()
        {
            return _state.damageBoost;
        }

        public bool UpdatePlayerHitPoints(float hpDelta)
        {
            _state.playerCurrentHitPoints += hpDelta;

            if (_state.playerCurrentHitPoints > 0)
                return true;

            _state.playerCurrentHitPoints = 0;
            return false;
        }

        public void MovePlayer(Transform playerTransform)
        {
            _state.playerTransform = playerTransform;
        }

        public void ResetPlayer()
        {
            _state.playerTransform = new SerializableTransform(Vector3.zero, Quaternion.identity);
        }

        public void SetIsPlayerInvisible(bool isInvisble)
        {
            _state.isPlayerInvisible = isInvisble;
        }
        
        public bool GetIsPlayerInvisible()
        {
            return _state.isPlayerInvisible;
        }

        private void ApplyPlayerState()
        {
            var playerMovement = FindObjectOfType<PlayerMovement>();

            if (playerMovement == null) 
                return;

            playerMovement.InitializePlayerTransform();
        }
    }
}