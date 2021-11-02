using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Spell : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;
    public int CurrentDamage { get; set; }
    public int BonusDamage { get; set; }

    private const int SPECIAL_DAMAGE = 50;

    void Awake()
    {
        BonusDamage = 5;
    }

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
