using System.Collections;
using System.Collections.Generic;
using Plugins;
using UnityEngine;
using Weapon;

public class FireballSpell : PlayerSpell
{
    protected override void CreateSpellAttack(float damage)
    {
        var spellClone = Instantiate(spellPrefab);
        spellClone.transform.position = shootPosition.transform.position;
        spellClone.transform.rotation = shootPosition.transform.rotation;
        
        spellClone.GetComponent<CustomFireProjectile>().ShootPosition = shootPosition.transform;
        spellClone.GetComponent<CustomFireProjectile>().Damage = (int)damage;
    }
}
