using System;
using Common.Enums;
using UnityEngine;
using Weapon;
using Weapon.Enums;

namespace Game
{
    public class EmpoweredSpellSelector : MonoBehaviour
    {
        private float heavyCooldown;
        private int currentlySelected = 0;
        [SerializeField] private GameObject invisibilityBorder;
        [SerializeField] private GameObject fireBorder;
        [SerializeField] private GameObject stunBorder;
        
        public void OnSubmitHeavy()
        {
            switch(currentlySelected)
            {
                case 1:
                    SetupHeavySpell(SpellType.InvisibleSpell);
                    break;
                case 2:
                    SetupHeavySpell(SpellType.FireballSpell);
                    break;
                case 3:
                    SetupHeavySpell(SpellType.StunSpell);
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

        public void SetupHeavySpell(SpellType chosenSpell)
        {
            if (chosenSpell == SpellType.None)
                chosenSpell = SpellType.FireballSpell; // default value
            
            GameManager.Instance.SetActiveHeavySpell(chosenSpell);
            SetHeavyCooldown(chosenSpell);
            GameManager.Instance.SetHeavySpellCooldownTime(heavyCooldown);
        }

        private void SetHeavyCooldown(SpellType type)
        {
            switch (type)
            {
                case SpellType.FireballSpell:
                    heavyCooldown = GameManager.Instance.FireHeavyCooldown;
                    break;
                case SpellType.InvisibleSpell:
                    heavyCooldown = GameManager.Instance.InvisibleHeavyCooldown;
                    break;
                case SpellType.StunSpell:
                    heavyCooldown = GameManager.Instance.StunHeavyCooldown;
                    break;
                default:
                    heavyCooldown = 10f;
                    break;
            }
        }
    }
}
