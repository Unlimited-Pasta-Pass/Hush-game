using UnityEngine;
using Common;
using Slider = UnityEngine.UI.Slider;

public class KillableHealthUI : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject killableObject;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        healthSlider.value = killableObject.GetComponent<IKillable>().HitPoints;
    }

    private void LateUpdate()
    {
       transform.LookAt(playerCamera.transform.position, Vector3.down);
       transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}