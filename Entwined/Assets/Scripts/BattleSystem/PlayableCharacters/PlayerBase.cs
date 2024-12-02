using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    #region Defines
    [SerializeField] private Sprite characterListAvatar;
    [SerializeField] private int baseDefend;       // Chỉ số phòng thủ cơ bản của nhân vật.
    [SerializeField] private int baseAggro;        // Chỉ số khiêu khích cơ bản của nhân vật (ảnh hưởng tới khả năng bị tấn công của nhân vật đó).
    [SerializeField] private int baseCritRate;
    [SerializeField] private int baseCritDamage;
    [SerializeField] private int baseMastery;
    [SerializeField] private int basePercentageHealing;       // Lượng trị liệu
    [SerializeField] private Element damageElement;
    [SerializeField] private Class characterRole;
    #endregion

    #region Properties
    public Sprite CharacterListAvatar => characterListAvatar;
    public int BaseDefend => baseDefend;
    public int BaseAggro => baseAggro;
    public int BaseCritRate => baseCritRate;
    public int BaseCritDamage => baseCritDamage;
    public int BaseMastery => baseMastery;
    public int BasePercentageHealing => basePercentageHealing;
    public Element DamageElement => damageElement;
    public Class CharacterRole => characterRole;

    /// <summary>
    /// // Điểm số tối đa để lên level tiếp theo.
    /// </summary>
    [SerializeField] private long maxLevelPoint;

    /// <summary>
    /// // Điểm số hiện tại.
    /// </summary>
    [SerializeField] private long currentLevelPoint;
    #endregion

    #region Properties
    public long CurrentLevelPoint => currentLevelPoint;
    #endregion

    #region Methods

    /// <summary>
    /// Set maxLevelPoint.
    /// </summary>
    public long SetLevelProgress(int currentLevel)
    {
        if (currentLevel <= 100)
        {
            return maxLevelPoint = 45 * currentLevel * currentLevel;
        }
        else
        {
            return maxLevelPoint = currentLevel * currentLevel + 450000;
        }
    }

    /// <summary>
    /// Tăng cấp.
    /// </summary>
    /// <param name="value"></param>
    public void LevelUpgrade(int value, long remainExp)
    {
        level += value;
        baseHp += Mathf.RoundToInt(12.5f * value);
        baseAttack += Mathf.RoundToInt(7.5f * value);
        baseDefend += Mathf.RoundToInt(4.7f * value);
        currentLevelPoint = remainExp;
    }
    #endregion
}

public enum Element
{
    Flare,
    Void,
    Zephyr,
    Tempest,
    Lumine,
    Frost,
    Gaia
}

public enum Class
{
    NoType,
    Illusionist,        // Damage đơn, chống chịu kém
    Eclipse,            // Damage đa (thường là 3 mục tiêu), chống chịu trung bình     
    Hexweaver,          // Gây debuff lên quái
    Oracle,             // Hồi máu
    Sage,               // Buff cho đồng minh
    Wardkeeper,         // Bảo vệ đồng minh, chống chịu tốt
    Arcanist            // Gây damage toàn mục tiêu, chống chịu kém
}