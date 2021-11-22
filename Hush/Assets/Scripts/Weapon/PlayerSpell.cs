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
        private float castDelta;
        

        [Header("Spell References")] 
        [SerializeField] private string activeSpell = SpellList.Fireball; // default active
        [SerializeField] protected GameObject spellPrefab; 
        [SerializeField] protected GameObject shootPosition;
    
        [Header("Other References")]
        [SerializeField] private Animator animator;
    
        private static InputManager Input => InputManager.Instance;

        public WeaponType WeaponType => WeaponType.Spell;

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
            castDelta += Time.deltaTime;
            
            if (castDelta >= castCooldown)
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
            castDelta = 0f;
        }

        public void PerformHeavyAttack(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || !canCast)
                return;

            canCast = false;
            
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(AttemptCrit(HeavyDamage));
            castDelta = 0f;
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