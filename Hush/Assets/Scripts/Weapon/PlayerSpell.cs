using Common.Enums;
using Game;
using Player;
using Player.Enums;
using Plugins;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using Weapon.Enums;

namespace Weapon
{
    public class PlayerSpell : MonoBehaviour, IWeapon
    {
        [Header("Parameters")]
        [SerializeField] protected float baseDamage = 5f;
        [SerializeField] protected float heavyDamage = 15f;
        [SerializeField] protected float castCooldown = 7f;
        protected bool canCast = true;
        private float lastCastTime;
        

        [Header("Spell References")]
        [SerializeField] protected GameObject spellPrefab; 
        [SerializeField] protected GameObject shootPosition;
    
        [Header("Other References")]
        [SerializeField] private Animator animator;
    
        private static InputManager Input => InputManager.Instance;

        public WeaponType WeaponType => WeaponType.FireballSpell;

        public float BaseDamage => baseDamage;
        public float HeavyDamage => heavyDamage;
    
        private void OnEnable()
        {
            Input.reference.actions[Actions.LightSpell].performed += PerformAttack;
            Input.reference.actions[Actions.HeavySpell].performed += PerformHeavyAttack;
        }
    
        private void OnDisable()
        {
            Input.reference.actions[Actions.LightSpell].performed -= PerformAttack;
            Input.reference.actions[Actions.HeavySpell].performed -= PerformHeavyAttack;
        }

        protected void Update()
        {
            if (Time.time - lastCastTime >= castCooldown)
            {
                canCast = true;
            }
        }
    
        public void PerformAttack(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || !canCast)
                return;

            canCast = false;
            
            animator.SetTrigger(PlayerAnimator.SpellAttack);
            CreateSpellAttack(AttemptCrit(BaseDamage));
            
            lastCastTime = Time.time;
            GameManager.Instance.SetSpellCooldownTime(lastCastTime);
        }

        public void PerformHeavyAttack(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || !canCast)
                return;

            canCast = false;
            
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(AttemptCrit(HeavyDamage));
            
            lastCastTime = Time.time;
            GameManager.Instance.SetSpellCooldownTime(lastCastTime);
        }

        public float AttemptCrit(float damage)
        {
            return damage;
        }

        protected virtual void CreateSpellAttack(float damage)
        {
        }
    }
}