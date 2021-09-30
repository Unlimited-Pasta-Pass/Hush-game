using MLAPI;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Rotation Speed")]
    [Range(1f, 720f)]
    public float rotationSpeed;

    [Header("References")]
    public GameObject playerCamera;
    public NetworkObject networkObject;
    
    private float TargetRotation => playerCamera.transform.eulerAngles.y;

    private void FixedUpdate()
    {
        if (networkObject.IsLocalPlayer)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        // Rotate the player in the camera's orientation
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x, 
            RotateTowards(transform.eulerAngles.y, TargetRotation, rotationSpeed * Time.fixedDeltaTime), 
            transform.eulerAngles.z
        );
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
