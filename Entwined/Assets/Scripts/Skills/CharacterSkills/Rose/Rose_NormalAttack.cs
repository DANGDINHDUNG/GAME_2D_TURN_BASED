using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rose_NormalAttack : Skills
{
    private void Awake()
    {
        skillName = "Grown";
        skillDescription = "Deal damage to enemy equal 100% attack (Recover 3 skill points)";
        skillShortDescription = "Deal damage to single enemy (Recover 3 skill points)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Deal damage to enemy equal <color=#00FF00>{0}</color>% attack (Recover 3 skill points)", 100 + (level * 10));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 0 + (skillLevel * 10)));
        TriggerHitEffect(target.transform.position, 0);
        FieldManager.Instance.IncreaseSkillPoint(cost);
    }
}
