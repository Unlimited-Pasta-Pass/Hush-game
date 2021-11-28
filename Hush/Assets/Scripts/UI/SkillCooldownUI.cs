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
            if (!GameManager.Instance.CanCastHeavy)
            {
                DisplayHeavyCooldown();
            }
            else
            {
                HideHeavyCooldown();
            }

            if (!GameManager.Instance.CanCastLight)
            {
                DisplayLightCooldown();
            }
            else
            {
                HideLightCooldown();
            }
        }

        private void DisplayHeavyCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetHeavySpellActivationTime();
            float timeRemaining = GameManager.Instance.GetHeavySpellCoolDownTime() - timeElapsed;

            heavyCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            heavyOverlay.SetActive(true);
            heavyCooldown.gameObject.SetActive(true);
        }
    
        private void DisplayLightCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            float timeRemaining = GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed;
            
            lightCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            lightOverlay.SetActive(true);
            lightCooldown.gameObject.SetActive(true);
        }
        
        private void DisplayEchoCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            float timeRemaining = GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed;
            
            lightCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            lightOverlay.SetActive(true);
            lightCooldown.gameObject.SetActive(true);
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
            heavyOverlay.SetActive(false);
            heavyCooldown.gameObject.SetActive(false);
        }
    }
}
