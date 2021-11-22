﻿using Game;
using Player.Enums;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Parameters

        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float walkSpeed = 2f;

        [Tooltip("Rotation speed of the character")]
        [SerializeField] private float rotationSpeed = 15f;

        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float sprintSpeed = 6f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;

        #endregion
        
        private static InputManager Input => InputManager.Instance;

        private float _playerSpeed;

        private void Start()
        {
            InitializePlayerTransform();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.PlayerIsAlive)
                return;
            
            MovePlayer();
            RotatePlayer();
            
            GameManager.Instance.MovePlayer(transform);
        }
        
        public void InitializePlayerTransform()
        {
            // For some reason disabling the character controller is
            // required to teleport the player to the desired position...
            controller.enabled = false;
            
            transform.position = GameManager.Instance.PlayerTransform.Position.Get();
            transform.rotation = GameManager.Instance.PlayerTransform.Rotation.Get();
            
            _playerSpeed = 0f;
            
            controller.enabled = true;
        }

        private void MovePlayer()
        {
            // Set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = Input.move.normalized.magnitude * (Input.walk ? walkSpeed : sprintSpeed);
            
            // Creates curved result rather than a linear one giving a more organic speed change
            _playerSpeed = Mathf.Lerp(_playerSpeed, targetSpeed, speedChangeRate * Time.deltaTime);

            // Determine displacement
            var horizontalDisplacement = Vector3.forward * (_playerSpeed * Time.deltaTime);
            var verticalDisplacement = Vector3.up * (Physics.gravity.y * Time.deltaTime) * (controller.isGrounded ? 0 : 1);
            
            // Move the player
            controller.Move(transform.TransformDirection(horizontalDisplacement + verticalDisplacement));

            // Update animator
            animator.SetFloat(PlayerAnimator.Speed, _playerSpeed);
        }

        private void RotatePlayer()
        {
            // Set the target direction based on the input
            Vector3 targetDirection = new Vector3(Input.move.x, 0f, Input.move.y);
                
            // Rotate the player only if the player inputs a new direction
            if (targetDirection == Vector3.zero) 
                return;
            
            // Creates curved result rather than a linear one giving a more organic rotation change
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
        }
    }
}