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
            _state.relicDomeHitPoints = domeHitPoints;
        }

        public void DisableDome()
        {
            _state.relicDomeHitPoints = 0f;
        }

        public bool AttackDome(float damage)
        {
            _state.relicDomeHitPoints = Math.Max(_state.relicDomeHitPoints - damage, 0f);
            return _state.relicDomeHitPoints > 0f;
        }

        private void ApplyRelicState()
        {
            var relic = FindObjectOfType<Relic>(true);
            if (relic != null)
            {
                relic.SetRelicVisibility(!_state.playerHasRelic);
            }

            var dome = FindObjectOfType<RelicDome>(true);
            if (dome != null)
            {
                var domeVisible = !_state.playerHasRelic && RelicDomeHitPoints > 0f;
                dome.SetDomeVisibility(domeVisible);
                
                if (!domeVisible)
                    dome.Killed.Invoke();
            }
        }
    }
}