using Cinemachine;
using Enums;
using Helpers;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovementController : NetworkBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    public float rotationSpeed;
    public float rotationThreshold;
    
    public Animator animator;
    
    private float _desiredForwardSpeed;
    private float _desiredLateralSpeed;
    private float _actualForwardSpeed;
    private float _actualLateralSpeed;

    private bool _sprinting;
    
    private Vector2 _moveDirection;

    private GameObject _camera;
    
    // TODO Fix this controller to use proper RPC calls
    public override void NetworkStart()
    {
        SelectSpawnPosition();
        
        if (!CameraHelper.TryFindMainCamera(out var mainCamera))
        {
            throw new MissingComponentException("No virtual camera was found in the scene.");
        }

        _camera = mainCamera;
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
    
    private void SelectSpawnPosition()
    {
        // Select a random spawn point for the player
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

    private void Move(Vector2 direction)
    {
        float characterSpeed = _sprinting ? runSpeed : walkSpeed;
        float acceleration = IsMoveInput ? accelerationSpeed : decelerationSpeed;
        
        _desiredForwardSpeed = direction.y * characterSpeed;
        _desiredLateralSpeed = direction.x * characterSpeed;

        _actualForwardSpeed = Mathf.MoveTowards(_actualForwardSpeed, _desiredForwardSpeed, acceleration * Time.deltaTime);
        _actualLateralSpeed = Mathf.MoveTowards(_actualLateralSpeed, _desiredLateralSpeed, acceleration * Time.deltaTime);

        animator.SetFloat(PlayerAnimator.ForwardSpeed, _actualForwardSpeed);
        animator.SetFloat(PlayerAnimator.LateralSpeed, _actualLateralSpeed);
        
        if (Mathf.Abs(_camera.transform.rotation.y - transform.rotation.y) > rotationThreshold)
        {
            float desiredRotation = Mathf.MoveTowards(transform.rotation.y, _camera.transform.rotation.y, rotationSpeed * Time.deltaTime);
            transform.Rotate(new Vector3(0f, 1f, 0f), desiredRotation);
        }
    }

    private void Update()
    {
        Move(_moveDirection);
    }
}
