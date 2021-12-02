using System;
using System.Collections;
using Common.Enums;
using DigitalRuby.PyroParticles;
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
        [SerializeField] private float projectileDelay = 0.2f;
        [SerializeField] private float castDelay = 0.1f;

        [SerializeField] private float heavyCooldown = 10f;
        [SerializeField] private float lightCooldown = 6f;
    
        [Header("Spell References")]
        [SerializeField] protected GameObject heavySpellPrefab; 
        [SerializeField] protected GameObject lightSpellPrefab; 
        [SerializeField] protected GameObject shootPosition;
    
        [Header("Other References")]
        [SerializeField] private Animator animator;
        private static InputManager Input => InputManager.Instance;

        public SpellType SpellType => SpellType.FireballSpell;

        public float HeavyCooldown => heavyCooldown;
        public float LightCooldown => lightCooldown;
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
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveLightSpell() != SpellType || !GameManager.Instance.CanCastLight)
                return;
            
            _player.OnAttackPerformed(baseDuration);
            Invoke(nameof(LightAttack), castDelay);
            
            GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != SpellType || !GameManager.Instance.CanCastHeavy)
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
            StartCoroutine(CreateSpellAttack(AttemptCrit(BaseDamage), false));
        }

        private void HeavyAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            StartCoroutine(CreateSpellAttack(AttemptCrit(HeavyDamage), true));
        }

        private IEnumerator CreateSpellAttack(float damage, bool isHeavy)
        {
            yield return new WaitForSeconds(projectileDelay);
            GameObject spellPrefab = isHeavy ? heavySpellPrefab : lightSpellPrefab;
            
            var spellClone = Instantiate(spellPrefab);
            spellClone.transform.position = shootPosition.transform.position;
            spellClone.transform.rotation = shootPosition.transform.rotation;
            
            spellClone.GetComponent<CustomFireProjectile>().ShootPosition = shootPosition.transform;
            spellClone.GetComponent<CustomFireProjectile>().Damage = (int)damage;
        }
    }
}
