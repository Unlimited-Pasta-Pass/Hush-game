using MLAPI;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float rotationSpeed;

    public GameObject playerCamera;
    public NetworkObject networkObject;
    
    private float TargetRotation => playerCamera.transform.eulerAngles.y;

    private void Start()
    {
        // Disable the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
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
            RotateTowards(transform.eulerAngles.y, TargetRotation, rotationSpeed * Time.deltaTime), 
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
