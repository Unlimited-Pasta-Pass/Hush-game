using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Spell : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;

    public string Name
    {
        get => "Spell";
    }

    public int CurrentDamage { get; set; }
    [SerializeField] private int bonusDamage = 5;

    public int BonusDamage
    {
        get => bonusDamage;
        set => bonusDamage = value;
    }

    private const int SPECIAL_DAMAGE = 50;

    public void PerformAttack(int damage)
    {
        animator.SetTrigger(PlayerAnimator.SpellAttack);
        CurrentDamage = damage;
    }

    public void PerformSpecialAttack()
    {
        animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
        CurrentDamage = SPECIAL_DAMAGE;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<IEnemy>().TakeDamage(CurrentDamage);
        }
    }
}