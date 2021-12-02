using UnityEngine;
using Weapon.Enums;

namespace Game
{
    public partial class GameManager
    {
        [SerializeField] private float fireHeavyCooldown = 10f;
        [SerializeField] private float fireLightCooldown = 5f;
        [SerializeField] private float stunHeavyCooldown =  10f;
        [SerializeField] private float stunLightCooldown = 5f;
        [SerializeField] private float invisibleHeavyCooldown = 10f;
        [SerializeField] private float invisibleLightCooldown =  5f;
        
        public float FireHeavyCooldown => fireHeavyCooldown;
        public float FireLightCooldown => fireLightCooldown;
        public float StunHeavyCooldown => stunHeavyCooldown ;
        public float StunLightCooldown => stunLightCooldown;
        public float InvisibleHeavyCooldown => invisibleHeavyCooldown ;
        public float InvisibleLightCooldown => invisibleLightCooldown;

        public bool CanCastHeavy => Time.time - GetHeavySpellActivationTime() > GetHeavySpellCoolDownTime();
        public bool CanCastLight => Time.time - GetLightSpellActivationTime() > GetLightSpellCoolDownTime();
        
        public float GetHeavySpellActivationTime()
        {
            return _state.heavySpellActivationTime;
        }

        public void SetHeavySpellActivationTime(float delta)
        {
            _state.heavySpellActivationTime = delta;
        }

        public float GetHeavySpellCoolDownTime()
        {
            return _state.heavySpellCooldownTime;
        }

        public void SetHeavySpellCooldownTime(float delta)
        {
            _state.heavySpellCooldownTime = delta;
        }

        public void SetActiveHeavySpell(SpellType type)
        {
            _state.activeHeavySpell = type;
        }

        public SpellType GetActiveHeavySpell()
        {
            return _state.activeHeavySpell;
        }
        
        public float GetLightSpellActivationTime()
        {
            return _state.lightSpellActivationTime;
        }

        public void SetLightSpellActivationTime(float delta)
        {
            _state.lightSpellActivationTime = delta;
        }

        public float GetLightSpellCoolDownTime()
        {
            return _state.lightSpellCooldownTime;
        }

        public void SetLightSpellCooldownTime(float delta)
        {
            _state.lightSpellCooldownTime = delta;
        }

        public void SetActiveLightSpell(SpellType type)
        {
            _state.activeLightSpell = type;
        }

        public SpellType GetActiveLightSpell()
        {
            return _state.activeLightSpell;
        }

        public void ApplySpellState()
        {
            float delta = Time.time - _state.saveTime;
            _state.heavySpellActivationTime += delta;
            _state.lightSpellActivationTime += delta;
        }
    }
}
