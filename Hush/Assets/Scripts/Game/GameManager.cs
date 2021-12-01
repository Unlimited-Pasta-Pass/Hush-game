using System;
using Game.Models;
using UnityEngine;
using Weapon.Enums;

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
            
        #if UNITY_EDITOR
            SetActiveLightSpell(SpellType.FireballSpell);
            SetActiveHeavySpell(SpellType.FireballSpell);
        #endif
        }

        public void SetSaveTime(float time)
        {
            _state.saveTime = time;
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
            ApplyEcholocationState();
            ApplySpellState();
        }
    }
}
