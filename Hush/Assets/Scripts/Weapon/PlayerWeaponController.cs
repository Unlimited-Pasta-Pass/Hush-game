using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private float critChance = 0.10f;
    [SerializeField] private PlayerInputController input;
    [SerializeField] private Sword sword;
    [SerializeField] private Spell spell;
    private CharacterStats characterStats;
    IWeapon equippedWeapon;

    void Start()
    {
        equippedWeapon = sword;
        characterStats = new CharacterStats(10, 10);
    }

    void OnEnable()
    {
        // TODO update to use enum after merge
        input.playerInput.actions["Light Attack"].performed += PerformWeaponAttack; 
        input.playerInput.actions["Heavy Attack"].performed += PerformWeaponSpecialAttack;
        input.playerInput.actions["SwitchWeapon"].performed += SwitchWeapon;
    }

    void Update()
    {
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
        int damageToDeal = (characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalculatedStatValue() * 2)
            + Random.Range(2, 8);
        damageToDeal += CalculateCrit(damageToDeal);
        
        //TODO damage ui?
        Debug.Log("Damage dealt: " + damageToDeal);
        
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
