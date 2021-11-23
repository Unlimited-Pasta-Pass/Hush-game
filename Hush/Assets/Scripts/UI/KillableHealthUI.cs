using Common.Interfaces;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class KillableHealthUI : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 cameraOffset;
    
        [Header("References")]
        [SerializeField] private GameObject killableObject;
        [SerializeField] private Slider healthSlider;

        private void Update()
        {
            healthSlider.value = killableObject.GetComponent<IKillable>().HitPoints;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(cameraOffset);
        }
    }
}