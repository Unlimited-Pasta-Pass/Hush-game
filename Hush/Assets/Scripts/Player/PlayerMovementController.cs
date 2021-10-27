using System;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Parameters

        [SerializeField] 
        [Tooltip("Move speed of the character in m/s")]
        private float walkSpeed = 2f;

        [SerializeField] 
        [Tooltip("Rotation speed of the character")]
        private float rotationSpeed = 15f;

        [SerializeField] 
        [Tooltip("Sprint speed of the character in m/s")]
        private float sprintSpeed = 6f;

        [SerializeField] 
        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInputManager input;

        #endregion

        private float _playerSpeed;
        private Quaternion _playerRotation;

        private void Start()
        {
            _playerRotation = transform.rotation;
        }

        private void Update()
        {
            MovePlayer();
            RotatePlayer();
        }

        private void MovePlayer()
        {
            // Set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = input.move.normalized.magnitude * (input.walk ? walkSpeed : sprintSpeed);
            
            // Creates curved result rather than a linear one giving a more organic speed change
            _playerSpeed = Mathf.Lerp(_playerSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
            
            // Move the player
            controller.Move(transform.TransformDirection(Vector3.forward * (_playerSpeed * Time.deltaTime)));

            // Update animator
            animator.SetFloat(PlayerAnimator.Speed, _playerSpeed);
        }

        private void RotatePlayer()
        {
            // Set the target direction based on the input
            Vector3 targetDirection = new Vector3(input.move.x, 0f, input.move.y);
                
            // Rotate the player only if the player inputs a new direction
            if (targetDirection != Vector3.zero)
            {
                // Creates curved result rather than a linear one giving a more organic rotation change
                _playerRotation = Quaternion.Lerp(_playerRotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);

                // Rotate the player
                transform.rotation = _playerRotation;
            }
        }
    }
}
