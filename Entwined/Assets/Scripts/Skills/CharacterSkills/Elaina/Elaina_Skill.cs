using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Elaina_Skill : Skills
{    
    private void Awake()
    {
        skillName = "Bloom";
        skillDescription = "Deal damage to enemy equal 200% attack, and splash to 2 enemy neareast equal 35% attack (Use 3 skill points)";
        skillShortDescription = "Deal damage to enemy and splash to 2 enemy near by (Use 3 skill points)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Deal damage to enemy equal <color=#00FF00>{0}</color>% attack, and splash to 2 enemy neareast equal <color=#00FF00>{1}</color>% attack (Use 3 skill points)", 200 + (level * 10), 35 + (level * 5));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        // Index của quái được chọn trong List.
        int index = SpawnHandler.instance.enemyOnField.IndexOf(target);
        
        target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 100 + (skillLevel * 10)));
        TriggerHitEffect(target.transform.position, 0);


        if (af.GetNearByEnemy(index) != null)
        {
            // Lượng sát thương lan ra 2 bên khi tấn công quái được chọn.
            foreach (GameObject enemy in af.GetNearByEnemy(index))
            {
                enemy.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 35 + (skillLevel * 5)));
            }
        }

        af.ResetMultipleEnemiesList();
        FieldManager.Instance.DecreaseSkillPoint(cost);
        caster.GetComponent<PlayerBattle>().IsCompleted = true;
    }
}
