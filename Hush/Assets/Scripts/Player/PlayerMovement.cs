using System;
using Common.Enums;
using Game;
using Player.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Parameters

        [Tooltip("Move speed of the character in m/s")] // [SerializeField] 
        private float walkSpeed;

        [Tooltip("Rotation speed of the character")]
        [SerializeField] private float rotationSpeed = 15f;

        // [Tooltip("Sprint speed of the character in m/s")] // [SerializeField]
        private float sprintSpeed;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;
        
        [Tooltip("The height at which the player will always stay at")]
        [SerializeField] private float playerHeight = 0.0f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private AudioSource footsteps;
        [SerializeField] private AudioClip running;
        [SerializeField] private AudioClip walking;

        #endregion
        
        private static InputManager Input => InputManager.Instance;
        
        public bool IsRunning => _playerSpeed > WalkSpeed + 0.25f * (SprintSpeed - WalkSpeed);
        public bool IsWalking => !IsRunning && _playerSpeed > 1;

        public float WalkSpeed => walkSpeed + GameManager.Instance.GetPlayerSpeedBoost();
        public float SprintSpeed => sprintSpeed + GameManager.Instance.GetPlayerSpeedBoost();
        
        private bool _isAttacking;
        private float _attackDelta;
        private float _playerSpeed;

        private float _attackPauseTime;

        private void Start()
        {
            InitializePlayerTransform();
            
            // set speed from game state
            walkSpeed = GameManager.Instance.PermanentSpeed;
            sprintSpeed = walkSpeed + 4;

        }

        private void Update()
        {
            if(IsWalking)
                footsteps.clip = walking;
            else if (IsRunning)
                footsteps.clip = running;

            if (!footsteps.isPlaying && (IsWalking || IsRunning))
                footsteps.Play();
            else if(!IsRunning && !IsWalking)
                footsteps.Stop();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.PlayerIsAlive)
                return;

            if (_attackDelta >= _attackPauseTime && _isAttacking)
            {
                _isAttacking = false;
            }
            else
            {
                _attackDelta += Time.fixedDeltaTime;
            }
            
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
            
            controller.enabled = true;
        }

        public void OnAttackPerformed(float delay)
        {
            if (TimeManager.Instance.GameIsPaused)
                return;
            
            _attackPauseTime = delay;
            _attackDelta = 0f;

            _isAttacking = true;
            
            // Set the target direction based on the input
            var cameraRay = mainCamera.ScreenPointToRay(Input.look);
            var groundPlane = new Plane(Vector3.up, transform.position);

            if (!groundPlane.Raycast(cameraRay, out var rayLength)) 
                return;
            
            var pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        private void MovePlayer()
        {
            // Set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = Input.move.normalized.magnitude * GetDesiredSpeed();
            
            // Creates curved result rather than a linear one giving a more organic speed change
            _playerSpeed = Mathf.Lerp(_playerSpeed, targetSpeed, speedChangeRate * Time.deltaTime);

            // Determine displacement
            var horizontalDisplacement = Vector3.forward * (_playerSpeed * Time.deltaTime);
            
            // Move the player
            controller.Move(transform.TransformDirection(horizontalDisplacement));
            
            // Lock player on y axis
            var pos = controller.transform.position;
            controller.transform.position = new Vector3(pos.x, playerHeight, pos.z);

            // Update the animator
            animator.SetFloat(PlayerAnimator.Speed, _playerSpeed);
        }

        private void RotatePlayer()
        {
            if (_isAttacking)
                return;
            
            // Set the target direction based on the input
            Vector3 targetDirection = new Vector3(Input.move.x, 0f, Input.move.y);
                
            // Rotate the player only if the player inputs a new direction
            if (targetDirection == Vector3.zero) 
                return;
            
            // Creates curved result rather than a linear one giving a more organic rotation change
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
        }
        
        private float GetDesiredSpeed()
        {
            if (_isAttacking)
                return 0f;
            
            return Input.walk ? WalkSpeed : SprintSpeed;
        }

        public void SetTemporarySpeedModifier(float amount, bool isEnabled)
        {
            if (isEnabled)
            {
                walkSpeed += amount;
                sprintSpeed += amount;
            }
            else
            {
                walkSpeed -= amount;
                sprintSpeed -= amount;
            }
        }
    }
}
