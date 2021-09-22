using Cinemachine;
using Helpers;
using MLAPI;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public NetworkObject networkObject;
    public GameObject lookAtTarget;

    private CinemachineFreeLook _virtualCamera;

    public override void NetworkStart()
    {
        // Only bind the local player to the camera
        if (!networkObject.IsLocalPlayer)
            return;

        if (!CameraHelper.TryFindVirtualCamera(out var virtualCamera))
        {
            throw new MissingComponentException("No virtual camera was found in the scene.");
        }
        
        _virtualCamera = virtualCamera;
        _virtualCamera.Follow = transform;
        _virtualCamera.LookAt = lookAtTarget.transform;
        
        // Disable the camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
