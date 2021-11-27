using Common.Enums;
using Game;
using Player;
using Player.Enums;
using Plugins;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

public class StunSpell : MonoBehaviour, ISpell
{
        [Header("Parameters")]
        [SerializeField] private float baseDuration = 0.25f;
        [SerializeField] private float heavyDuration = 0.4f;
        [SerializeField] private float castDelay = 0.1f;
        [SerializeField] private float spellHeightOffset = 1f;
        [SerializeField] private float heavySpellDuration = 5f;
        [SerializeField] private float lightSpellDuration = 3f;
        [SerializeField] private float stunEffectDuration = 2f;
        

        [Header("Spell References")]
        [SerializeField] protected GameObject heavySpellPrefab; 
        [SerializeField] protected GameObject lightSpellPrefab;

        [Header("Other References")]
        [SerializeField] private Animator animator;
        private static InputManager Input => InputManager.Instance;
        private static SpellManager SpellManager => SpellManager.Instance;

        private PlayerMovement _player;
        
        private void Awake()
        {
             GameManager.Instance.SetActiveHeavySpell(SpellType.StunSpell);
             GameManager.Instance.SetActiveLightSpell(SpellType.StunSpell);
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
            if (!GameManager.Instance.PlayerIsAlive ||  GameManager.Instance.GetActiveLightSpell()!= SpellType.StunSpell || !SpellManager.CanCastLight)
                return;

            _player.OnAttackPerformed(baseDuration);
            Invoke(nameof(LightAttack), castDelay);
            
           GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != SpellType.StunSpell || !SpellManager.CanCastHeavy)
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
            spellClone.GetComponent<StunCollision>().duration = stunLength;
            
            Vector3 pos = transform.position;
            pos.y += spellHeightOffset;
            spellClone.transform.position = pos;

            spellClone.GetComponent<ParticleSystem>().Play();
           
            Destroy(spellClone, stunEffectDuration);
        }
}
