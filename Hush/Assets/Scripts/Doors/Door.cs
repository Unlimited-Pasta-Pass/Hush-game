using System;
using Common.Enums;
using Doors.enums;
using Game;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Doors
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject interactOverlay;
        private bool playerIsClose = false;
        private Animator anim;
        private bool CanOpenDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic &&
                                    InputManager.Instance.interact;

        private static InputManager Input => InputManager.Instance;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

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

            // show interaction text
            interactOverlay.SetActive(true);

            playerIsClose = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            // hide interaction text
            interactOverlay.SetActive(false);

            playerIsClose = false;
        }

        private void OpenDoor(InputAction.CallbackContext context)
        {
            if (!CanOpenDoor)
                return;

            anim.SetTrigger(DoorAnimator.Open);
            Invoke(nameof(LoadNext), 2f);
        }

        private void LoadNext()
        {
            SceneManager.Instance.LoadNextScene();
        }
    }
}