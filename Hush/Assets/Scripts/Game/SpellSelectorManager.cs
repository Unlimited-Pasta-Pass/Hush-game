using System;
using Common.Enums;
using UnityEngine;
using Weapon;
using Weapon.Enums;

namespace Game
{
    public class SpellSelectorManager : MonoBehaviour
    {
        public static SpellSelectorManager Instance;
        private float heavyCooldown;
        private float lightCooldown;
        
        private int currentlySelected = 0;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
        }
        
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
                default: // select nothing
                    break;
            }
        }
        
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
                default: // select nothing
                    break;
            }
        }
        
        public void SetCurrentlySelected(int selected)
        {
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

        public void SetupHeavySpell(SpellType chosenSpell)
        {
            if (chosenSpell == SpellType.None)
                chosenSpell = SpellType.FireballSpell; // default value
            
            GameManager.Instance.SetActiveHeavySpell(chosenSpell);
            SetHeavyCooldown(chosenSpell);
            GameManager.Instance.SetHeavySpellCooldownTime(heavyCooldown);
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

        public void MoveToNextLevel()
        {
           SceneManager.Instance.LoadNextScene();
        }
    }
}
