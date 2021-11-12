using Common;
using UnityEngine;
using Enums;
using UnityEngine.InputSystem;
using Weapon.Enums;

public class Sword : MonoBehaviour, IWeapon
{
    [Header("Damage Parameters")]
    [SerializeField] private float baseDamage = 5;
    [SerializeField] private float heavyDamage = 15;
    
    [Header("Crit Parameters")]
    [SerializeField] private float critChance = 0.10f;
    [SerializeField] private float critMultiplierLow = 0.5f;
    [SerializeField] private float critMultiplierHigh = 0.75f;
    
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerInputManager input;

    public WeaponType WeaponType => WeaponType.Sword;
    
    public float BaseDamage => baseDamage;
    public float HeavyDamage => heavyDamage;

    private void OnEnable()
    {
        input.reference.actions[Actions.LightAttack].performed += PerformAttack;
        input.reference.actions[Actions.HeavyAttack].performed += PerformHeavyAttack;
    }

    private void OnDisable()
    {
        input.reference.actions[Actions.LightAttack].performed -= PerformAttack;
        input.reference.actions[Actions.HeavyAttack].performed -= PerformHeavyAttack;
    }

    private void Reset()
    {
        playerAnimator = GetComponentInParent<Animator>();
    }

    public void PerformAttack(InputAction.CallbackContext context)
    {
        playerAnimator.SetTrigger(PlayerAnimator.LightAttack);
    }

    public void PerformHeavyAttack(InputAction.CallbackContext context)
    {
        playerAnimator.SetTrigger(PlayerAnimator.HeavyAttack);
    }
    
    public float AttemptCrit(float damage)
    {
        if (Random.value <= critChance)
        {
            return damage + (damage * Random.Range(critMultiplierLow, critMultiplierHigh));
        }

        return damage;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag(Tags.Enemy) && !col.CompareTag(Tags.Dome)) 
            return;
        
        var stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(PlayerAnimator.Layer.UpperBody);
        var killable = col.GetComponent<IKillable>();
        if (stateInfo.IsName(PlayerAnimator.State.LightAttack))
        {
            killable.TakeDamage(AttemptCrit(BaseDamage));
        }
        if (stateInfo.IsName(PlayerAnimator.State.HeavyAttack))
        {
            killable.TakeDamage(AttemptCrit(HeavyDamage));
        }
    }
}
