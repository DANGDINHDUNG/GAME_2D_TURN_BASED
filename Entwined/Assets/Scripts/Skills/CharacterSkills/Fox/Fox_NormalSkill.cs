using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fox_NormalSkill : Skills
{
    private void Awake()
    {
        skillName = "Tail";
        skillShortDescription = "Deal damage to single enemy and apply Flare stack (Recover 1 skill point)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Deal <color=#00FF00>{0}</color>% damage to single enemy and apply 1 Flare stack (Recover 1 skill point)", 100 + (level * 10));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 0 + (skillLevel * 10)));
        target.GetComponent<CharacterBattle>().AddElementStack(Element.Flare, 1);
        TriggerHitEffect(target.transform.position, 0);
        FieldManager.Instance.IncreaseSkillPoint(cost);
    }
}
