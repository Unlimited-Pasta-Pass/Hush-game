using Cinemachine;
using Enums;
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
        
        var cameras = GameObject.FindGameObjectsWithTag(Tags.FreeLookCamera);
        
        if (cameras.Length < 1)
            return;
        
        foreach (var cam in cameras)
        {
            if (cam.TryGetComponent(typeof(CinemachineFreeLook), out Component component))
            {
                CinemachineFreeLook freeLookCamera = (CinemachineFreeLook)component;
                freeLookCamera.Follow = transform;
                freeLookCamera.LookAt = lookAtTarget.transform;
            }
        }
        
        // Disable the camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
