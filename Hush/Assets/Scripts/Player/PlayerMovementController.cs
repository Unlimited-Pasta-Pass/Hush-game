using Enums;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerMovementController : MonoBehaviour
	{
		#region Public Parameters 
		
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float moveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float sprintSpeed = 5.335f;
		[Tooltip("Terminal velocity of the character in m/s")]
		public float terminalVelocity = 53.0f;
		[Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;

		[Header("Component References")]
		public Animator animator;
		public CharacterController controller;
		public PlayerInputController input;
		public NetworkObject networkObject;
		
		#endregion

		#region Private Variables
		
		// Player
		private float _lateralSpeed;
		private float _forwardSpeed;
		private float _verticalVelocity;

		// Timeouts
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		
		#endregion

		private void Start()
		{
			// Reset timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;
		}

		private void Update()
		{
			if (networkObject.IsLocalPlayer)
			{
				JumpAndGravity();
				MovePlayer();
			}
			else
			{
				SyncAnimations();
			}
		}

		private void MovePlayer()
		{
			// Set target speed based on move speed, sprint speed and if sprint is pressed
			Vector2 targetSpeed = input.move.normalized;
			targetSpeed *= input.sprint ? sprintSpeed : moveSpeed;

			// Creates curved result rather than a linear one giving a more organic speed change
			_forwardSpeed = Mathf.Lerp(_forwardSpeed, targetSpeed.y, speedChangeRate * Time.deltaTime );
			_lateralSpeed = Mathf.Lerp(_lateralSpeed, targetSpeed.x, speedChangeRate * Time.deltaTime );

			// Move the player
			var forwardMotion = Vector3.forward * (_forwardSpeed * Time.deltaTime);
			var lateralMotion = Vector3.right * (_lateralSpeed * Time.deltaTime);
			var verticalMotion = new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
			controller.Move(transform.TransformDirection(forwardMotion + lateralMotion + verticalMotion));

			// Update animator
			animator.SetFloat(PlayerAnimator.ForwardSpeed, _forwardSpeed);
			animator.SetFloat(PlayerAnimator.LateralSpeed, _lateralSpeed);
			
			// Update animator Sync values
			animator.SetFloat(PlayerAnimator.ForwardSpeedSync, _forwardSpeed);
			animator.SetFloat(PlayerAnimator.LateralSpeedSync, _lateralSpeed);
		}

		private void JumpAndGravity()
		{
			animator.SetBool(PlayerAnimator.Grounded, controller.isGrounded);
			
			if (controller.isGrounded)
			{
				// Reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				// Update animator
				animator.SetBool(PlayerAnimator.Jumping, false);
				animator.SetBool(PlayerAnimator.Falling, false);

				// Stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump if necessary
				if (input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// The square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

					// Update animator
					animator.SetBool(PlayerAnimator.Jumping, true);
				}

				// Jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// Reset the jump timeout timer
				_jumpTimeoutDelta = jumpTimeout;

				// Update fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// Update animator if using character
					animator.SetBool(PlayerAnimator.Falling, true);
				}

				// If we are not grounded, do not jump
				input.jump = false;
			}

			// Apply gravity over time if under terminal
			// NOTE: Multiply by delta time twice to linearly speed up over time
			if (_verticalVelocity < terminalVelocity)
			{
				_verticalVelocity += gravity * Time.deltaTime;
			}
		}
		
		private void SyncAnimations()
		{
			animator.SetFloat(PlayerAnimator.ForwardSpeed, animator.GetFloat(PlayerAnimator.ForwardSpeedSync), 1.0f, speedChangeRate * Time.deltaTime);
			animator.SetFloat(PlayerAnimator.LateralSpeed, animator.GetFloat(PlayerAnimator.LateralSpeedSync), 1.0f, speedChangeRate * Time.deltaTime);
		}
	}
}