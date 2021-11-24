using System.Collections;
using System.Collections.Generic;
using Game;
using Plugins;
using UnityEngine;
using Weapon;
using Weapon.Enums;

public class StunSpell : PlayerSpell
{
    void Start()
    {
        GameManager.Instance.SetActiveSpell(WeaponType.StunSpell);
    }
    protected override void CreateSpellAttack(float damage)
    {
        var spellClone = Instantiate(spellPrefab);
        spellClone.transform.position = shootPosition.transform.position;
        spellClone.transform.rotation = shootPosition.transform.rotation;
        
        spellClone.GetComponent<StunProjectileCustom>().ShootPosition = shootPosition.transform;
        spellClone.GetComponent<StunProjectileCustom>().Damage = 0;
    }
}
