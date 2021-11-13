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

        public UnityEvent attacked;

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();
        
        public float HitPoints => GameManager.Instance.RelicDomeHitPoints;

        private bool CanUnlockDome => GameManager.Instance.KeysInPossession.Count >= keysNeededToUnlock;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player) || !CanUnlockDome) 
                return;

            GameManager.Instance.ResetKeys();
            SetDomeVisibility(false);
        }
    
        public void TakeDamage(float damage)
        {
            if (!GameManager.Instance.DomeWasHit)
            {
                attacked ??= new UnityEvent();
                attacked.Invoke();
            }
            
            if (!GameManager.Instance.AttackDome(damage))
                Die();
        }

        public void Die()
        {
            // TODO breaking animation
            Killed.Invoke();
            SetDomeVisibility(false);
        }

        public void SetDomeVisibility(bool visibility)
        {
            gameObject.SetActive(visibility);
        }
    }
}
