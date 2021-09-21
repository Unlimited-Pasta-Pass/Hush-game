using Enums;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovementController : NetworkBehaviour
{
    public float turnSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    
    public Animator animator;
    
    private float _desiredSpeed;
    private float _actualSpeed;

    private bool _sprinting;
    
    private Vector2 _moveDirection;
    
    // TODO Fix this controller to use proper RPC calls
    public override void NetworkStart()
    {
        var spawns = GameObject.FindGameObjectsWithTag(Tags.Spawn);
        if (spawns.Length < 1)
        {
            Debug.LogError("A spawn point is required in the scene.");
            return;
        }

        var spawnIndex = Random.Range(0, spawns.Length - 1);
        transform.position = spawns[spawnIndex].transform.position;
        transform.rotation = spawns[spawnIndex].transform.rotation;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _sprinting = context.ReadValueAsButton();
    }

    private bool IsMoveInput
    {
        get { return !Mathf.Approximately(_moveDirection.sqrMagnitude, 0f); }
    }

    private void Move(Vector2 direction)
    {
        if (!IsMoveInput && Mathf.Approximately(_actualSpeed, 0f))
            return;
        
        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        float characterSpeed = _sprinting ? runSpeed : walkSpeed;
        
        _desiredSpeed = direction.magnitude * characterSpeed;
        float acceleration = IsMoveInput ? accelerationSpeed : decelerationSpeed;

        _actualSpeed = Mathf.MoveTowards(_actualSpeed, _desiredSpeed, acceleration * Time.deltaTime);

        animator.SetFloat(PlayerAnimator.ForwardSpeed, _actualSpeed);

        // TODO Fix the rotation so that it's not axis-dependant but rather character-root-angle-dependant
        if (IsMoveInput)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, turnSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        Move(_moveDirection);
    }
}
