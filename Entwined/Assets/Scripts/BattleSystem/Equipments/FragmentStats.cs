using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FragmentStats
{
    public StatType statType;
    public Stats stats;
    public double statValue;

    private static SubStatLimitsData statLimitsData = new SubStatLimitsData();
    private static MainStatValueDatas mainStatValueDatas = new MainStatValueDatas();

    public FragmentStats(StatType type, Stats stat, double value)
    {
        this.statType = type;
        this.stats = stat;
        this.statValue = value;
    }

    /// <summary>
    /// Cộng chỉ số khi fragment lên được một cấp nhất định.
    /// </summary>
    public void Grasp()
    {
        Stats randomStat = GetRandomStat();

        double randomValue = GetRandomValueWithinLimits(randomStat);

        statValue = randomValue;
    }

    /// <summary>
    /// Lấy ngẫu nhiên chỉ số trong Stats
    /// </summary>
    /// <returns></returns>
    public static Stats GetRandomStat()
    {
        Array statsArray = Enum.GetValues(typeof(Stats));
        int randomIndex = UnityEngine.Random.Range(0, statsArray.Length);
        return (Stats)statsArray.GetValue(randomIndex);
    }

    /// <summary>
    /// Random ngẫu nhiên giá trị cho stats
    /// </summary>
    /// <param name="stats"></param>
    /// <returns></returns>
    public static double GetRandomValueWithinLimits(Stats stats)
    {
        SubStatLimits limits = statLimitsData.subStatlimits[stats];
        return UnityEngine.Random.Range((float)limits.min, (float)limits.max);
    }

    /// <summary>
    /// Tạo ngẫu nhiên subStat.
    /// </summary>
    /// <returns></returns>
    public static FragmentStats GenerateRandomSubStat()
    {
        StatType statType = StatType.SubStat;
        Stats randomStat = GetRandomStat();
        double statValue;
        if (GetRandomValueWithinLimits(randomStat) < 1)
        {
            statValue = Math.Round(GetRandomValueWithinLimits(randomStat), 3);
        }
        else statValue = Math.Round(GetRandomValueWithinLimits(randomStat), 0);

        return new FragmentStats(statType, randomStat, statValue);
    }

    /// <summary>
    /// Tạo ngẫu nhiên mainStat.
    /// </summary>
    /// <returns></returns>
    public static FragmentStats GenerateRandomMainStat(Rarity rare)
    {
        StatType statType = StatType.MainStat;
        Stats[] possibleStats = { Stats.pHp, Stats.pAtk, Stats.pDef, Stats.crRate, Stats.crDmg, Stats.mastery, Stats.healingPlus };
        Stats randomStat = possibleStats[UnityEngine.Random.Range(0, possibleStats.Length)];
        double statValue = mainStatValueDatas.mainStatValue[Tuple.Create(randomStat, rare)].minValue;

        return new FragmentStats(statType, randomStat, statValue);
    }


}

/// <summary>
/// Chỉ số của fragments
/// </summary>
public enum Stats
{
    atk,
    pAtk,
    def,
    pDef,
    hp,
    pHp,
    crRate,
    crDmg,
    mastery,
    healingPlus
}

/// <summary>
/// Loại chỉ số (Chỉ số chính và chỉ số phụ).
/// </summary>
public enum StatType
{
    MainStat,
    SubStat
}

/// <summary>
/// Khoảng giới hạn giá trị các chỉ số của thuộc tính dòng phụ.
/// </summary>
public class SubStatLimits
{
    public double min;
    public double max;
}

/// <summary>
/// Min, max của thuộc tính dòng phụ.
/// </summary>
[System.Serializable]
public class SubStatLimitsData
{
    public Dictionary<Stats, SubStatLimits> subStatlimits;

