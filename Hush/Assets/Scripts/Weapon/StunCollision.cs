using Common.Enums;
using Enemies;
using UnityEngine;

namespace Weapon
{
    public class StunCollision : MonoBehaviour
    {
        [SerializeField] private float radius = 10f;

        public void StunEffect(Vector3 center, float duration)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
           
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag(Tags.Enemy))
                {
                    hitCollider.gameObject.GetComponent<Enemy>().Stun(duration);
                }
            }
        }
    }
}