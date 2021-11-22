using System;
using Common.Enums;
using Game;
using Player;
using UnityEngine;

namespace Doors
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wall;

        private bool _isDoorOpen = true; // TODO: modify this value when the door opening conditionals are met
        private bool playerIsClose = false;

        private bool CanOpenDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic &&
                                    InputManager.Instance.interact;

        private void Update()
        {
            OpenDoor();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            playerIsClose = true;
            // TODO UI showing interact button
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            playerIsClose = false;
            // TODO UI hiding interact button
        }

        private void OpenDoor()
        {
            if (!CanOpenDoor)
                return;

            _isDoorOpen = true;
            
            // TODO Door opening animation
            wall.SetActive(false);
            
            // respawn back into scene
            SceneManager.Instance.LoadNextScene();
        }
    }
}