    public SubStatLimitsData()
    {
        subStatlimits = new Dictionary<Stats, SubStatLimits>
        {
            { Stats.hp, new SubStatLimits { min = 320, max = 580 } },
            { Stats.pHp, new SubStatLimits { min = 0.064, max = 0.116 } },
            { Stats.atk, new SubStatLimits { min = 30, max = 60 } },
            { Stats.def, new SubStatLimits { min = 40, max = 70 } },
            { Stats.pAtk, new SubStatLimits { min = 0.064, max = 0.116 } },
            { Stats.pDef, new SubStatLimits { min = 0.081, max = 0.147 } },
            { Stats.crRate, new SubStatLimits { min = 0.063, max = 0.105 } },
            { Stats.crDmg, new SubStatLimits { min = 0.126, max = 0.21 } },
            { Stats.mastery, new SubStatLimits { min = 10, max = 30 } },
            { Stats.healingPlus, new SubStatLimits { min = 0.05, max = 0.12 } }
        };
    }
}

/// <summary>
/// value của thuộc tính dòng chính
/// </summary>
[System.Serializable]
public class MainStatValue
{
    public double minValue;
    public double maxValue;
}

[System.Serializable]
public class MainStatValueDatas
{
    public Dictionary<Tuple<Stats, Rarity>, MainStatValue> mainStatValue;

    public MainStatValueDatas()
    {
        mainStatValue = new Dictionary<Tuple<Stats, Rarity>, MainStatValue>
        {
            // Rarity.Mythic
            { Tuple.Create(Stats.pAtk, Rarity.Mythic), new MainStatValue { minValue = 0.066, maxValue = 0.33 } },
            { Tuple.Create(Stats.pHp, Rarity.Mythic), new MainStatValue { minValue = 0.066, maxValue = 0.33 } },
            { Tuple.Create(Stats.pDef, Rarity.Mythic), new MainStatValue { minValue = 0.083, maxValue = 0.415 } },
            { Tuple.Create(Stats.crRate, Rarity.Mythic), new MainStatValue { minValue = 0.044, maxValue = 0.22 } },
            { Tuple.Create(Stats.crDmg, Rarity.Mythic), new MainStatValue { minValue = 0.088, maxValue = 0.44 } },
            { Tuple.Create(Stats.mastery, Rarity.Mythic), new MainStatValue { minValue = 12, maxValue = 120 } },
            { Tuple.Create(Stats.healingPlus, Rarity.Mythic), new MainStatValue { minValue = 0.052, maxValue = 0.26 } },
            
            // Rarity.Legend
            { Tuple.Create(Stats.pAtk, Rarity.Legend), new MainStatValue { minValue = 0.049, maxValue = 0.205 } },
            { Tuple.Create(Stats.pHp, Rarity.Legend), new MainStatValue { minValue = 0.049, maxValue = 0.205 } },
            { Tuple.Create(Stats.pDef, Rarity.Legend), new MainStatValue { minValue = 0.062, maxValue = 0.26 } },
            { Tuple.Create(Stats.crRate, Rarity.Legend), new MainStatValue { minValue = 0.033, maxValue = 0.138 } },
            { Tuple.Create(Stats.crDmg, Rarity.Legend), new MainStatValue { minValue = 0.066, maxValue = 0.278 } },
            { Tuple.Create(Stats.mastery, Rarity.Legend), new MainStatValue { minValue = 10, maxValue = 90 } },
            { Tuple.Create(Stats.healingPlus, Rarity.Legend), new MainStatValue { minValue = 0.039, maxValue = 0.163 } },

            // Rarity.Epic
            { Tuple.Create(Stats.pAtk, Rarity.Epic), new MainStatValue { minValue = 0.043, maxValue = 0.146 } },
            { Tuple.Create(Stats.pHp, Rarity.Epic), new MainStatValue { minValue = 0.043, maxValue = 0.146 } },
            { Tuple.Create(Stats.pDef, Rarity.Epic), new MainStatValue { minValue = 0.055, maxValue = 0.187 } },
            { Tuple.Create(Stats.crRate, Rarity.Epic), new MainStatValue { minValue = 0.029, maxValue = 0.098 } },
            { Tuple.Create(Stats.crDmg, Rarity.Epic), new MainStatValue { minValue = 0.058, maxValue = 0.197 } },
            { Tuple.Create(Stats.mastery, Rarity.Epic), new MainStatValue { minValue = 6, maxValue = 68 } },
            { Tuple.Create(Stats.healingPlus, Rarity.Epic), new MainStatValue { minValue = 0.035, maxValue = 0.119 } },

        };
    }
}



