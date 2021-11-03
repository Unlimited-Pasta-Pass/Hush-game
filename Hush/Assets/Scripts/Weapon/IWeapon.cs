using System.Collections.Generic;

public interface IWeapon
{
    string WeaponType { get; }
    int CurrentDamage { get; set; }
    int BonusDamage { get; set; }

    void PerformAttack(int damage);
    void PerformSpecialAttack();
}