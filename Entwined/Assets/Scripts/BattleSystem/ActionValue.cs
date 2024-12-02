using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
///<summary>
/// Tính toán Action Value để quyết định thứ tự hành động của nhân vật.
///</summary>
public class ActionValue
{
    // Thang hành động, là một số cho trước.
    // Có thể nhiều hơn hoặc ít đi khi có buff hoặc debuff.
    [SerializeField] private float actionGauge = 10000f;

    /// <summary>
    /// Tính giá trị AV ban đầu.
    /// </summary>
    /// <param name="characterSpeed"></param>
    /// <returns></returns>
    public int StartActionValueCalculator(int characterSpeed)
    {
        return Mathf.CeilToInt(actionGauge / characterSpeed);
    }

    /// <summary>
    /// Tính toán Action Value của nhân vật khi mới khởi tạo trận đấu.
    /// </summary>
    /// <param name="participants"></param>
    public void SetStartActionValue(List<GameObject> participants)
    {
        for (int i = 0; i < participants.Count; i++)
        {
            participants[i].GetComponent<CharacterBattle>().AV = StartActionValueCalculator(participants[i].GetComponent<CharacterBattle>().Speed);
        }
    }

    /// <summary>
    /// Tính toán Action Value sau khi đã sắp xếp theo thứ tự tốc độ nhân vật.
    /// </summary>
    /// <param name="participants"></param>
    /// <param name="currentParticipants"></param>
    public void SetActionValueCalculator(List<GameObject> participants, GameObject currentParticipants)
    {
        int actionValue = StartActionValueCalculator(currentParticipants.GetComponent<CharacterBattle>().Speed);
        foreach (GameObject participant in participants)
        {
            participant.GetComponent<CharacterBattle>().AV -= actionValue;
        }
    }

    /// <summary>
    /// Tiếp tục tính toán Action Value theo từng hiệp hành động tiếp theo.
    /// </summary>
    /// <param name="participants"></param>
    public void SetActionValueCalculator(List<GameObject> participants)
    {
        int actionValue = participants[1].GetComponent<CharacterBattle>().AV;
        foreach (GameObject participant in participants)
        {
            if (participant != null)
            {
                CharacterBattle characterBattle = participant.GetComponent<CharacterBattle>();
                if (characterBattle != null)
                {
                    characterBattle.AV -= actionValue;
                }
            }
        }
    }


}
