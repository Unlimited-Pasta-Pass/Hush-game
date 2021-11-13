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
            keyCountChanged ??= new UnityEvent<int>();
        
            // Make sure the UI follows the Game Master State
            keyCountChanged.Invoke(_state.keysInPossession.Count);
        }

        // Game Model
        public void SetGameState(GameState gameState)
        {
            _state = gameState;

            ApplyEnemiesState();
        }
    }
}