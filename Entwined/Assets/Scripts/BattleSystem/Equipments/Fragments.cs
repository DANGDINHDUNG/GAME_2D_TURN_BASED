using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Fragments
{
    #region Defines
    /// <summary>
    /// Tên fragment.
    /// </summary>
    public string fragmentsName;

    /// <summary>
    /// ID của fragment.
    /// </summary>
    public string fragmentsID;

    /// <summary>
    /// Bộ fragment.
    /// </summary>
    public FragmentSets fragmentSet;

    /// <summary>
    /// Loại fragment.
    /// </summary>
    public FragmentType fragmentType;

    /// <summary>
    /// Hiệu ứng bộ fragment.
    /// </summary>
    public FragmentSetEffect fragmentSetEffect;

    /// <summary>
    /// Cấp hiện tại của fragment. (Tối đa là 20 cấp cho fragment vàng)
    /// </summary>
    public int fragmentLevel;

    /// <summary>
    /// Level tối đa của fragment dựa vào rarity.
    /// </summary>
    public int fragmentMaxLevel;

    /// <summary>
    /// Sprite của fragment.
    /// </summary>
    public Sprite fragmentSprite;

    /// <summary>
    /// Phẩm của fragment.
    /// </summary>
    public Rarity fragmentRarity;

    /// <summary>
    /// Id của nhân vật trang bị fragment. Null nếu fragment chưa được trang bị.
    /// </summary>
    public string playerUsedID;

    /// <summary>
    /// Chỉ số chính của fragment.
    /// </summary>
    public FragmentStats mainStat;

    /// <summary>
    /// Danh sách chỉ số phụ của fragment (tối đa 5 chỉ số).
    /// </summary>
    public List<FragmentStats> subStats;

    public long currentExp;
    public long maxExp;

    private static MainStatValueDatas maxStatValueDatas = new MainStatValueDatas();
    private static FragmentNameDatas fragmentNameDatas = new FragmentNameDatas();
    private static RarityMaxLevelDatas rarityMaxLevelDatas = new RarityMaxLevelDatas();
    private static FragmentSpriteDatas fragmentSpriteDatas = new FragmentSpriteDatas();
    #endregion

    #region Methods
    public Fragments(string fragmentName, string id, FragmentSets set, FragmentType type, FragmentSetEffect effect, Sprite sprite, Rarity rare, int maxLevel, FragmentStats main, string usedId)
    {
        this.fragmentsName = fragmentName;
        this.fragmentsID = id;
        this.fragmentSet = set;
        this.fragmentType = type;
        this.fragmentSetEffect = effect;
        this.fragmentLevel = 1;
        this.fragmentSprite = sprite;
        this.fragmentRarity = rare;
        this.fragmentMaxLevel = maxLevel;
        this.mainStat = main;
        this.playerUsedID = usedId;
        this.subStats = new List<FragmentStats>(4);
    }

    /// <summary>
    /// Tăng cấp fragment.
    /// </summary>
    public void LevelUp(int value, long remainExp)
    {
        for (int i = 0; i < value; i++)
        {
            this.fragmentLevel++;
            this.mainStat.statValue += Math.Round(GetUpdatedValuePerLevel(), 3);

            // Với mỗi 5 level fragment thì sẽ được thêm 1 substat.
            if (fragmentLevel % 5 == 0)
            {
                FragmentStats fs = FragmentStats.GenerateRandomSubStat();

                //// 1 fragment không thể trùng substat.
                while (CheckSubStatExist(fs))
                {
                    Debug.Log(CheckSubStatExist(fs));
                    fs = FragmentStats.GenerateRandomSubStat();
                }
                subStats.Add(fs);

            }
        }

        currentExp = remainExp;
    }

    /// <summary>
    /// Tính giá trị value tăng lên mỗi cấp tùy vào Stat.
    /// </summary>
    /// <returns></returns>
    float GetUpdatedValuePerLevel()
    {
        MainStatValue uv = maxStatValueDatas.mainStatValue[Tuple.Create(mainStat.stats, fragmentRarity)];
        return (float)((uv.maxValue - uv.minValue) / fragmentMaxLevel);
    }

    /// <summary>
    /// Trả về true nếu SubStat đã tồn tại trong list.
    /// </summary>
    /// <param name="fs"></param>
    /// <returns></returns>
    bool CheckSubStatExist(FragmentStats fs)
    {
        var existStat = subStats.FirstOrDefault(s => s.stats == fs.stats);

        if (existStat != null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Tạo ra fragment mới.
    /// </summary>
    /// <returns></returns>
    public static Fragments GenerateNewFragment()
    {
        string id = GenerateID();
        FragmentSets set = GetRandomFragmentSets();
        FragmentType type = GetRandomFragmentType();
        string name = fragmentNameDatas.nameDatas[Tuple.Create(set, type)].fName;
        FragmentSetEffect setEffect = null;
        Sprite sprite = fragmentSpriteDatas.spriteDatas[Tuple.Create(set, type)];
        Rarity rare = GetRandomRarity();
        int maxLevel = rarityMaxLevelDatas.rarityMax[rare];
        string usedId = "";
        FragmentStats mainStat = FragmentStats.GenerateRandomMainStat(rare);

        return new Fragments(name, id, set, type, setEffect, sprite, rare, maxLevel, mainStat, usedId);
    }

    /// <summary>
    /// Random bộ fragment.
    /// </summary>
    /// <returns></returns>
    private static FragmentSets GetRandomFragmentSets()
    {
        Array setArray = Enum.GetValues(typeof(FragmentSets));
        int randomIndex = UnityEngine.Random.Range(0, setArray.Length);
        return (FragmentSets)setArray.GetValue(randomIndex);
    }

    /// <summary>
    /// Random type của set fragment.
    /// </summary>
    /// <returns></returns>
    private static FragmentType GetRandomFragmentType()
    {
        Array typeArray = Enum.GetValues(typeof(FragmentType));
        int randomIndex = UnityEngine.Random.Range(0, typeArray.Length);
        return (FragmentType)typeArray.GetValue(randomIndex);
    }

    /// <summary>
    /// Random rariry của fragment.
    /// </summary>
    /// <returns></returns>
    private static Rarity GetRandomRarity()
    {
        Array rareArray = Enum.GetValues(typeof(Rarity));
        int randomIndex = UnityEngine.Random.Range(0, rareArray.Length);
        return (Rarity)rareArray.GetValue(randomIndex);
    }

    /// <summary>
    /// Exp cần để tăng lên cấp tiếp theo.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="rare"></param>
    public long GetExpPerLevel(int level, Rarity rare)
    {
        switch (rare)
        {
            case Rarity.Mythic:
                return maxExp = 15 * level * level + 220 * level + 560;
            case Rarity.Legend:
                return maxExp = 15 * level * level + 140 * level + 420;
            case Rarity.Epic:
                return maxExp = 15 * level * level + 85 * level + 280;
        }
        return 0;
    }

    //This allows you to re-generate the GUID for this object by clicking the 'Generate New ID' button in the context menu dropdown for this script
    public static string GenerateID()
    {
        string id = Guid.NewGuid().ToString();
        return id;
    }

    #endregion
}

public enum FragmentSets
{
    Ethereal_Echoes,
    SoulboundAegis,
    //AstralReminiscence,
    //VerdantReverie,
    //TempestMemories,
    //CrimsonLegacy,
    //SilentRequiem,
    //FrostboundMemories
}

public enum FragmentType
{
    Essence,
    Vitality,
    Clarity,
    Resonance
}

/// <summary>
/// Tên của fragment ứng với type và set.
/// </summary>
[System.Serializable]
public class FragmentNamePerType
{
    public string fName;
}

[System.Serializable]
public class FragmentNameDatas
{
    public Dictionary<Tuple<FragmentSets, FragmentType>, FragmentNamePerType> nameDatas;

    public FragmentNameDatas()
    {
        nameDatas = new Dictionary<Tuple<FragmentSets, FragmentType>, FragmentNamePerType>
        {
            {Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Essence), new FragmentNamePerType{fName = "Whispering Essence" } },
            {Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Vitality), new FragmentNamePerType{fName = "Fading Vitality" } },
            {Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Clarity), new FragmentNamePerType{fName = "Shimmering Clarity" } },
            {Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Resonance), new FragmentNamePerType{fName = "Echoing Resonance" } },
            {Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Essence), new FragmentNamePerType{fName = "Guardian's Essence" } },
            {Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Vitality), new FragmentNamePerType{fName = "Boundless Vitality" } },
            {Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Clarity), new FragmentNamePerType{fName = "Shielded Clarity" } },
            {Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Resonance), new FragmentNamePerType{fName = "Harmonized Resonance" } },

        };
    }
}

