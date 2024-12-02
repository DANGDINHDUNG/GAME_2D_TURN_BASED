using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VerdantGrove_Effect : CrystalEffects
{
    // Mỗi nhân vật xài chung một hiệu ứng có actionCount riêng.
    private Dictionary<GameObject, int> actionCounts = new Dictionary<GameObject, int>();
    private int healAmount;
    private float healingBoostMultiplier;
    private bool isCount;

    public override void BuffedStat(MemoryCrystals crystal)
    {
        hpPlus = 10 + (2 * crystal.refinementRank);
        outGoingHealingPlus = 12 + (3 * crystal.refinementRank);
    }

    public override void Description(MemoryCrystals crystal, TextMeshProUGUI textComponent)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Increases the wearer's crit rate by <color=#00FF00>{0}</color>%. When the wearer casts a " +
            "combat skill or ultimate skill, it will ignore <color=#00FF00>{1}</color>% of the target's defend.", crRatePlus, 27);
    }

    public override void Effect(GameObject caster, GameObject target)
    {
        if (caster == TeamManager.GetInstance().characterInAction)
        {
            PlayerBattle playerBattle = caster.GetComponent<PlayerBattle>();
            PlayerDetails playerDetails = caster.GetComponent<PlayerDetails>();

            if (playerBattle.isAction)
            {
                if (!isCount)
                {
                    isCount = true;

                    // Kiểm tra xem nhân vật đã có actionCount trong từ điển chưa, nếu chưa thì thêm vào
                    if (!actionCounts.ContainsKey(caster))
                    {
                        actionCounts[caster] = 0;
                    }

                    if (actionCounts[caster] >= 3)
                    {
                        playerBattle.currentOutgoingHealing += outGoingHealingPlus;
                        healingBoostMultiplier = 1 + (float)playerBattle.currentOutgoingHealing / 100;
                        healAmount = Mathf.RoundToInt((600 * (float)playerDetails.characterPercentageHealing / 100 + 50) * healingBoostMultiplier);

                        GameObject character = mulEnemies.GetCharacterWithLowestHp();
                        PlayerStatus playerStatus = character.GetComponent<PlayerStatus>();
                        playerStatus.currentHp += healAmount;

                        if (playerStatus.currentHp >= playerStatus.maxHP)
                        {
                            playerStatus.currentHp = playerStatus.maxHP;
                        }
                        playerStatus.FloatingHealthBar.SetValue(playerStatus.currentHp);
                        Debug.Log("Trigger healing + " + character.name + " + " + healAmount);
                        actionCounts[caster] = 0;
                    }
                    else
                    {
                        actionCounts[caster]++;
                        Debug.Log(actionCounts[caster] + " + " + caster.name);
                    }
                }
            }
            else
            {
                if (isCount)
                {
                    isCount = false;
                }
            }
        }

    }
}
