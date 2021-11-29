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
        private GameObject player;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(Instance.gameObject);
            
            player = GameObject.FindWithTag(Tags.Player);
        }

        public void SetupLightSpell(SpellType chosenSpell)
        {
            if (chosenSpell == SpellType.None)
                chosenSpell = SpellType.InvisibleSpell; // default value
            
            GameManager.Instance.SetActiveLightSpell(chosenSpell);
            ISpell spell = GetSpellComponent(chosenSpell);
            GameManager.Instance.SetLightSpellCooldownTime(spell.LightCooldown);
        }

        public void SetupHeavySpell(SpellType chosenSpell)
        {
            if (chosenSpell == SpellType.None)
                chosenSpell = SpellType.FireballSpell; // default value
            
            GameManager.Instance.SetActiveHeavySpell(chosenSpell);
            ISpell spell = GetSpellComponent(chosenSpell);
            GameManager.Instance.SetHeavySpellCooldownTime(spell.HeavyCooldown);
        }

        private ISpell GetSpellComponent(SpellType type)
        {
            ISpell spell;
            switch (type)
            {
                case SpellType.FireballSpell:
                    spell = player.GetComponent<FireballSpell>();
                    break;
                case SpellType.InvisibleSpell:
                    spell = player.GetComponent<InvisibleSpell>();
                    break;
                case SpellType.StunSpell:
                    spell = player.GetComponent<StunSpell>();
                    break;
                default:
                    spell = null;
                    break;
            }

            return spell;
        }
    
    
    }
}
