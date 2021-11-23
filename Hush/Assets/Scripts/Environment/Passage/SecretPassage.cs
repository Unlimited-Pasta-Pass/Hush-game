using System;
using Common.Enums;
using UnityEngine;

namespace Environment.Passage
{
    public class SecretPassage : MonoBehaviour
    {
        [Header("Parameters")] [SerializeField]
        private float hideDelay = 5.0f;

        [SerializeField] private float hideDelayOnLeave = 1.0f;

        [Header("References")] [SerializeField]
        private SecretPassageDoor[] passageDoors;

        [SerializeField] private SecretPassageElement[] passageElements;

        private bool _playerInside;
        private bool _shown;
        private float _lastShown;
        private float _lastInside;

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
            foreach (var door in passageDoors)
            {
                door.Init();
            }

            foreach (var floor in passageElements)
            {
                floor.Init();
            }

            Hide(true);
        }

        void Update()
        {
            // Hide after delay if player not in passage
            if (!_playerInside && Time.time - _lastShown >= hideDelay && Time.time - _lastInside >= hideDelayOnLeave)
            {
                Hide();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _lastInside = Time.time;
                _playerInside = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _playerInside = false;
            }
        }

        public void Reveal(bool force = false)
        {
            if (_shown && !force)
                return;

            _shown = true;
            _lastShown = Time.time;

            // Hide doors
            foreach (var door in passageDoors)
                door.Hide();

            // Show passage elements
            foreach (var element in passageElements)
                element.Show();
        }

        public void Hide(bool force = false)
        {
            if (!_shown && !force)
                return;

            _shown = false;

            // Show doors
            foreach (var door in passageDoors)
                door.Show();

            // Hide passage elements
            foreach (var element in passageElements)
                element.Hide();
        }
    }
}