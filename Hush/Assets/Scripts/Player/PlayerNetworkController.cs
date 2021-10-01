using MLAPI;
using UnityEngine;

public class PlayerNetworkController : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private NetworkObject networkObject;
    
    private void Start()
    {
        if (!networkObject.IsLocalPlayer)
        {
            playerCamera.SetActive(false);
            virtualCamera.SetActive(false);
        }
    }
}
