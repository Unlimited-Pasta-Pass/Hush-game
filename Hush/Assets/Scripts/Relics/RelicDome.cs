using System;
using Common.Enums;
using Common.Interfaces;
using Game;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Relics
{
    public class RelicDome : MonoBehaviour, IKillable
    {
        [SerializeField] private GameObject interactOverlay;
        [SerializeField] private int keysNeededToUnlock;

        private bool playerIsClose = false;
        private UnityEvent _killed;
        private static InputManager Input => InputManager.Instance;

        public UnityEvent attacked;
        public UnityEvent Killed => _killed ??= new UnityEvent();
        public float HitPoints => GameManager.Instance.RelicDomeHitPoints;
        private bool CanUnlockDome => GameManager.Instance.KeysInPossession.Count >= keysNeededToUnlock && InputManager.Instance.interact && playerIsClose;

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.Interact].performed += UnlockDome;
            }
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.Interact].performed -= UnlockDome;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;

            // show interaction text
            interactOverlay.SetActive(true);

            playerIsClose = true;
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (!collider.gameObject.CompareTag(Tags.Player))
                return;

            // hide interaction text
            interactOverlay.SetActive(false);

            playerIsClose = false;
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

        private void UnlockDome(InputAction.CallbackContext context)
        {
            if (!CanUnlockDome)
                return;
            
            Die();
        }
    }
}
