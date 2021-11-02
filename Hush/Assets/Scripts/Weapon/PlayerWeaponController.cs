using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;
using Common;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private float critChance = 0.10f;
    [SerializeField] private PlayerInputManager input;
    [SerializeField] private Sword sword;
    [SerializeField] private Spell spell;
    private CharacterStats characterStats;
    IWeapon equippedWeapon;

    void Start()
    {
        equippedWeapon = sword;
        characterStats = new CharacterStats(10, 10);
        
        // add any bonuses related to specific weapons
        characterStats.AddBonus( StatType.Strength , sword.BonusDamage);
        characterStats.AddBonus( StatType.SpellPower , spell.BonusDamage);
    }

    void OnEnable()
    {
        // TODO update to use enum after merge
        input.reference.actions[Actions.LightAttack].performed += PerformWeaponAttack; 
        input.reference.actions[Actions.HeavyAttack].performed += PerformWeaponSpecialAttack;
        input.reference.actions[Actions.SwitchWeapon].performed += SwitchWeapon;
    }

    private void SwitchWeapon(InputAction.CallbackContext callbackContext)
    {
        equippedWeapon = (equippedWeapon == sword) ? (IWeapon) spell : sword;
    }

    public void PerformWeaponAttack(InputAction.CallbackContext callbackContext)
    {
        equippedWeapon.PerformAttack(CalculateDamage());
    }
    public void PerformWeaponSpecialAttack(InputAction.CallbackContext callbackContext)
    {
        equippedWeapon.PerformSpecialAttack();
    }

    private int CalculateDamage()
    {
        StatType type = (equippedWeapon == sword) ? StatType.Strength : StatType.SpellPower;
        int damageToDeal = (characterStats.GetStat(type).GetCalculatedStatValue() * 2);
        damageToDeal += CalculateCrit(damageToDeal);
        
        //TODO damage ui?
        
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        if (Random.value <= critChance)
        {
            int critDamage = (int)(damage * Random.Range(.5f, .75f));
            return critDamage;
        }
        return 0;
    }
}
