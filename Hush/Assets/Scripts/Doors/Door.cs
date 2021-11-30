using System;
using Common.Enums;
using Game;
using UnityEngine;

namespace Doors
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject interactionOverlay;

        private bool _isDoorOpen = true; // TODO: modify this value when the door opening conditionals are met

        private bool CanGoThroughDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic && _isDoorOpen;
    
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;

            if (other.gameObject.CompareTag(Tags.Player))
            {
                // toggle interaction script
                transform.GetChild(0).gameObject.SetActive(true);
            }

            if (CanGoThroughDoor)
            {
                // TODO Door opening animation
                wall.SetActive(false);
                
                // respawn back into scene
                SceneManager.Instance.LoadNextScene();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                // toggle interaction script
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
