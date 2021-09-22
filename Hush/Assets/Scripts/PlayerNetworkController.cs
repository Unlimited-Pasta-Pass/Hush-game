using MLAPI;
using UnityEngine;

public class PlayerNetworkController : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject virtualCamera;
    public NetworkObject networkObject;
    
    private void Start()
    {
        if (!networkObject.IsLocalPlayer)
        {
            playerCamera.SetActive(false);
            virtualCamera.SetActive(false);
        }
    }
}
