using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public enum StatType { Strength , SpellPower }
    public StatType Type { get; set; }
    public List<StatBonus> Bonuses = new List<StatBonus>();
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }

    public Stat(int baseValue, string statName, string statDescription)
    {
        this.BaseValue = baseValue;
        this.StatName = statName;
        this.StatDescription = statDescription;
    }
    
    public Stat(StatType statType, int baseValue, string statName)
    {
        this.Type = statType;
        this.BaseValue = baseValue;
        this.StatName = statName;
    }
    
    public void AddBonus(StatBonus statbonus)
    {
        Bonuses.Add(statbonus);
    }
    
    public int GetCalculatedStatValue()
    {
        int totalBonuses = 0;
        foreach(StatBonus bonus in Bonuses)
        {
            totalBonuses = bonus.value;
        }

        return BaseValue + totalBonuses;
    }
}
