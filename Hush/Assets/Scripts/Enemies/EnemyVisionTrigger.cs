using UnityEngine;

namespace Enemies
{
    public class EnemyVisionTrigger : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;

        void Reset()
        {
            enemy = GetComponentInParent<Enemy>();
        }

        void OnTriggerStay(Collider other)
        {
            enemy.OnVisionTrigger(other);
        }
    }
}
