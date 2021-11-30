using System;
using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using UnityEngine;

namespace Environment.Passage
{
    public class SecretPassage : MonoBehaviour, ITogglelable
    {
        [Header("Parameters")] 
        [SerializeField] private float hideDelay = 5.0f;
        [SerializeField] private float hideDelayOnLeave = 1.0f;

        [Header("References")] [SerializeField]
        private SecretPassageDoor[] passageDoors;

        [SerializeField] private SecretPassageElement[] passageElements;

        private bool _shown;
        private float _lastShown;
        private float _lastInside;
        private readonly HashSet<Collider> _insideTrigger = new HashSet<Collider>();

        public bool RevealOnEcholocate => false;
        public bool Shown => _shown;

        public void Reset()
        {
            passageDoors = GetComponentsInChildren<SecretPassageDoor>();
            passageElements = GetComponentsInChildren<SecretPassageElement>();
        }

        void Start()
        {
            // Start hidden
            _lastShown = Time.time;
            _lastInside = Time.time;
            Hide(true);
        }

        void Update()
        {
            // Always attempt to hide since hiding is based on a timer
            Hide();
        }

        void OnTriggerEnter(Collider other)
        {
            _insideTrigger.Add(other);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _lastInside = Time.time;
            }
        }

        void OnTriggerExit(Collider other)
        {
            _insideTrigger.Remove(other);
        }

        public void Show(bool force = false)
        {
            if (_shown && !force)
                return;

            _shown = true;
            _lastShown = Time.time;

            // Show doors
            foreach (var door in passageDoors)
                door.Show();

            // Show passage elements
            foreach (var element in passageElements)
                element.Show();
        }

        public void Hide(bool force = false)
        {
            if (!_shown && !force)
                return;
        
            // Check preconditions to hide
            // Hide after delay if no objects in passage
            if ((_insideTrigger.Count > 0 || Time.time - _lastShown < hideDelay || Time.time - _lastInside < hideDelayOnLeave) && !force)
                return;

            _shown = false;

            // Hide doors
            foreach (var door in passageDoors)
                door.Hide();

            // Hide passage elements
            foreach (var element in passageElements)
                element.Hide();
        }
    }
}
