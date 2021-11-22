using UnityEngine.InputSystem;
using Weapon.Enums;

public interface IWeapon
{
    public WeaponType WeaponType { get; }
    public float BaseDamage { get; }
    public float HeavyDamage { get; }

    void PerformAttack(InputAction.CallbackContext context);
    void PerformHeavyAttack(InputAction.CallbackContext context);
    float AttemptCrit(float damage);
}