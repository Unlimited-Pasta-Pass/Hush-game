using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Enemies;
using Enemies.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

namespace Weapon
{
    public class EnemySword : MonoBehaviour, IWeapon
    {
        #region Parameters

        [Header("Parameters")]
        [SerializeField] private float baseDamage = 10f;
        
        [Header("References")]
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private Enemy enemy;

        #endregion
        
        #region Events

        private void Reset()
        {
            enemyAnimator = GetComponentInParent<Animator>();
            enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (enemy.IsAttacking && other.CompareTag(Tags.Player) && !enemy.AttackedObjects.Contains(other.gameObject))
            {
                other.GetComponent<IKillable>()?.TakeDamage(baseDamage);
                enemy.AttackedObjects.Add(other.gameObject);
            }
        }

        #endregion

        #region Public Variables

        public WeaponType WeaponType => WeaponType.EnemySword;

        public float BaseDamage => baseDamage;
        public float HeavyDamage => 0f;

        #endregion

        #region Public Methods
        
        public void PerformAttack(InputAction.CallbackContext context)
        {
            enemyAnimator.SetTrigger(EnemyAnimator.BaseAttack);
        }

        public void PerformHeavyAttack(InputAction.CallbackContext context)
        {
            PerformAttack(context);
        }

        public float AttemptCrit(float damage)
        {
            return damage;
        }

        #endregion
    }
}
