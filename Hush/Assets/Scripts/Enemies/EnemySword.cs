using Common;
using UnityEngine;

namespace Enemies
{
    public class EnemySword : MonoBehaviour, IWeapon
    {
        #region Parameters

        [Header("Parameters")]
        [SerializeField] private int damage = 10;
        
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
                other.GetComponent<IKillable>()?.TakeDamage(damage);
            }
        }

        #endregion

        #region Public Variables

        public string WeaponType => "EnemySword";

        public int CurrentDamage { get => damage; set => damage = value; }
        public int BonusDamage { get; set; }

        #endregion

        #region Public Methods

        public void PerformAttack(int attackDamage)
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
