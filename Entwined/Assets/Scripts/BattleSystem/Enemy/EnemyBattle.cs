using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : CharacterBattle
{
    #region Core MonoBehaviour
    private void Start()
    {
        CharacterBase enemyBase = GetComponent<CharacterBase>();
        var currentLevel = enemyBase.Level;
        currentAttack = Mathf.RoundToInt(0.07f * currentLevel * currentLevel + currentLevel + enemyBase.BaseAttack);

        if (currentLevel <= 80)
        {
            currentSpeed = enemyBase.Speed;
        }
        else currentSpeed = enemyBase.Speed + 32;
    }

    private void Update()
    {
        TriggerStatusEffect();

    }
    #endregion

    #region Methods
    /// <summary>
    /// Hành động của quái khi đến lượt.
    /// </summary>
    /// <param name="manager"></param>
    public override void TakeTurn(BattleHandler manager)
    {
        base.TakeTurn(manager);
        StartCoroutine(TargetCharacter());       
    }

    /// <summary>
    /// Thực hiện hành động tấn công của quái.
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetCharacter()
    {
        // Đợi một thời gian quái mới bắt đầu hành động.
        yield return new WaitForSeconds(1.5f);

        // Chọn random một nhân vật trên sân để tấn công dựa vào điểm aggro.
        GameObject attackedCharacter = ac.GetTarget(SpawnHandler.instance.characterOnField);

        attackedCharacter.GetComponent<CharacterStatus>().TakeDamage(Tuple.Create(currentAttack, false));
        Debug.Log(attackedCharacter.name + " Take: " + currentAttack + " damage - CurrentHp: " + attackedCharacter.GetComponent<CharacterStatus>().currentHp);
        isCompleted = true;
    }
    #endregion
}
