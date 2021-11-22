using System;
using Common.Enums;
using Common.Interfaces;
using Game;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Relics
{
    public class RelicDome : MonoBehaviour, IKillable
    {
        [SerializeField] private int keysNeededToUnlock;

        private bool playerIsClose = false;

        public UnityEvent attacked;

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();
        
        public float HitPoints => GameManager.Instance.RelicDomeHitPoints;

        private bool CanUnlockDome => GameManager.Instance.KeysInPossession.Count >= keysNeededToUnlock && InputManager.Instance.interact && playerIsClose;

        private void Update()
        {
            UnlockDome();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;

            playerIsClose = true;
            // TODO UI showing interact button
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.CompareTag(Tags.Player))
                return;
        
            playerIsClose = false;
            // TODO UI hiding interact button
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
            
            SetDomeVisibility(false);
            
            GameManager.Instance.DisableDome();
            Killed.Invoke();
        }

        public void SetDomeVisibility(bool visibility)
        {
            gameObject.SetActive(visibility);
        }

        private void UnlockDome()
        {
            if (!CanUnlockDome)
                return;
            
            Die();
        }
    }
}
