using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class May_Skill : Skills
{
    private void Awake()
    {
        skillName = "Guardian";
        skillDescription = "Create shield to all teammate equal 30% defend of caster. (Use 3 skill points)";
        skillShortDescription = "Create shield to all teammate (Use 3 skill points)";
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Create shield to all teammate equal <color=#00FF00>{0}</color>% defend of caster + 20 (Use 3 skill points)", 30 + (float)(level * 2));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        var shield = Mathf.RoundToInt((float)(caster.GetComponent<PlayerBattle>().Defend * (0.3 + (skillLevel * 2)) + 20));
        
        foreach (GameObject player in SpawnHandler.instance.characterOnField)
        {
            player.GetComponent<PlayerStatus>().currentShield = shield;
            player.GetComponent<PlayerStatus>().FloatingShieldBar.SetValue(shield);
        }
        TriggerHitEffect(caster.transform.position, 0);
        FieldManager.Instance.DecreaseSkillPoint(cost);
        caster.GetComponent<PlayerBattle>().IsCompleted = true;
    }
}
