using System.Collections;
using System.Collections.Generic;
using Common.Enums;
using Game;
using Player;
using Player.Enums;
using Plugins;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using Weapon.Enums;

public class StunSpell : MonoBehaviour, ISpell
{
        [Header("Parameters")]
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
        private static SpellManager SpellManager => SpellManager.Instance;

        private PlayerMovement _player;
        
        private void Awake()
        {
            GameManager.Instance.SetActiveHeavySpell(WeaponType.InvisibleSpell);
            GameManager.Instance.SetActiveLightSpell(WeaponType.InvisibleSpell);
            GameManager.Instance.SetHeavySpellCooldownTime(4f);
            GameManager.Instance.SetLightSpellCooldownTime(4f);
            GameManager.Instance.SetHeavySpellActivationTime(0);
            GameManager.Instance.SetLightSpellActivationTime(0);
        }

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                // only bind which one is useful move to public method
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
            if (!GameManager.Instance.PlayerIsAlive ||  GameManager.Instance.GetActiveLightSpell()!= WeaponType.StunSpell || !SpellManager.canCastLight)
                return;

            _player.OnAttackPerformed(baseDuration);
            Invoke(nameof(LightAttack), castDelay);
            
           GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != WeaponType.StunSpell || !SpellManager.canCastHeavy)
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
            CreateSpellAttack(false);
        }

        private void HeavyAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(true);
        }

        private void CreateSpellAttack(bool isHeavy) 
        {
            var prefab = isHeavy ? heavySpellPrefab : lightSpellPrefab;
            var spellClone = Instantiate(prefab);
            
            spellClone.transform.position = shootPosition.transform.position;
            spellClone.transform.rotation = shootPosition.transform.rotation;
            
            spellClone.GetComponent<CustomFireProjectile>().ShootPosition = shootPosition.transform;
            spellClone.GetComponent<CustomFireProjectile>().Damage = (int) 10;
        }
        
        // public override void HandleCollision(GameObject obj, Collision c)
        // {
        //     base.HandleCollision(obj, c);
        //     
        //     if(c.gameObject.TryGetComponent<Enemy>(out var enemy))
        //     {
        //         enemy.Stun();
        //     }
        // }
}
