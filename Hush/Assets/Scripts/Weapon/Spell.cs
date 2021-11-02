using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Spell : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;
    [SerializeField] private int spellLifetime = 5;
    [SerializeField] private GameObject spellPrefab;
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
        CreateSpellAttack();
        CurrentDamage = damage;
    }

    public void PerformSpecialAttack()
    {
        animator.SetTrigger(PlayerAnimator.SpellSpecialAttack);
        CreateSpellAttack();
        CurrentDamage = SPECIAL_DAMAGE;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<IEnemy>().TakeDamage(CurrentDamage);
        }
    }

    private void CreateSpellAttack()
    {
        
        Vector3 SpellSpawnLocation = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject spellClone = Instantiate(spellPrefab, SpellSpawnLocation, Quaternion.identity);
        spellClone.GetComponent<Rigidbody>().velocity = this.transform.forward;
        Debug.Log(this.transform.forward);
        Destroy(spellClone, spellLifetime);

    }
}
