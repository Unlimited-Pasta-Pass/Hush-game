using System;
using Common.Enums;
using Game;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Doors
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private AudioSource openSound;
        
        private bool playerIsClose = false;

        private bool CanOpenDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic &&
                                    InputManager.Instance.interact;

        private static InputManager Input => InputManager.Instance;
        
        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.Interact].performed += OpenDoor;
            }
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.Interact].performed -= OpenDoor;
            }
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
        
        private void OpenDoor(InputAction.CallbackContext context)
        {
            if (!CanOpenDoor)
                return;

            openSound.PlayOneShot(openSound.clip);
            
            // TODO Door opening animation
            wall.SetActive(false);
            
            // respawn back into scene
            SceneManager.Instance.LoadNextScene();
        }
    }
}
