using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat
{
    public enum BaseStatType { Power, AttackSpeed }
    public BaseStatType StatType { get; set; }
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }

    public BaseStat(int baseValue, string statName, string statDescription)
    {
        this.BaseValue = baseValue;
        this.StatName = statName;
        this.StatDescription = statDescription;
    }
    
    public BaseStat(BaseStatType statType, int baseValue, string statName)
    {
        this.StatType = statType;
        this.BaseValue = baseValue;
        this.StatName = statName;
    }
    
    public int GetCalculatedStatValue()
    {
        return BaseValue;
    }
}
