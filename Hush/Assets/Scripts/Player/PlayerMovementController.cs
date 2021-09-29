using Enums;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    // Movement Speeds
    public float runSpeed;
    public float walkSpeed;
    public float crouchWalkSpeed;
    
    // Acceleration / Rotation / Movement
    public float accelerationSpeed;
    public float decelerationSpeed;
    public float slideDecelerationSpeed;
    public float movementRatio;
    
    // Network
    public float networkLerpSpeed;
    
    // References
    public Animator animator;
    public NetworkObject networkObject;
    public CharacterController characterController;
    public PlayerInput playerInput;
    
    // Player Speed
    private Vector2 _moveDirection;
    private float _desiredForwardSpeed;
    private float _desiredLateralSpeed;
    private float _actualForwardSpeed;
    private float _actualLateralSpeed;
    
    // Input States
    private bool _sprinting;
    private bool _crouching;

    // Called to update 2-axis movement input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }

    // Called to update sprinting state
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _sprinting = context.ReadValueAsButton();
        }
    }

    // Called to update crouching state
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _crouching = context.ReadValueAsButton();
        }
    }

    // Computed getters
    private bool IsMoveInput => !Mathf.Approximately(_moveDirection.sqrMagnitude, 0f);
    private bool IsSprinting => _actualForwardSpeed > 0 && MovementVelocity.magnitude > walkSpeed + (runSpeed - walkSpeed) / 4;

    private Vector3 MovementVelocity => new Vector3(_actualForwardSpeed, 0, _actualLateralSpeed);

    private void Start()
    {
        // Set the right control scheme
        playerInput.SwitchCurrentControlScheme(ControlSchemes.KeyboardAndMouse);
    }

    private void Update()
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
        
        _desiredForwardSpeed = _moveDirection.normalized.y * speed;
        _desiredLateralSpeed = _moveDirection.normalized.x * speed;

        _actualForwardSpeed = Mathf.MoveTowards(_actualForwardSpeed, _desiredForwardSpeed, acceleration * Time.deltaTime);
        _actualLateralSpeed = Mathf.MoveTowards(_actualLateralSpeed, _desiredLateralSpeed, acceleration * Time.deltaTime);

        // Animate the X/Y player position
        animator.SetFloat(PlayerAnimator.ForwardSpeed, _actualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, _actualLateralSpeed);
        animator.SetFloat(PlayerAnimator.ForwardSpeedSync, _actualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeedSync, _actualLateralSpeed);
        
        // Sprint / Crouch
        animator.SetBool(PlayerAnimator.Sprinting, IsSprinting);
        animator.SetBool(PlayerAnimator.Crouching, _crouching);
        
        // Translate the player at the proper speed
        var movement = transform.rotation * new Vector3(_actualLateralSpeed, 0f, _actualForwardSpeed);
        characterController.Move(movement / movementRatio);
    }

    private void SyncAnimations()
    {
        animator.SetFloat(PlayerAnimator.ForwardSpeed, animator.GetFloat(PlayerAnimator.ForwardSpeedSync), 1.0f, Time.deltaTime * networkLerpSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, animator.GetFloat(PlayerAnimator.LateralSpeedSync), 1.0f, Time.deltaTime * networkLerpSpeed);
    }

    private float GetCharacterSpeed()
    {
        if (IsSliding())
            return 0;
        if (_crouching)
            return crouchWalkSpeed;
        if (_sprinting)
            return runSpeed;
        return walkSpeed;
    }
    
    private float GetCharacterAcceleration()
    {
        if (IsSliding())
            return slideDecelerationSpeed;
        if (IsMoveInput)
            return accelerationSpeed;
        return decelerationSpeed;
    }
    
    private bool IsSliding()
    {
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        return IsSprinting && (_crouching || (animState.IsName(PlayerAnimator.State.Slide) && animState.normalizedTime < 0.75));
    }

}
