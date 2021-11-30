using System;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SkillCooldownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI heavyCooldown;
        [SerializeField] private TextMeshProUGUI lightCooldown;
        [SerializeField] private TextMeshProUGUI echoCooldown;
        [SerializeField] private GameObject heavyOverlay;
        [SerializeField] private GameObject lightOverlay;
        [SerializeField] private GameObject echoOverlay;

        private void Update()
        {
           UpdateLightCooldown();
           UpdateHeavyCooldown();
           UpdateEchoCooldown();
        }

        private void UpdateHeavyCooldown()
        {
            if (GameManager.Instance.CanCastHeavy)
            {
                HideHeavyCooldown();
                return;
            }
            
            float timeElapsed = Time.time - GameManager.Instance.GetHeavySpellActivationTime();
            string timeRemaining = Mathf.RoundToInt(GameManager.Instance.GetHeavySpellCoolDownTime() - timeElapsed).ToString();

            heavyCooldown.text = timeRemaining;
            heavyOverlay.SetActive(true);
            heavyCooldown.gameObject.SetActive(true);
        }
    
        private void UpdateLightCooldown()
        {
            if (GameManager.Instance.CanCastLight)
            {
                HideLightCooldown();
                return;
            }
            
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            string timeRemaining = Mathf.RoundToInt(GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed).ToString();
            
            lightCooldown.text = timeRemaining;
            lightOverlay.SetActive(true);
            lightCooldown.gameObject.SetActive(true);
        }
        
        private void UpdateEchoCooldown()
        {
            if (GameManager.Instance.CanPlayerReveal)
            {
                HideEchoCooldown();
                return;
            }
            
            float timeElapsed = Time.time - GameManager.Instance.EcholocationActivationTime;
            string timeRemaining = Mathf.RoundToInt(GameManager.Instance.EcholocationCooldownTime - timeElapsed).ToString();

            echoCooldown.text = timeRemaining;
            echoOverlay.SetActive(true);
            echoCooldown.gameObject.SetActive(true);
        }

        private void HideLightCooldown()
        {
            lightOverlay.SetActive(false);
            lightCooldown.gameObject.SetActive(false);
        }
        
        private void HideHeavyCooldown()
        {
            heavyOverlay.SetActive(false);
            heavyCooldown.gameObject.SetActive(false);
        }
        
        private void HideEchoCooldown()
        {
            echoOverlay.SetActive(false);
            echoCooldown.gameObject.SetActive(false);
        }
    }
}
