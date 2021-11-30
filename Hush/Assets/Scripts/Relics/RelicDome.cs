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
        [SerializeField] private int keysNeededToUnlock;
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private GameObject healthBar;
        
        private bool playerIsClose = false;

        public UnityEvent attacked;

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();
        
        public float HitPoints => GameManager.Instance.RelicDomeHitPoints;
        
        private static InputManager Input => InputManager.Instance;

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
            SetDomeVisibility(false);
            
            explosion.Play();
            
            GameManager.Instance.DisableDome();
            Killed.Invoke();
        }

        public void SetDomeVisibility(bool visibility)
        {
            GetComponent<MeshRenderer>().enabled = visibility;
            GetComponent<Collider>().enabled = visibility;
            healthBar.SetActive(visibility);
        }

        private void UnlockDome(InputAction.CallbackContext context)
        {
            if (!CanUnlockDome)
                return;
            
            Die();
        }
    }
}
