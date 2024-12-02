using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lilian_Skill : Skills
{
    private void Awake()
    {
        skillName = "Bloom";
        skillDescription = "Deal damage to all enemy equal 65% attack (Use 3 skill points)";
        skillShortDescription = "Deal damage to all enemy (Use 3 skill points)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Deal damage to all enemy equal <color=#00FF00>{0}</color>% attack (Use 3 skill points)", 65 + (level * 5));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        // Lượng sát thương cho toàn bộ quái trên sân.
        foreach (GameObject enemy in af.GetAllEnemy())
        {
            if (enemy != null)
            {
                enemy.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, enemy, 65 + (skillLevel * 5)));
                TriggerHitEffect(enemy.transform.position, 0);
            }
        }
        af.ResetMultipleEnemiesList();
        FieldManager.Instance.DecreaseSkillPoint(cost);
        caster.GetComponent<PlayerBattle>().IsCompleted = true;
    }
}
