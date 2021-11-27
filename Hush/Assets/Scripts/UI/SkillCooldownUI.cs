using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SkillCooldownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI heavyCooldown;
        [SerializeField] private TextMeshProUGUI lightCooldown;
        [SerializeField] private GameObject heavyOverlay;
        [SerializeField] private GameObject lightOverlay;
        private void Update()
        {
            if (!SpellManager.Instance.CanCastHeavy)
            {
                DisplayHeavyCooldown();
            }
            else
            {
                HideHeavyCooldown();
            }

            if (!SpellManager.Instance.CanCastLight)
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
            heavyCooldown.enabled = true;
        }
    
        private void DisplayLightCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            float timeRemaining = GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed;
            
            lightCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            lightOverlay.SetActive(true);
            lightCooldown.enabled = true;
        }

        private void HideLightCooldown()
        {
            lightOverlay.SetActive(false);
            lightCooldown.enabled = false;
        }
        
        private void HideHeavyCooldown()
        {
            heavyOverlay.SetActive(false);
            lightCooldown.enabled = false;
        }
    }
}
