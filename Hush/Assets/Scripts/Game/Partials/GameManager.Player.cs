using Common.Models;
using Player;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        [Header("Player")]
        [Tooltip("Amount of damage the player can receive before dying")] 
        [SerializeField] private float playerHitPoints = 100f;
        
        public float PlayerCurrentHitPoints
        {
            get
            {
                if (_state.playerCurrentHitPoints < 0)
                    _state.playerCurrentHitPoints = playerHitPoints;
                
                return _state.playerCurrentHitPoints;
            }
        }

        public float PlayerMaxHitPoints
        {
            get
            {
                if (_state.playerMaxHitPoints < 0)
                    _state.playerMaxHitPoints = playerHitPoints;
                
                return _state.playerMaxHitPoints;
            }
        }

        public SerializableTransform PlayerTransform => _state.playerTransform;
        
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

        public void MovePlayer(Transform playerTransform)
        {
            _state.playerTransform = playerTransform;
        }

        private void ApplyPlayerState()
        {
            var playerMovement = FindObjectOfType<PlayerMovement>();
            
            playerMovement.InitializePlayerTransform();
        }
    }
}