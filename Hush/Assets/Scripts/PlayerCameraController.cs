using Cinemachine;
using Helpers;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : NetworkBehaviour
{
    public float cameraSpeed;
    
    public NetworkObject networkObject;
    public GameObject lookAtTarget;

    private CinemachineFreeLook _freeLookCamera;
    private Vector2 _lookDirection;

    public override void NetworkStart()
    {
        // Only bind the local player to the camera
        if (!networkObject.IsLocalPlayer)
            return;

        _freeLookCamera = CameraHelper.FindFreeLookCamera();
        _freeLookCamera.Follow = transform;
        _freeLookCamera.LookAt = lookAtTarget.transform;
        
        // Disable the camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookDirection = context.ReadValue<Vector2>();
    }

    private void ChangePlayerOrientation(Vector2 direction)
    {
        transform.Rotate(new Vector3(0f, 1f, 0f), direction.x * cameraSpeed * Time.deltaTime);
    }
    
    private void Update()
    {
        ChangePlayerOrientation(_lookDirection);
    }
}
