using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SkillCooldownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI heavyCooldown;
        [SerializeField] private TextMeshProUGUI lightCooldown;
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
            //TODO Grey Out Box
        }
    
        private void DisplayLightCooldown()
        {
            float timeElapsed = Time.time - GameManager.Instance.GetLightSpellActivationTime();
            float timeRemaining = GameManager.Instance.GetLightSpellCoolDownTime() - timeElapsed;
            
            lightCooldown.text = $"{Mathf.RoundToInt(timeRemaining)}";
            //TODO Grey out box
        }

    }
}
