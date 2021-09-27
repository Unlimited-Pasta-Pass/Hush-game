using Enums;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    public float rotationSpeed;
    public float movementRatio;
    public Animator animator;
    public GameObject playerCamera;
    public NetworkObject networkObject;
    public CharacterController characterController;
    public PlayerInput playerInput;

    private float _desiredForwardSpeed;
    private float _desiredLateralSpeed;
    private float _actualForwardSpeed;
    private float _actualLateralSpeed;
    private bool _sprinting;
    private Vector2 _moveDirection;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _sprinting = context.ReadValueAsButton();
        }
    }

    private bool IsMoveInput => !Mathf.Approximately(_moveDirection.sqrMagnitude, 0f);

    private void Start()
    {
        // Set the right control scheme
        playerInput.SwitchCurrentControlScheme(ControlSchemes.KeyboardAndMouse);
        
        // Disable the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (networkObject.IsLocalPlayer)
        {
            Move();
        }
    }

    private void Move()
    {
        float characterSpeed = _sprinting ? runSpeed : walkSpeed;
        float acceleration = IsMoveInput ? accelerationSpeed : decelerationSpeed;
        
        _desiredForwardSpeed = _moveDirection.normalized.y * characterSpeed;
        _desiredLateralSpeed = _moveDirection.normalized.x * characterSpeed;

        _actualForwardSpeed = Mathf.MoveTowards(_actualForwardSpeed, _desiredForwardSpeed, acceleration * Time.deltaTime);
        _actualLateralSpeed = Mathf.MoveTowards(_actualLateralSpeed, _desiredLateralSpeed, acceleration * Time.deltaTime);

        // Animate the X/Y player position
        animator.SetFloat(PlayerAnimator.ForwardSpeed, _actualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, _actualLateralSpeed);
        
        // Rotate the player in the camera's orientation
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x, 
            RotateTowards(transform.eulerAngles.y, playerCamera.transform.eulerAngles.y, rotationSpeed * Time.deltaTime), 
            transform.eulerAngles.z
        );
        
        // Translate the player at the proper speed
        var movement =  transform.rotation * new Vector3(_actualLateralSpeed, 0f, _actualForwardSpeed);
        characterController.Move(movement / movementRatio);
    }
    
    private float RotateTowards(float current, float target, float maxDelta)
    {
        float diff = target - current;
        if (Mathf.Abs(diff) > 180)
        {
            current += diff > 0 ? 360 : -360;
        }
        return Mathf.MoveTowards(current, target, maxDelta);
    }
    
}
