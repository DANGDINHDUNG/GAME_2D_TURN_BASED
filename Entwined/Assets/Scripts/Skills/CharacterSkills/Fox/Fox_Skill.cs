using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fox_Skill : Skills
{
    private void Awake()
    {
        skillName = "Tail";
        skillShortDescription = "Deal burst damage to single enemy according to Flare stack (Use 3 skill point)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Deal <color=#00FF00>{0}</color>% damage. When enemy has 5 or more Flare stack, deal additional <color=#00FF00>10</color>% damage per Flare stack to single enemy (Use 3 skill point)", 150 + (level * 10));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        CharacterBattle targetCb = target.GetComponent<CharacterBattle>();
        targetCb.AddElementStack(Element.Flare, 2);
        int count = targetCb.GetElementStack(Element.Flare);

        // Thêm hiệu ứng thiêu đốt vào mục tiêu
        var foxBurn = new FoxBurn(2, Mathf.RoundToInt(caster.GetComponent<PlayerBattle>().CurrentAttack * (float)83/100));
        targetCb.AddEffect(foxBurn);

        if (count >= 5)
        {
            target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 150 * (skillLevel * 10) + count * 10));
            targetCb.SetElementStack(Element.Flare, 0);
            TriggerHitEffect(target.transform.position, 1);

        }
        else
        {
            target.GetComponent<CharacterStatus>().TakeDamage(ac.EnhanceDamageCalculation(caster, target, 150 * (skillLevel * 10)));
            TriggerHitEffect(target.transform.position, 0);
        }
        //TriggerHitEffect(target.transform.position);
        FieldManager.Instance.DecreaseSkillPoint(cost);

        caster.GetComponent<PlayerBattle>().IsCompleted = true;

    }
}
