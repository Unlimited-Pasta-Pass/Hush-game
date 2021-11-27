using System;
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
    public class FireballSpell : MonoBehaviour, ISpell
    {
        [Header("Parameters")]
        [SerializeField] private float baseDamage = 5f;
        [SerializeField] private float heavyDamage = 15f;
        
        [SerializeField] private float baseDuration = 0.25f;
        [SerializeField] private float heavyDuration = 0.4f;
        [SerializeField] private float castDelay = 0.1f;
    
        [Header("Spell References")]
        [SerializeField] protected GameObject heavySpellPrefab; 
        [SerializeField] protected GameObject lightSpellPrefab; 
        [SerializeField] protected GameObject shootPosition;
    
        [Header("Other References")]
        [SerializeField] private Animator animator;
        private static InputManager Input => InputManager.Instance;
        private static SpellManager Spell => SpellManager.Instance;

        public float BaseDamage => baseDamage;
        public float HeavyDamage => heavyDamage;

        private PlayerMovement _player;
        
        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightSpell].performed += PerformLightSpell;
                Input.reference.actions[Actions.HeavySpell].performed += PerformHeavySpell;
            }

            _player = FindObjectOfType<PlayerMovement>();

            if (_player == null)
                throw new MissingComponentException("Missing PlayerMovement Component in the Scene");
        }

        // TODO Remove
        private void Awake()
        {
            GameManager.Instance.SetActiveHeavySpell(SpellType.FireballSpell);
            GameManager.Instance.SetActiveLightSpell(SpellType.FireballSpell);
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightSpell].performed -= PerformLightSpell;
                Input.reference.actions[Actions.HeavySpell].performed -= PerformHeavySpell;
            }
        }

        public void PerformLightSpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveLightSpell() != SpellType.FireballSpell || !Spell.CanCastLight)
                return;
            
            _player.OnAttackPerformed(baseDuration);
            Invoke(nameof(LightAttack), castDelay);
            
            GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != SpellType.FireballSpell || !Spell.CanCastHeavy)
                return;
            
            _player.OnAttackPerformed(heavyDuration);
            Invoke(nameof(HeavyAttack), castDelay);
            
            GameManager.Instance.SetHeavySpellActivationTime(Time.time);
        }

        public float AttemptCrit(float damage)
        {
            return damage;
        }

        private void LightAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellAttack);
            CreateSpellAttack(AttemptCrit(BaseDamage), false);
        }

        private void HeavyAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(AttemptCrit(HeavyDamage), true);
        }

        private void CreateSpellAttack(float damage, bool isHeavy)
        {
            GameObject spellPrefab = isHeavy ? heavySpellPrefab : lightSpellPrefab;
            
            var spellClone = Instantiate(spellPrefab);
            spellClone.transform.position = shootPosition.transform.position;
            spellClone.transform.rotation = shootPosition.transform.rotation;

            spellClone.GetComponent<CustomFireProjectile>().ShootPosition = shootPosition.transform;
            spellClone.GetComponent<CustomFireProjectile>().Damage = (int)damage;
        }
    }
}