using System.Collections;
using System.Collections.Generic;

public class CharacterStats
{
    public List<BaseStat> stats = new List<BaseStat>();

    public CharacterStats(int power, int attackSpeed)
    {
        stats = new List<BaseStat>() {
            new BaseStat(BaseStat.BaseStatType.Power, power, "Power"),
            new BaseStat(BaseStat.BaseStatType.AttackSpeed, attackSpeed, "Atk Spd")
        };
    }

    public BaseStat GetStat(BaseStat.BaseStatType stat)
    {
        return this.stats.Find(x => x.StatType == stat);
    }
}
