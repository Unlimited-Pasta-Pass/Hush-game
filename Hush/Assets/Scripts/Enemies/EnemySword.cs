
using Common;
using UnityEngine;

namespace Enemies
{
    // TODO: PerformAttack() and PerformSpecialAttack() dont really apply to the weapon because they require animations from the character
    public class EnemySword : MonoBehaviour, IWeapon
    {
        #region Parameters

        [Header("References")]
        [SerializeField] private Enemy enemy;

        #endregion

        #region Events

        void Reset()
        {
            enemy = GetComponentInParent<Enemy>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (enemy.IsAttacking && other.CompareTag(Tags.Player))
            {
                // TODO: Damage player
            }
        }

        #endregion

        #region Public Variables

        public int CurrentDamage { get; set; }
        public int BonusDamage { get; set; }

        #endregion
        
        #region Public Methods

        public void PerformAttack(int damage)
        {
            enemy.PerformAttack();
        }

        public void PerformSpecialAttack()
        {
            // TODO: Implement special attack
        }

        #endregion
    }
}
