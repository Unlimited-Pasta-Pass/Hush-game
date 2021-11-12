using Common.Enums;
using Common.Interfaces;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace Relics
{
    public class RelicDome : MonoBehaviour, IKillable
    {
        [SerializeField] private int keysNeededToUnlock;
        [SerializeField] private float maxHitPoints = 150f;

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();
    
        private float _hitPoints;
        public float HitPoints
        {
            get => _hitPoints;
            private set
            {
                _hitPoints = value;
                if (_hitPoints <= 0)
                {
                    _hitPoints = 0;
                    Die();
                }
            }
        }

        private bool CanUnlockDome => GameManager.Instance.KeysInPossession.Count >= keysNeededToUnlock;

        private void Awake()
        {
            HitPoints = maxHitPoints;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player) || !CanUnlockDome) 
                return;

            GameManager.Instance.ResetKeys();
            gameObject.SetActive(false);
        }
    
        public void TakeDamage(float damage)
        {
            HitPoints -= damage;
        }

        public void Die()
        {
            // TODO breaking animation
            Killed.Invoke();
            Destroy(gameObject);
        }
    }
}
