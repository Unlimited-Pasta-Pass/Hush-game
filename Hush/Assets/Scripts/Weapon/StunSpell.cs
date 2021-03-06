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
        [SerializeField] private Vector3 spellOffset;
        [SerializeField] private float heavySpellDuration = 5f;
        [SerializeField] private float lightSpellDuration = 3f;
        [SerializeField] private float lightStunEffectDuration = 1f;
        [SerializeField] private float heavyStunEffectDuration = 3f;
        
        [SerializeField] private float heavyCooldown = 10f;
        [SerializeField] private float lightCooldown = 6f;
        
        [Header("Spell References")]
        [SerializeField] protected GameObject heavySpellPrefab; 
        [SerializeField] protected GameObject lightSpellPrefab;
        [SerializeField] private AudioSource stunAudio;

        [Header("Other References")]
        [SerializeField] private Animator animator;
        private static InputManager Input => InputManager.Instance;

        private PlayerMovement _player;
        
        public SpellType SpellType => SpellType.StunSpell;
        public float HeavyCooldown => heavyCooldown;
        public float LightCooldown => lightCooldown;

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
            if (!GameManager.Instance.PlayerIsAlive ||  GameManager.Instance.GetActiveLightSpell()!= SpellType || !GameManager.Instance.CanCastLight)
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

        private void LightAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellAttack);
            CreateSpellAttack(lightSpellDuration, false);
        }

        private void HeavyAttack()
        {
            animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
            CreateSpellAttack(heavySpellDuration, true);
        }

        private void CreateSpellAttack(float stunLength, bool isHeavy) 
        {
            var prefab = isHeavy ? heavySpellPrefab : lightSpellPrefab;
            var spellClone = Instantiate(prefab);

            Vector3 pos = transform.position + spellOffset;
            spellClone.transform.position = pos;
            
            spellClone.GetComponent<StunCollision>().StunEffect(pos, stunLength, isHeavy);
            spellClone.GetComponent<ParticleSystem>().Play();
            stunAudio.Play();
           
            Destroy(spellClone, isHeavy ? heavyStunEffectDuration : lightStunEffectDuration);
        }
}
