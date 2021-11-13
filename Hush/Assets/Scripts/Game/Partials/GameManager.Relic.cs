using System;
using Relics;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        [Header("Relic")]
        [SerializeField] private float domeHitPoints = 200f;
        
        public bool PlayerHasRelic => _state.playerHasRelic;

        public bool DomeWasHit => Math.Abs(RelicDomeHitPoints - domeHitPoints) > float.Epsilon;
        
        public float RelicDomeHitPoints
        {
            get
            {
                if (_state.relicDomeHitPoints < 0f)
                    _state.relicDomeHitPoints = domeHitPoints;
                
                return _state.relicDomeHitPoints;
            }
        }

        public void CollectRelic()
        {
            _state.playerHasRelic = true;
        }

        public void ResetRelic()
        {
            _state.playerHasRelic = false;
        }

        public bool AttackDome(float damage)
        {
            _state.relicDomeHitPoints = Math.Max(_state.relicDomeHitPoints - damage, 0f);
            return _state.relicDomeHitPoints > 0f;
        }

        private void ApplyRelicState()
        {
            // Should only ever be 1 relic but iterate to be safe
            foreach (var relic in FindObjectsOfType<Relic>(true))
            {
                relic.SetRelicVisibility(!_state.playerHasRelic);
            }
            
            // Should only ever be 1 relic dome but iterate to be safe
            foreach (var dome in FindObjectsOfType<RelicDome>(true))
            {
                dome.SetDomeVisibility(!_state.playerHasRelic && RelicDomeHitPoints > 0f);
            }
        }
    }
}