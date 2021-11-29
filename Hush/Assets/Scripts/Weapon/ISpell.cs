using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

public interface ISpell
{
    public SpellType SpellType { get; }
    public float LightCooldown { get; }
    public float HeavyCooldown { get; }
    void PerformLightSpell(InputAction.CallbackContext context);
    void PerformHeavySpell(InputAction.CallbackContext context);
}
