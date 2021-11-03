using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using DigitalRuby.PyroParticles;

public class Spell : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spellPrefab; 
    [SerializeField] private GameObject shootPosition;

    public string WeaponType => "Spell";

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
        CurrentDamage = damage;
        animator.SetTrigger(PlayerAnimator.SpellAttack);
        CreateSpellAttack(CurrentDamage);
        
    }

    public void PerformSpecialAttack()
    {
        CurrentDamage = SPECIAL_DAMAGE;
        animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
        CreateSpellAttack(CurrentDamage);
    }

    private void CreateSpellAttack(int currentDamage)
    {
        Vector3 SpellSpawnLocation = new Vector3(shootPosition.transform.position.x, shootPosition.transform.position.y, shootPosition.transform.position.z);
        GameObject spellClone = Instantiate(spellPrefab, SpellSpawnLocation, shootPosition.transform.rotation);
		spellClone.GetComponent<FireProjectileScript>().ShootPosition = shootPosition.transform;
    }
}