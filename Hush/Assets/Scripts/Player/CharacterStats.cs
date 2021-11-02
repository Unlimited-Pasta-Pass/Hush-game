using System.Collections;
using System.Collections.Generic;

public class CharacterStats
{
    public List<Stat> stats = new List<Stat>();

    public CharacterStats(int strength, int spellPower)
    {
        stats = new List<Stat>() {
            new Stat(Stat.StatType.Strength, strength, "Strength"),
            new Stat(Stat.StatType.SpellPower, spellPower, "Spell Power")
        };
    }

    public Stat GetStat(Stat.StatType stat)
    {
        return this.stats.Find(x => x.Type == stat);
    }

	public void AddBonus(Stat.StatType type, int value)
	{
		GetStat(type).AddBonus(new StatBonus(value));
	}
}
