using Common;
using UnityEngine;
using Enums;
using DigitalRuby.PyroParticles;
using Plugins;
using UnityEngine.InputSystem;
using Weapon.Enums;

public class Spell : MonoBehaviour, IWeapon
{
    [Header("Parameters")]
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float heavyDamage = 15f;
    
    [Header("Spell References")]
    [SerializeField] private GameObject spellPrefab; 
    [SerializeField] private GameObject shootPosition;
    
    [Header("Other References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInputManager input;

    public WeaponType WeaponType => WeaponType.Spell;

    public float BaseDamage => baseDamage;
    public float HeavyDamage => heavyDamage;
    
    private void OnEnable()
    {
        input.reference.actions[Actions.LightSpell].performed += PerformAttack;
        input.reference.actions[Actions.HeavySpell].performed += PerformHeavyAttack;
    }
    
    private void OnDisable()
    {
        input.reference.actions[Actions.LightSpell].performed -= PerformAttack;
        input.reference.actions[Actions.HeavySpell].performed -= PerformHeavyAttack;
    }
    
    public void PerformAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger(PlayerAnimator.SpellAttack);
        CreateSpellAttack(AttemptCrit(BaseDamage));
    }

    public void PerformHeavyAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
        CreateSpellAttack(AttemptCrit(HeavyDamage));
    }

    public float AttemptCrit(float damage)
    {
        return damage;
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