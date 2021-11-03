using System.Collections;
using System.Collections.Generic;
using Enums;

public class CharacterStats
{
    public List<Stat> stats = new List<Stat>();

    public CharacterStats(int strength, int spellPower)
    {
        stats = new List<Stat>() {
            new Stat(StatType.Strength, strength, "Strength"),
            new Stat(StatType.SpellPower, spellPower, "Spell Power")
        };
    }

    public Stat GetStat(StatType stat)
    {
        return this.stats.Find(x => x.Type == stat);
    }

	public void AddBonus(StatType type, int value)
	{
		GetStat(type).AddBonus(value);
	}
}
