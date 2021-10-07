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

        [Header("Player")] 
        [SerializeField] 
        [Tooltip("Crouch speed of the character in m/s")]
        private float crouchSpeed = 1.5f;

        [SerializeField] 
        [Tooltip("Move speed of the character in m/s")]
        private float moveSpeed = 3.0f;

        [SerializeField] 
        [Tooltip("Rotation speed of the character")]
        private float rotationSpeed = 15f;

        [SerializeField] 
        [Tooltip("Sprint speed of the character in m/s")]
        private float sprintSpeed = 6f;

        [SerializeField] 
        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [SerializeField] 
        [Tooltip("Deceleration during slide animation")]
        public float slideDeceleration = 7.0f;

        [SerializeField] 
        [Tooltip("Normalized time at which the slide deceleration ends")] [Range(0.1f, 1f)]
        public float slideDecelerationEndTime = 0.9f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInputController input;

        #endregion

        #region Computed Getters

        private bool IsPlayerSprinting => _playerSpeed > (moveSpeed + sprintSpeed) / 2;

        private bool IsPlayerSliding
        {
            get
            {
                var animState = animator.GetCurrentAnimatorStateInfo(0);
                return (IsPlayerSprinting && input.crouch) || (animState.IsName(PlayerAnimationStates.Slide) && animState.normalizedTime < slideDecelerationEndTime);
            }
        }

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
            float targetSpeed = input.move.normalized.magnitude * GetTargetSpeed();
            
            // Creates curved result rather than a linear one giving a more organic speed change
            float changeRate = IsPlayerSliding ? slideDeceleration : speedChangeRate;
            _playerSpeed = Mathf.Lerp(_playerSpeed, targetSpeed, changeRate * Time.deltaTime);
            
            // Move the player
            controller.Move(transform.TransformDirection(Vector3.forward * (_playerSpeed * Time.deltaTime)));

            // Update animator
            animator.SetBool(PlayerAnimator.Sprinting, IsPlayerSprinting);
            animator.SetBool(PlayerAnimator.Crouching, input.crouch);
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

        private float GetTargetSpeed()
        {
            if (IsPlayerSliding || input.crouch)
                return crouchSpeed;
            if (input.sprint)
                return sprintSpeed;
            return moveSpeed;
        }
    }
}
