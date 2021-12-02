using System;
using Common.Enums;
using Game;
using TMPro;
using UnityEngine;

namespace Relics
{
    public class FinalRelic : MonoBehaviour
    {
        [SerializeField] private GameObject interactOverlay;
        [SerializeField] private AudioSource collectRelicSound;
    
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;

            interactOverlay.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (InputManager.Instance.interact)
            {
                GameManager.Instance.CollectRelic();
                SetRelicVisibility(false);
            }
        }

        public void SetRelicVisibility(bool visibility)
        {
            AudioSource.PlayClipAtPoint(collectRelicSound.clip, transform.position);
            gameObject.SetActive(visibility);
        }

        private void OnTriggerExit(Collider other)
        {
            interactOverlay.SetActive(false);
        }
    }
}
