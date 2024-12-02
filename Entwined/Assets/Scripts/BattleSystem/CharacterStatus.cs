using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Thông số cơ bản xài chung cho cả quái và nhân vật
/// </summary>
public abstract class CharacterStatus : MonoBehaviour
{
    public int currentHp;   // Máu hiện tại của nhân vật trong trận.
    [SerializeField] protected FloatingBar floatingHealthBar;
    [SerializeField] protected FloatingBar floatingShieldBar;

    #region Properties
    public FloatingBar FloatingHealthBar
    {
        get { return floatingHealthBar; }
        set { floatingHealthBar = value; }
    }

    public FloatingBar FloatingShieldBar
    {
        get { return floatingShieldBar; }
        set { floatingShieldBar = value; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Trừ lượng máu hiện tại của quái khi nhận damage từ người chơi.
    /// </summary>
    /// <param name="damage"></param>
    public abstract void TakeDamage(Tuple<int, bool> damage);
    #endregion
}
