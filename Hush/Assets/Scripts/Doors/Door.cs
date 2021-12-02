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
        [SerializeField] private AudioSource openSound;

        private bool playerIsClose = false;
        private Animator anim;

        private bool CanOpenDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic &&
                                    InputManager.Instance.interact && playerIsClose;

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

            playerIsClose = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            // show interaction text
            if (!GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic)
                interactOverlay.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;

            playerIsClose = false;

            // hide interaction text
            interactOverlay.SetActive(false);
        }

        private void OpenDoor(InputAction.CallbackContext context)
        {
            if (!CanOpenDoor)
                return;

            openSound.Play();
            anim.SetTrigger(DoorAnimator.Open);
            // TODO: Remove When floor reward Is moved to UI
            Invoke(nameof(LoadNext), 2f);
            interactOverlay.SetActive(false);
        }

        // TODO: Remove When floor reward Is moved to UI
        private void LoadNext()
        {
            SceneManager.Instance.LoadPowerScene();
        }
    }
}