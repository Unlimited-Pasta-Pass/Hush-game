using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using Environment.Passage;
using Game;
using LOS;
using UnityEngine;
using UnityEngine.Events;
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
        
        private readonly List<LOSObjectHider> _hiddenObjectsInRange = new List<LOSObjectHider>();
        private readonly List<ObjectToggle> _togglelablesInRange = new List<ObjectToggle>();

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
                
                foreach (var togglelable in _togglelablesInRange)
                {
                    togglelable.Hide();
                }
            }
        }

        public void OnEcholocate(InputAction.CallbackContext callbackContext)
        {
            if (!GameManager.Instance.CanPlayerReveal)
                return;

            effect.Play();
            
            _hiddenObjectsInRange.Clear();
            _hiddenObjectsInRange.AddRange(FindObjectsOfType<LOSObjectHider>().Where(o => Vector3.Distance(transform.position, o.transform.position) <= revealDistance));
            foreach (var hiddenObject in _hiddenObjectsInRange)
            {
                hiddenObject.RevealObject();
            }
            
            _togglelablesInRange.Clear();
            _togglelablesInRange.AddRange(FindObjectsOfType<ObjectToggle>().Where(o => o.RevealOnEcholocate && Vector3.Distance(transform.position, o.transform.position) <= revealDistance));
            foreach (var togglelable in _togglelablesInRange)
            {
                togglelable.Show();
            }
            
            GameManager.Instance.EcholocationActivationTime = Time.time;
            _revealCountdown = 0f;
        }
    }
}
