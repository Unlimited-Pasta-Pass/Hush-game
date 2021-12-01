using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Weapon.Enums;

public class SkillBarManager : MonoBehaviour
{
    [SerializeField] private Image heavyIcon;
    [SerializeField] private Image lightIcon;
    [SerializeField] private Sprite stunSprite;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite invisibleSprite;

    private void Start()
    {
        heavyIcon.sprite = GetSpellIcon(GameManager.Instance.GetActiveHeavySpell());
        lightIcon.sprite = GetSpellIcon(GameManager.Instance.GetActiveLightSpell());
    }

    private Sprite GetSpellIcon(SpellType spell)
    {
        Sprite sprite;
        switch (spell)
        {
            case SpellType.FireballSpell:
                sprite = fireSprite;
                break;
            case SpellType.InvisibleSpell:
                sprite = invisibleSprite;
                break;
            case SpellType.StunSpell:
                sprite = stunSprite;
                break;
            default:
                sprite = fireSprite;
                break;
        }
        return sprite;
    }
}
