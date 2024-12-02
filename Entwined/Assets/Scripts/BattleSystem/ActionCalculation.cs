using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
///<summary>
/// Thực hiện hành động tính toán các chỉ số như (bạo kích, độ khiêu khích,...).
///</summary>

public class ActionCalculation
{
    #region Tính lượng sát thương đầu ra
    /// <summary>
    /// Kiểm tra đòn tấn công có gây bạo kích hay không
    /// </summary>
    /// <param name="criticalRate"></param>
    /// <returns></returns>
    public bool IsCriticalHit(double criticalRate)
    {
        // Generate a random number between 0 and 1
        float randomValue = UnityEngine.Random.Range(0f, 1.0f);
        // Check if the random number is less than the critical rate to determine a critical hit
        return randomValue < criticalRate / 100;
    }

    /// <summary>
    /// Tính toán lượng damage được cường hóa theo tỉ lệ.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public Tuple<int, bool> EnhanceDamageCalculation(GameObject caster, GameObject target,  int ratio)
    {
        bool isCritical = IsCriticalHit(caster.GetComponent<PlayerBattle>().CurrentCritRate);
        int totalAttack = 0;

        if (isCritical)
        {
            totalAttack = (int)Mathf.Round(caster.GetComponent<PlayerBattle>().CurrentAttack *     // Sát thương cơ bản
                (1 + (float)caster.GetComponent<PlayerBattle>().CurrentCritDamage / 100) *                 // Bội số bạo kích
                (1 + (float)ratio / 100) *                                                          // Bội số kỹ năng     
                DefMultiplierEnemy(caster, target)                                                  // Bội số phòng thủ                                                                                                      
                );
        }
        else
        {
            totalAttack = (int)Mathf.Round(caster.GetComponent<PlayerBattle>().CurrentAttack * (1 + (float)ratio / 100) * DefMultiplierEnemy(caster, target));
        }

        return Tuple.Create(totalAttack, isCritical);
    }

    #endregion
    /// <summary>
    /// Dựa vào xác suất bị tấn công của nhân vật mà sẽ chọn ra một nhân vật bị quái tấn công.
    /// </summary>
    /// <param name="participants"></param>
    /// <returns></returns>
    public GameObject GetTarget(List<GameObject> participants)
    {
        float randomValue = UnityEngine.Random.Range(0, 100);
        float cumulativeProbability = 0;

        foreach (var character in participants)
        {
            cumulativeProbability += character.GetComponent<PlayerBattle>().ProbabilityOfBeingTargeted;
            if (randomValue <= cumulativeProbability)
            {
                return character;
            }
        }

        // Trường hợp không tìm thấy, trả về một nhân vật mặc định hoặc xử lý lỗi
        return null;
    }

    #region Tính độ khiêu khích
    /// <summary>
    /// Tính toán độ khiêu khích của nhân vật.
    /// </summary>
    /// <param name="baseAggro"></param>
    /// <param name="aggroModifier"></param>
    /// <returns></returns>
    public float AggroCalculation(int baseAggro, int aggroModifier)
    {
        return baseAggro * (1 + (float)aggroModifier / 100);
    }

    /// <summary>
    /// Tính toán xác suất bị tấn công của nhân vật.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="participants"></param>
    /// <returns></returns>
    public float ProbabilityOfBeingTargeted(float currentAggro, List<GameObject> participants)
    {
        return (float)(currentAggro / ToTalAggroInTeam(participants)) * 100;
    }

    /// <summary>
    /// Tính tổng độ khiêu khích của toàn bộ nhân vật trong trận.
    /// </summary>
    /// <param name="participants"></param>
    /// <returns></returns>
    public float ToTalAggroInTeam(List<GameObject> participants)
    {
        float sum = 0;

        foreach (var participant in participants)
        {
            sum += participant.GetComponent<PlayerBattle>().CurrentAggro;
        }

        return sum;
    }
    #endregion

    #region Tính bội số
    /// <summary>
    /// Tính bội số phòng thủ của quái.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public float DefMultiplierEnemy(GameObject caster, GameObject target)
    {
        return (float)(caster.GetComponent<CharacterBase>().Level + 20) /
            ((target.GetComponent<CharacterBase>().Level + 20) * (1 - (float)caster.GetComponent<PlayerBattle>().currentDefIgnore / 100f) + caster.GetComponent<CharacterBase>().Level + 20);
    }

    /// <summary>
    /// Tính bội số phòng thủ của nhân vật.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public float DefMultiplier(GameObject caster, GameObject target)
    {
        return (float)target.GetComponent<PlayerBase>().BaseDefend / 
            (target.GetComponent<PlayerBase>().BaseDefend + 200 + 10 * caster.GetComponent<CharacterBase>().Level);
    }
    #endregion
}
