using UnityEngine;
using Weapon.Enums;

namespace Game
{
    public partial class GameManager
    {
        public float GetSpellActivationTime()
        {
            return _state.spellActivationTime;
        }

        public void SetSpellActivationTime(float delta)
        {
            _state.spellActivationTime = delta;
        }

        public float GetSpellCoolDownTime()
        {
            return _state.spellCooldownTime;
        }

        public void SetSpellCooldownTime(float delta)
        {
            _state.spellCooldownTime = delta;
        }

        public void SetActiveSpell(WeaponType type)
        {
            _state.activeSpell = type;
        }

        public WeaponType GetActiveSpell()
        {
            return _state.activeSpell;
        }
    }
}