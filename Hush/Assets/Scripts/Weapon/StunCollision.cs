using Common.Enums;
using Enemies;
using UnityEngine;

namespace Weapon
{
    public class StunCollision : MonoBehaviour
    {
        [SerializeField] private float lightRadius = 5f;
        [SerializeField] private float heavyRadius = 10f;

        public void StunEffect(Vector3 center, float duration, bool isHeavy)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, isHeavy ? heavyRadius : lightRadius);
           
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