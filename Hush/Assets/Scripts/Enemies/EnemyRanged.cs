using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyRanged : Enemy
    {
        [Header("Spell Parameters")]
        [SerializeField] private float shootDelay = 2.0f;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform shootPosition;
        
        public override void PerformAttack()
        {
            base.PerformAttack();
            StartCoroutine(FireProjectile());
        }

        private IEnumerator FireProjectile()
        {
            yield return new WaitForSeconds(shootDelay);
            Vector3 offsetPosition = Quaternion.AngleAxis(-attackRotationOffset, Vector3.up) * shootPosition.localPosition;
            Vector3 spellPosition = transform.TransformPoint(offsetPosition);
            Vector3 playerDir = _player.transform.position - spellPosition;
            playerDir.y = 0;
            Instantiate(projectilePrefab, spellPosition, Quaternion.LookRotation(playerDir));
        }
    }
}
