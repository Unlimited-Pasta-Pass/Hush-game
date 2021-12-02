using Common.Enums;
using Common.Interfaces;
using Game;
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
        
        private void OnEnable()
        {
            if (killableObject.CompareTag(Tags.Enemy))
            {
                healthSlider.maxValue = 50;
            }
            else if (killableObject.CompareTag(Tags.Dome))
            {
                healthSlider.maxValue = GameManager.Instance.RelicDomeHitPoints;
            }
            else // default
            {
                healthSlider.maxValue = 100;
            }
        }

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