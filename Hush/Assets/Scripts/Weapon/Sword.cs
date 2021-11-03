using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Enums;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;

    public string WeaponType => "Sword";

    public int CurrentDamage { get; set; }

    [SerializeField] private int bonusDamage = 5;

    public int BonusDamage
    {
        get => bonusDamage;
        set => bonusDamage = value;
    }

    private const int SPECIAL_DAMAGE = 100;

    public void PerformAttack(int damage)
    {
        animator.SetTrigger(PlayerAnimator.LightAttack);
        CurrentDamage = damage;
    }

    public void PerformSpecialAttack()
    {
        animator.SetTrigger(PlayerAnimator.HeavyAttack);
        CurrentDamage = SPECIAL_DAMAGE;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(Tags.Enemy))
        {
            col.GetComponent<IEnemy>().TakeDamage(CurrentDamage);
        }
    }
}