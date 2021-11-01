using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class PlayerWeaponController : MonoBehaviour
{
    //public GameObject EquippedWeapon { get; set; }
    
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInputController input;
    
    [SerializeField] private CharacterStats characterStats;
    IWeapon equippedWeapon;
    

    void Start()
    {
        equippedWeapon = GetComponent<IWeapon>();
    }

    void OnEnable()
    {
        // TODO update to use enum after merge
        input.playerInput.actions["Light Attack"].performed += PerformWeaponAttack; 
        input.playerInput.actions["Heavy Attack"].performed += PerformWeaponSpecialAttack;
    }

    void Update()
    {
    }

    public void PerformWeaponAttack(InputAction.CallbackContext callbackContext)
    {
        animator.SetTrigger("Light Attack");
        equippedWeapon.PerformAttack(CalculateDamage());
    }
    public void PerformWeaponSpecialAttack(InputAction.CallbackContext callbackContext)
    {
        animator.SetTrigger("Heavy Attack");
        equippedWeapon.PerformSpecialAttack();
    }

    private int CalculateDamage()
    {
        int damageToDeal = 10 // (characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalculatedStatValue() * 2)
            + Random.Range(2, 8);
        damageToDeal += CalculateCrit(damageToDeal);
        
        //TODO damage ui?
        Debug.Log("Damage dealt: " + damageToDeal);
        
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        if (Random.value <= .10f)
        {
            int critDamage = (int)(damage * Random.Range(.5f, .75f));
            return critDamage;
        }
        return 0;
    }
}
