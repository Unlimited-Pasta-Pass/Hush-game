using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

public interface ISpell
{
    void PerformLightSpell(InputAction.CallbackContext context);
    void PerformHeavySpell(InputAction.CallbackContext context);
}
