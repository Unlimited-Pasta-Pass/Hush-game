using Common.Enums;
using Game;
using Player;
using Player.Enums;
using Plugins;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

namespace Weapon
{
    public class PlayerSpell : MonoBehaviour, IWeapon
    {
        [Header("Parameters")]
        [SerializeField] private float baseDamage = 5f;
        [SerializeField] private float heavyDamage = 15f;
        [SerializeField] private float baseDuration = 0.25f;
        [SerializeField] private float heavyDuration = 0.4f;
        [SerializeField] private float castDelay = 0.1f;
    
        [Header("Spell References")]
        [SerializeField] private GameObject spellPrefab; 
        [SerializeField] private GameObject shootPosition;
    
        [Header("Other References")]
        [SerializeField] private Animator animator;

        private static InputManager Input => InputManager.Instance;

        public WeaponType WeaponType => WeaponType.Spell;

        public float BaseDamage => baseDamage;
        public float HeavyDamage => heavyDamage;

        private PlayerMovement _player;

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightSpell].performed += PerformAttack;
                Input.reference.actions[Actions.HeavySpell].performed += PerformHeavyAttack;
            }

            _player = FindObjectOfType<PlayerMovement>();

            if (_player == null)
                throw new MissingComponentException("Missing PlayerMovement Component in the Scene");
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightSpell].performed -= PerformAttack;
                Input.reference.actions[Actions.HeavySpell].performed -= PerformHeavyAttack;
            }
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
            return damage;
        }

        private void LightAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellAttack);
            CreateSpellAttack(AttemptCrit(BaseDamage));
        }

        private void HeavyAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(AttemptCrit(HeavyDamage));
        }

        private void CreateSpellAttack(float damage)
        {
            var spellClone = Instantiate(spellPrefab);
            spellClone.transform.position = shootPosition.transform.position;
            spellClone.transform.rotation = shootPosition.transform.rotation;
            
            spellClone.GetComponent<CustomFireProjectile>().ShootPosition = shootPosition.transform;
            spellClone.GetComponent<CustomFireProjectile>().Damage = (int)damage;
        }
    }
}