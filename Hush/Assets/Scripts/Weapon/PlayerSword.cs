using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Game;
using Player;
using Player.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;
using Random = UnityEngine.Random;

namespace Weapon
{
    public class PlayerSword : MonoBehaviour, IWeapon
    {
        [Header("Sound Parameters")]
        [SerializeField] private AudioSource lightAttack;
        [SerializeField] private AudioSource heavyAttack;
        [SerializeField] private AudioSource heavyHit;
        [SerializeField] private AudioSource lightHit;
        
        // [Header("Damage Parameters")]
        // [SerializeField] 
        private float baseDamage;
        // [SerializeField]
        private float heavyDamage;
        
        [Header("Animation Parameters")]
        [SerializeField] private float baseDuration = 0.3f;
        [SerializeField] private float heavyDuration = 0.5f;
        [SerializeField] private float castDelay = 0.15f;
    
        [Header("Crit Parameters")]
        [SerializeField] private float critChance = 0.10f;
        [SerializeField] private float critMultiplierLow = 0.5f;
        [SerializeField] private float critMultiplierHigh = 0.75f;
    
        [Header("References")]
        [SerializeField] private Animator playerAnimator;
    
        private static InputManager Input => InputManager.Instance;

        public WeaponType WeaponType => WeaponType.Sword;
    
        public float BaseDamage => baseDamage + GameManager.Instance.GetPlayerDamageBoost();
        public float HeavyDamage => heavyDamage + GameManager.Instance.GetPlayerDamageBoost();

        private PlayerMovement _player;

        private readonly HashSet<GameObject> _attackedObjects = new HashSet<GameObject>();

        private void Start()
        {
            baseDamage = GameManager.Instance.PermanentDamage;
            heavyDamage = baseDamage + 10;
        }

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightAttack].performed += PerformAttack;
                Input.reference.actions[Actions.HeavyAttack].performed += PerformHeavyAttack;
            }

            _player = FindObjectOfType<PlayerMovement>();

            if (_player == null)
                throw new MissingComponentException("Missing PlayerMovement Component in the Scene");
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightAttack].performed -= PerformAttack;
                Input.reference.actions[Actions.HeavyAttack].performed -= PerformHeavyAttack;
            }
        }

        private void Reset()
        {
            playerAnimator = GetComponentInParent<Animator>();
        }

        public void PerformAttack(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive)
                return;
            
            _player.OnAttackPerformed(baseDuration);
            Invoke(nameof(LightAttack), castDelay);
        }

        public void PerformHeavyAttack(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive)
                return;
            
            _player.OnAttackPerformed(heavyDuration);
            Invoke(nameof(HeavyAttack), castDelay);
        }
    
        public float AttemptCrit(float damage)
        {
            if (Random.value <= critChance)
            {
                return damage + (damage * Random.Range(critMultiplierLow, critMultiplierHigh));
            }

            return damage;
        }

        private void LightAttack()
        {
            _attackedObjects.Clear();
            lightAttack.Play();
            playerAnimator.SetTrigger(PlayerAnimator.LightAttack);
        }

        private void HeavyAttack()
        {
            _attackedObjects.Clear();
            heavyAttack.Play();
            playerAnimator.SetTrigger(PlayerAnimator.HeavyAttack);
        }

        private void OnTriggerStay(Collider col)
        {
            if (!col.CompareTag(Tags.Enemy) && !col.CompareTag(Tags.Dome)) 
                return;

            if (_attackedObjects.Contains(col.gameObject))
                return;

            var stateInfo = playerAnimator.GetCurrentAnimatorStateInfo((int)PlayerAnimator.Layer.UpperBody);
            var killable = col.GetComponent<IKillable>();
            if (stateInfo.IsName(PlayerAnimator.State.LightAttack))
            {
                lightHit.Play();
                killable.TakeDamage(AttemptCrit(BaseDamage));
                _attackedObjects.Add(col.gameObject);
            }
            if (stateInfo.IsName(PlayerAnimator.State.HeavyAttack))
            {
                heavyHit.Play();
                killable.TakeDamage(AttemptCrit(HeavyDamage));
                _attackedObjects.Add(col.gameObject);
            }
        }
    }
}
