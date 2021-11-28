using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using Environment.Passage;
using Game;
using LOS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerEcholocation : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float revealDistance = 20f;
        [SerializeField] private float revealDuration = 5f;
        [SerializeField] private float revealDelay = 7f;

        [Header("References")]
        [SerializeField] private ParticleSystem effect;
        
        private static InputManager Input => InputManager.Instance;
        
        private List<LOSObjectHider> _hiddenObjectsInRange;

        private float _revealCountdown;

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
                Input.reference.actions[Actions.Reveal].performed += OnEcholocate;

            GameManager.Instance.EcholocationCooldownTime = revealDelay;
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
                Input.reference.actions[Actions.Reveal].performed -= OnEcholocate;
        }

        private void Update()
        {
            _revealCountdown += Time.deltaTime;

            if (_revealCountdown >= revealDuration && _hiddenObjectsInRange != null)
            {
                foreach (var hiddenObject in _hiddenObjectsInRange)
                {
                    hiddenObject.ResetObjectVisibility();
                }
            }
        }

        public void OnEcholocate(InputAction.CallbackContext callbackContext)
        {
            if (!GameManager.Instance.CanReveal)
                return;

            effect.Play();
            
            _hiddenObjectsInRange = new List<LOSObjectHider>();
            _hiddenObjectsInRange.AddRange(FindObjectsOfType<LOSObjectHider>().Where(o => Vector3.Distance(transform.position, o.transform.position) <= revealDistance));

            foreach (var hiddenObject in _hiddenObjectsInRange)
            {
                hiddenObject.RevealObject();
            }

            var passageDoorList = FindObjectsOfType<SecretPassageDoor>().Where(o => Vector3.Distance(transform.position, o.transform.position) <= revealDistance);
            foreach (var door in passageDoorList)
            {
                door.RevealPassage();
            }

            GameManager.Instance.EcholocationActivationTime = Time.time;
            _revealCountdown = 0f;
        }
    }
}
