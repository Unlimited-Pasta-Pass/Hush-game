using Enums;
using MLAPI;
using Player.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Speed")]
    [Range(0.1f, 10f)]
    public float runSpeed;
    [Range(0.1f, 10f)]
    public float walkSpeed;
    [Range(0.1f, 10f)]
    public float crouchWalkSpeed;
    [Range(0.0f, 10f)]
    public float landMoveSpeed;
    
    [Header("Acceleration")]
    [Range(0.1f, 10f)]
    public float accelerationSpeed;
    [Range(0.1f, 10f)]
    public float decelerationSpeed;
    [Range(0.1f, 10f)]
    public float slideDecelerationSpeed;
    
    [Header("Animations")]
    [Range(0.1f, 5f)]
    public float movementToAnimationRatio;
    
    [Header("Network")]
    [Range(1f, 30f)]
    public float networkLerpSpeed;
    
    [Header("References")]
    public Animator animator;
    public NetworkObject networkObject;
    public Rigidbody body;
    public PlayerInput playerInput;
        
    // Computed getters
    private Vector3 MovementVelocity => new Vector3(_state.ActualForwardSpeed, 0, _state.ActualLateralSpeed);
    private bool IsMoveInput => !Mathf.Approximately(_state.MoveDirection.sqrMagnitude, 0f);
    private bool IsPlayerSprinting => _state.ActualForwardSpeed > 0 && MovementVelocity.magnitude > walkSpeed + (runSpeed - walkSpeed) / 4;
    private bool IsPlayerSliding
    {
        get {
            var animState = animator.GetCurrentAnimatorStateInfo(0);
            return IsPlayerSprinting && (_state.Crouching || (animState.IsName(PlayerStates.Slide) && animState.normalizedTime < 0.75));
        }
    }
    private bool IsPlayerLanding
    {
        get {
            var animState = animator.GetCurrentAnimatorStateInfo(0);
            return animState.IsName(PlayerStates.Land);
        }
    }

    // Shared Player State
    private PlayerState _state;

    // Called to update 2-axis movement input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _state.MoveDirection = context.ReadValue<Vector2>();
        }
    }

    // Called to update sprinting state
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _state.Sprinting = context.ReadValueAsButton();
        }
    }

    // Called to update crouching state
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _state.Crouching = context.ReadValueAsButton();
        }
    }

    private void Start()
    {
        // Set the right control scheme
        playerInput.SwitchCurrentControlScheme(ControlSchemes.KeyboardAndMouse);
        
        // Initialize the character state variable
        _state = PlayerState.Instance;
    }

    private void FixedUpdate()
    {
        if (networkObject.IsLocalPlayer)
        {
            Move();
        }
        else
        {
            SyncAnimations();
        }
    }

    private void Move()
    {
        float speed = GetCharacterSpeed();
        float acceleration = GetCharacterAcceleration();
        
        _state.DesiredForwardSpeed = _state.MoveDirection.normalized.y * speed;
        _state.DesiredLateralSpeed = _state.MoveDirection.normalized.x * speed;

        _state.ActualForwardSpeed = Mathf.MoveTowards(_state.ActualForwardSpeed, _state.DesiredForwardSpeed, acceleration * Time.fixedDeltaTime);
        _state.ActualLateralSpeed = Mathf.MoveTowards(_state.ActualLateralSpeed, _state.DesiredLateralSpeed, acceleration * Time.fixedDeltaTime);
        
        // Translate the player at the proper speed
        var movement = new Vector3(_state.ActualLateralSpeed * movementToAnimationRatio, body.velocity.y, _state.ActualForwardSpeed * movementToAnimationRatio);
        body.velocity = transform.TransformDirection(movement);
        
        // Animate the X/Y player position
        animator.SetFloat(PlayerAnimator.ForwardSpeed, _state.ActualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, _state.ActualLateralSpeed);
        animator.SetFloat(PlayerAnimator.ForwardSpeedSync, _state.ActualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeedSync, _state.ActualLateralSpeed);
        
        // Sprint / Crouch
        animator.SetBool(PlayerAnimator.Sprinting, IsPlayerSprinting);
        animator.SetBool(PlayerAnimator.Crouching, _state.Crouching);
    }

    private void SyncAnimations()
    {
        animator.SetFloat(PlayerAnimator.ForwardSpeed, animator.GetFloat(PlayerAnimator.ForwardSpeedSync), 1.0f, Time.fixedDeltaTime * networkLerpSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, animator.GetFloat(PlayerAnimator.LateralSpeedSync), 1.0f, Time.fixedDeltaTime * networkLerpSpeed);
    }
    
    private float GetCharacterSpeed()
    {
        if (IsPlayerSliding)
            return 0;
        if (IsPlayerLanding)
            return landMoveSpeed;
        if (_state.Crouching)
            return crouchWalkSpeed;
        if (_state.Sprinting)
            return runSpeed;
        return walkSpeed;
    }
    
    private float GetCharacterAcceleration()
    {
        if (IsPlayerSliding)
            return slideDecelerationSpeed;
        if (IsMoveInput)
            return accelerationSpeed;
        return decelerationSpeed;
    }
}
