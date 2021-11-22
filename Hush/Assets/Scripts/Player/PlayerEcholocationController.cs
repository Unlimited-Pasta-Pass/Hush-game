using System.Collections.Generic;
using System.Linq;
using Common;
using LOS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class PlayerEcholocationController : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float revealDistance = 20f;
        [SerializeField] private float revealDuration = 5f;
        [SerializeField] private float revealDelay = 7f;

        [Header("References")]
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private PlayerInputManager input;
        
        private List<LOSObjectHider> _hiddenObjectsInRange;

        private bool _canReveal = true;
        
        private float _revealCountdown;
        private float _revealDelta;

        private void OnEnable()
        {
            input.reference.actions[Actions.Reveal].performed += OnEcholocate;
        }

        private void OnDisable()
        {
            input.reference.actions[Actions.Reveal].performed -= OnEcholocate;
        }

        private void Update()
        {
            _revealCountdown += Time.deltaTime;
            _revealDelta += Time.deltaTime;
            
            if (_revealCountdown >= revealDuration && _hiddenObjectsInRange != null)
            {
                foreach (var hiddenObject in _hiddenObjectsInRange)
                {
                    hiddenObject.ResetObjectVisibility();
                }
            }

            if (_revealDelta >= revealDelay)
            {
                _canReveal = true;
            }
        }

        public void OnEcholocate(InputAction.CallbackContext callbackContext)
        {
            if (!_canReveal)
                return;

            _canReveal = false;
            
            effect.Play();
            
            _hiddenObjectsInRange = new List<LOSObjectHider>();
            _hiddenObjectsInRange.AddRange(FindObjectsOfType<LOSObjectHider>().Where(o => Vector3.Distance(transform.position, o.transform.position) <= revealDistance));

            foreach (var hiddenObject in _hiddenObjectsInRange)
            {
                hiddenObject.RevealObject();
            }

            _revealDelta = 0f;
            _revealCountdown = 0f;
        }
    }
}