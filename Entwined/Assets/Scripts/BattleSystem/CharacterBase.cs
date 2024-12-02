using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chỉ số cơ bản xài chung cho cả quái và nhân vật
/// </summary>
public abstract class CharacterBase : MonoBehaviour
{
    #region Chỉ số nhân vật
    [SerializeField] protected string characterName;      // Tên của nhân vật.
    [SerializeField] protected int speed;     // Tốc độ hành động của nhân vật.
    [SerializeField] protected Sprite characterIcon;    // Icon nhân vật trên thanh action bar.
    [SerializeField] protected int baseHp;      // Chỉ số máu cơ bản của nhân vật.
    [SerializeField] protected int baseAttack;      // Chỉ số tấn công cơ bản của nhân vật.
    [SerializeField] protected int baseDamageRES;        // Chỉ số kháng sát thương.
    [SerializeField] protected int level;     // Cấp độ của nhân vật. 
    #endregion

    #region Properties
    public string Name => characterName;
    public int Speed => speed;
    public Sprite CharacterIcon => characterIcon;
    public int BaseHp => baseHp;
    public int BaseAttack => baseAttack;
    public int BaseDamageRES => baseDamageRES;
    public int Level => level;
    #endregion
}
