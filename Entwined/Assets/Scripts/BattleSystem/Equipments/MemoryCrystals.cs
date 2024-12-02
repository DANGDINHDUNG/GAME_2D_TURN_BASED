using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MemoryCrystals
{
    #region Defines
    public string crystalName;
    public string crystalID;
    public Sprite crystalSprite;
    public Purity crystalPurify;
    public int crystalLevel;
    public int buffedHp;
    public int buffedDefend;
    public int buffedAttack;
    public Class classRequired;
    public CrystalEffects crystalEffect;

    /// <summary>
    /// Độ tinh luyện của crystal.
    /// </summary>
    public int refinementRank;

    /// <summary>
    /// Id của nhân vật đang trang bị, null nếu crystal đang ko được trang bị.
    /// </summary>
    public string PlayerUsedID;

    /// <summary>
    /// Cấp cao nhất để đột phá lên cấp tiếp theo.
    /// </summary>
    public int maxLevelBreakThrough;
    public long maxExpPoint;
    public long currentExpPoint;

    private static PurifyCrystalNameDatas purifyCrustalNameDatas = new PurifyCrystalNameDatas();
    private static CrystalInfoDatas crystalInfoDatas = new CrystalInfoDatas();
    #endregion

    #region Methods
    public MemoryCrystals(string id, string name, Purity pure, Sprite sprite, CrystalEffects effect, Class classR )
    {
        this.crystalID = id;
        this.crystalName = name;
        this.crystalLevel = 1;
        this.maxLevelBreakThrough = 40;
        this.buffedAttack = 29;
        this.buffedDefend = 18;
        this.buffedHp = 48;
        this.crystalPurify = pure;
        this.refinementRank = 1;
        this.crystalSprite = sprite;
        this.crystalEffect = effect;
        this.PlayerUsedID = "";
        this.classRequired = classR;
    }

    /// <summary>
    /// Tăng cấp cho crystal
    /// </summary>
    public void UpgradeLevel(int value, long remainExp)
    {
        crystalLevel += value;
        IncreaseBuffedStat(value);
        currentExpPoint = remainExp;
    }

    /// <summary>
    /// Các chỉ số buffed được tăng lên theo level crystal
    /// </summary>
    void IncreaseBuffedStat(int value)
    {
        buffedAttack += Mathf.RoundToInt(7.6f * value);
        buffedHp += Mathf.RoundToInt(12.7f * value);
        buffedDefend += Mathf.RoundToInt(4.7f * value);
    }

    public long GetMaxExpPoint(int level)
    {
        return maxExpPoint = 5 * level * level + 20 * level;
    }

    //This allows you to re-generate the GUID for this object by clicking the 'Generate New ID' button in the context menu dropdown for this script
    public static string GenerateID()
    {
        string id = Guid.NewGuid().ToString();
        return id;
    }

    /// <summary>
    /// Tạo ra Crystal mới.
    /// </summary>
    /// <param name="pure"></param>
    /// <returns></returns>
    public static MemoryCrystals GenerateNewMemoryCrystal(Purity pure)
    {
        string id = GenerateID();
        string name = GetRandomCrystalName(pure);
        Sprite sprite = crystalInfoDatas.cData[name].cSprite;
        CrystalEffects effects = crystalInfoDatas.cData[name].cEffect;
        Class classR = crystalInfoDatas.cData[name].rClass;
        return new MemoryCrystals(id, name, pure, sprite, effects, classR);
    }

    /// <summary>
    /// random tên crystal dựa vào purify.
    /// </summary>
    private static string GetRandomCrystalName(Purity pure)
    {
        List<string> crystalNames = purifyCrustalNameDatas.purifyCrystalName[pure];
        int randomIndex = UnityEngine.Random.Range(0, crystalNames.Count);
        return crystalNames[randomIndex];
    }

    #endregion
}

public enum Purity
{
    Faded,
    Pristine,
    Eternal,
}

/// <summary>
/// Danh sách tên crystal ứng với mỗi phẩm.
/// </summary>
[System.Serializable]
public class PurifyCrystalNameDatas
{
    public Dictionary<Purity, List<string>> purifyCrystalName;

    public PurifyCrystalNameDatas()
    {
        purifyCrystalName = new Dictionary<Purity, List<string>>
        { 
          {Purity.Eternal, new List<string> { "Hellfire Core", "Digital Nexus", "Guardian Blossom", "Abyssal Shadecore" } },
          {Purity.Pristine, new List<string> { "Fading Verdure", "Knight's Honor",  "Verdant Grove", "Whirling Gale"} }
        };
    }
}

[System.Serializable]
public class CrystalInfo
{
    public Sprite cSprite;
    public CrystalEffects cEffect;
    public Class rClass;
}

[System.Serializable]
public class CrystalInfoDatas
{
    public Dictionary<string, CrystalInfo> cData;

    public CrystalInfoDatas()
    {
        cData = new Dictionary<string, CrystalInfo>()
        {
            {"Hellfire Core", new CrystalInfo { cSprite = Database.Instance.crystalIcons[0], cEffect = Database.Instance.crystalEffects[0], rClass = Class.Eclipse} },
            {"Digital Nexus", new CrystalInfo { cSprite = Database.Instance.crystalIcons[1], cEffect = Database.Instance.crystalEffects[1], rClass = Class.Arcanist} },
            {"Abyssal Shadecore", new CrystalInfo { cSprite = Database.Instance.crystalIcons[2], cEffect = Database.Instance.crystalEffects[2], rClass = Class.Hexweaver} },
            {"Guardian Blossom", new CrystalInfo { cSprite = Database.Instance.crystalIcons[3], cEffect = Database.Instance.crystalEffects[3], rClass = Class.Oracle} },
            {"Knight's Honor", new CrystalInfo { cSprite = Database.Instance.crystalIcons[4], cEffect = Database.Instance.crystalEffects[4], rClass = Class.Illusionist} },
            {"Fading Verdure", new CrystalInfo { cSprite = Database.Instance.crystalIcons[5], cEffect = Database.Instance.crystalEffects[5], rClass = Class.Sage} },
            {"Verdant Grove", new CrystalInfo { cSprite = Database.Instance.crystalIcons[6], cEffect = Database.Instance.crystalEffects[6], rClass = Class.Oracle} },
            {"Whirling Gale", new CrystalInfo { cSprite = Database.Instance.crystalIcons[7], cEffect = Database.Instance.crystalEffects[7], rClass = Class.Eclipse} },
        };
    }
}

[System.Serializable]
public class CrystalBreakthrough
{
    public int amount;
    public Rarity rarity;
    public long cost;
}

[System.Serializable]
public class BreakthroughIngredientsDatas
{
    public Dictionary<int, CrystalBreakthrough> brDatas;

    public BreakthroughIngredientsDatas()
    {
        brDatas = new Dictionary<int, CrystalBreakthrough>
        {
            {40, new CrystalBreakthrough { amount = 4, rarity = Rarity.Epic, cost = 10000 } },
            {50, new CrystalBreakthrough { amount = 4, rarity = Rarity.Legend, cost = 20000 } },
            {60, new CrystalBreakthrough { amount = 8, rarity = Rarity.Legend, cost = 50000 } },
            {70, new CrystalBreakthrough { amount = 5, rarity = Rarity.Mythic, cost = 100000 } },
            {80, new CrystalBreakthrough { amount = 10, rarity = Rarity.Mythic, cost = 200000 } },
            {90, new CrystalBreakthrough { amount = 15, rarity = Rarity.Mythic, cost = 400000 } },
        };
    }
}

