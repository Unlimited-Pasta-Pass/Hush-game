using Common;
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
        [SerializeField] private PlayerInputManager input;

        #endregion

        private float _playerSpeed;
        private Quaternion _playerRotation;

        private void Start()
        {
            _playerRotation = transform.rotation;
        }
        
        private void FixedUpdate()
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
            if (targetDirection == Vector3.zero) 
                return;
            
            // Creates curved result rather than a linear one giving a more organic rotation change
            _playerRotation = Quaternion.Lerp(_playerRotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);

            // Rotate the player
            transform.rotation = _playerRotation;
        }
    }
}
