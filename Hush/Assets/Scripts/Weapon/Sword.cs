using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common;
using UnityEngine;
using Enums;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private int bonusDamage = 5;
    [SerializeField] private Animator playerAnimator;

    public string WeaponType => "Sword";
    public int CurrentDamage { get; set; }
    
    public int BonusDamage
    {
        get => bonusDamage;
        set => bonusDamage = value;
    }

    private const int SPECIAL_DAMAGE = 100;

    private void Reset()
    {
        playerAnimator = GetComponentInParent<Animator>();
    }

    public void PerformAttack(int damage)
    {
        playerAnimator.SetTrigger(PlayerAnimator.LightAttack);
        CurrentDamage = damage;
    }

    public void PerformSpecialAttack()
    {
        playerAnimator.SetTrigger(PlayerAnimator.HeavyAttack);
        CurrentDamage = SPECIAL_DAMAGE;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(Tags.Enemy))
        {
            var stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(PlayerAnimator.Layer.UpperBody);
            if (stateInfo.IsName(PlayerAnimator.State.LightAttack) || stateInfo.IsName(PlayerAnimator.State.HeavyAttack))
            {
                var enemy = col.GetComponent<IEnemy>();
                enemy.TakeDamage(CurrentDamage);
            }
        }
    }
}
