using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    #region Defines
    public int maxHP;
    public int currentShield;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        maxHP = GetComponent<PlayerDetails>().characterHP;
        currentHp = maxHP;
        currentShield = 0;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Trừ lượng máu hiện tại của quái khi nhận damage từ người chơi.
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(Tuple<int,bool> damage)
    {
        // Nếu tồn tại shield, trừ vào shield thay vì máu.
        if (currentShield > 0)
        {
            // Trừ giáp bằng lượng sát thương
            int remainingDamage = Math.Max(damage.Item1 - currentShield, 0);
            currentShield = Math.Max(currentShield - damage.Item1, 0);
            floatingShieldBar.SetValue(currentShield);
            // Nếu sát thương còn lại sau khi phá giáp, trừ vào máu.
            currentHp = Math.Max(currentHp - remainingDamage, 0);
        }
        else
        {
            // Không có giáp, trừ trực tiếp vào máu.
            currentHp = Math.Max(currentHp - damage.Item1, 0);
        }

        floatingHealthBar.SetValue(currentHp);
        if (currentHp <= 0)
        {
            SpawnHandler.instance.characterOnField.Remove(this.gameObject);
            BattleHandler.instance.participants.Remove(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
