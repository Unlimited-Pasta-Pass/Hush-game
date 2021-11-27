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

            if (!SpellManager.Instance.CanCastLight)
            {
                DisplayLightCooldown();
            }
        }

        private void DisplayHeavyCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetHeavySpellActivationTime();
            float timeRemaining = GameManager.Instance.GetHeavySpellCoolDownTime() - timeElapsed;

            heavyCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            heavyOverlay.SetActive(true);
        }
    
        private void DisplayLightCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            float timeRemaining = GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed;
            
            lightCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            lightOverlay.SetActive(true);
        }

    }
}
