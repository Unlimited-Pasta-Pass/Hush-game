using System;
using Game.Models;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [Serializable]
    public partial class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        private GameState _state;
        
        public GameState CurrentGameState => _state;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        
            DontDestroyOnLoad(Instance.gameObject);

            Instance.InitializeValues();
        }

        private void InitializeValues()
        {
            _state ??= new GameState();
        }

        // Game Model
        public void SetGameState(GameState gameState)
        {
            _state = gameState;

            ApplyPlayerState();
            ApplyEnemiesState();
            ApplyKeysState();
            ApplyRelicState();
        }
    }
}