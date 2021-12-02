using System;
using Common.Enums;
using Common.Interfaces;
using Game;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Relics
{
    public class RelicDome : MonoBehaviour, IKillable
    {
        [SerializeField] private GameObject interactOverlay;
        [SerializeField] private int keysNeededToUnlock;
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private GameObject healthBar;
        
        [SerializeField] private AudioSource shatterSound;
        [SerializeField] private AudioSource hitSound;

        private bool shatterTriggered = false;
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

            playerIsClose = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;
            
            // show interaction text
            if(GameManager.Instance.KeysInPossession.Count >= keysNeededToUnlock)
                interactOverlay.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            playerIsClose = false;
            
            // hide interaction text
            interactOverlay.SetActive(false);
        }
    
        public void TakeDamage(float damage)
        {
            if (!GameManager.Instance.DomeWasHit)
            {
                attacked ??= new UnityEvent();
                attacked.Invoke();
            }
            
            hitSound.Play();
            
            if (!GameManager.Instance.AttackDome(damage))
                Die();
        }

        public void Die()
        {
            if (shatterTriggered)
                return;

            shatterTriggered = true;
            shatterSound.Play();
            SetDomeVisibility(false);
            
            explosion.Play();
            interactOverlay.SetActive(false);
            
            GameManager.Instance.DisableDome();
            Killed.Invoke();
        }

        public void SetDomeVisibility(bool visibility)
        {
            GetComponent<MeshRenderer>().enabled = visibility;
            GetComponent<Collider>().enabled = visibility;
            GetComponent<NavMeshObstacle>().carving = false;
            healthBar.SetActive(visibility);
        }

        private void UnlockDome(InputAction.CallbackContext context)
        {
            if (!CanUnlockDome)
                return;
            
            Die();
            interactOverlay.SetActive(false);
        }
    }
}