/// <summary>
/// Dữ liệu max level fragment ứng với rarity.
/// </summary>
[System.Serializable]
public class RarityMaxLevelDatas
{
    public Dictionary<Rarity, int> rarityMax;

    public RarityMaxLevelDatas()
    {
        rarityMax = new Dictionary<Rarity, int>
        {
            { Rarity.Epic, 10 },
            { Rarity.Legend, 15 },
            { Rarity.Mythic, 20 },

        };
    }
}

[System.Serializable]
public class FragmentSpriteDatas
{
    public Dictionary<Tuple<FragmentSets, FragmentType>, Sprite> spriteDatas;

    public FragmentSpriteDatas()
    {
        spriteDatas = new Dictionary<Tuple<FragmentSets, FragmentType>, Sprite>
        {
            { Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Essence), Database.Instance.fragmentSprites[0].sprite },
            { Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Vitality), Database.Instance.fragmentSprites[1].sprite },
            { Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Clarity), Database.Instance.fragmentSprites[2].sprite },
            { Tuple.Create(FragmentSets.Ethereal_Echoes, FragmentType.Resonance), Database.Instance.fragmentSprites[3].sprite },
            { Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Essence), Database.Instance.fragmentSprites[4].sprite },
            { Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Vitality), Database.Instance.fragmentSprites[5].sprite },
            { Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Clarity), Database.Instance.fragmentSprites[6].sprite },
            { Tuple.Create(FragmentSets.SoulboundAegis, FragmentType.Resonance), Database.Instance.fragmentSprites[7].sprite }
        };
    }
}



