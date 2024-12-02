using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnightHonor_Effect : CrystalEffects
{
    private bool isBuffed = false; // Giữ trạng thái buff.
    private int initialDefIgnore; // Biến để lưu giá trị ban đầu của currentDefIgnore.

    public override void BuffedStat(MemoryCrystals crystal)
    {
        crRatePlus = 10 + (2 * crystal.refinementRank); // Tăng tỉ lệ chí mạng.
    }

    public override void Description(MemoryCrystals crystal, TextMeshProUGUI textComponent)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Increases the wearer's crit rate by <color=#00FF00>{0}</color>%. When the wearer casts a " +
            "combat skill or ultimate skill, it will ignore <color=#00FF00>{1}</color>% of the target's defend.", crRatePlus, 24 + (3 * crystal.refinementRank));
    }

    public override void Effect(GameObject caster, GameObject target)
    {
        if (caster == TeamManager.GetInstance().characterInAction)
        {
            // Lấy đối tượng PlayerBattle từ caster.
            var playerBattle = caster.GetComponent<PlayerBattle>();

            if (playerBattle.isAction)
            {
                if (!isBuffed && playerBattle.isUsingSkill)
                {
                    // Lưu giá trị ban đầu của currentDefIgnore khi buff chưa được áp dụng.
                    initialDefIgnore = playerBattle.currentDefIgnore;

                    // Tăng currentDefIgnore.
                    playerBattle.currentDefIgnore += 27;
                    isBuffed = true; // Đánh dấu buff đã được áp dụng.
                }
            }
            else
            {
                if (isBuffed)
                {
                    // Trả lại currentDefIgnore về giá trị ban đầu.
                    playerBattle.currentDefIgnore = initialDefIgnore;
                    isBuffed = false; // Đánh dấu buff đã được gỡ bỏ.
                }
            }
        }
    }
}
