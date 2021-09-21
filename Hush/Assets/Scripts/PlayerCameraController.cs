using Cinemachine;
using Helpers;
using MLAPI;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public NetworkObject networkObject;
    public GameObject lookAtTarget;

    public override void NetworkStart()
    {
        // Only bind the local player to the camera
        if (!networkObject.IsLocalPlayer)
            return;

        CinemachineFreeLook freeLookCamera = CameraHelper.FindFreeLookCamera();
        freeLookCamera.Follow = transform;
        freeLookCamera.LookAt = lookAtTarget.transform;
        
        // Disable the camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // TODO Move the player in the direction the camera is pointing at
        // use _camera.m_XAxis.Value to rotate the player in the right direction
    }
}
