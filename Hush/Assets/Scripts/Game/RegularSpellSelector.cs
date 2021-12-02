using System;
using Common.Enums;
using UnityEngine;
using Weapon;
using Weapon.Enums;

namespace Game
{
    public class RegularSpellSelector : MonoBehaviour
    {
        private float lightCooldown;
        private int currentlySelected = 0;

        [SerializeField] private GameObject invisibilityBorder;
        [SerializeField] private GameObject fireBorder;
        [SerializeField] private GameObject stunBorder;
        
        public void OnSubmitLight()
        {
            switch(currentlySelected)
            {
                case 1:
                    SetupLightSpell(SpellType.InvisibleSpell);
                    break;
                case 2:
                    SetupLightSpell(SpellType.FireballSpell);
                    break;
                case 3:
                    SetupLightSpell(SpellType.StunSpell);
                    break;
            }
        }
        
        public void SetCurrentlySelected(int selected)
        {
            invisibilityBorder.SetActive(false);
            fireBorder.SetActive(false);
            stunBorder.SetActive(false);

            switch(selected)
            {
                case 1:
                    invisibilityBorder.SetActive(true);
                    break;
                case 2:
                    fireBorder.SetActive(true);
                    break;
                case 3:
                    stunBorder.SetActive(true);
                    break;
            }
            
            currentlySelected = selected;
        }

        public void SetupLightSpell(SpellType chosenSpell)
        {
            if (chosenSpell == SpellType.None)
                chosenSpell = SpellType.InvisibleSpell; // default value
            
            GameManager.Instance.SetActiveLightSpell(chosenSpell);
            SetLightCooldown(chosenSpell);
            GameManager.Instance.SetLightSpellCooldownTime(lightCooldown);
        }

        private void SetLightCooldown(SpellType type)
        {
            switch (type)
            {
                case SpellType.FireballSpell:
                    lightCooldown = GameManager.Instance.FireLightCooldown;
                    break;
                case SpellType.InvisibleSpell:
                    lightCooldown = GameManager.Instance.InvisibleLightCooldown;
                    break;
                case SpellType.StunSpell:
                    lightCooldown = GameManager.Instance.StunLightCooldown;
                    break;
                default:
                    lightCooldown = 5f;
                    break;
            }
        }
    }
}
