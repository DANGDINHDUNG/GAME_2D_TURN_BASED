using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rose_Skill : Skills
{
    private GameObject previousBuffedCharacter;
    private void Awake()
    {
        skillName = "Bloom";
        skillDescription = "Buff attack to target character equal <color=#00FF00>20</color>% of target character's attack (Use 1 skill point)";    
        skillShortDescription = "Buff attack to target character (Use 1 skill point)";    
    }

    public override void Description(TextMeshProUGUI textComponent, int level)
    {
        // Gán giá trị văn bản cho đối tượng TextMeshPro
        textComponent.SetText("Buff attack to target character equal <color=#00FF00>{0}</color>% of target character's attack (Use 1 skill point)", 20 + (level * 0.5f));
    }

    public override void Attack(GameObject caster, GameObject target, int cost)
    {
        Debug.Log("Wait for touch buff");
        StartCoroutine(StartBuff(caster));

        FieldManager.Instance.DecreaseSkillPoint(cost);

    }

    /// <summary>
    /// Kích hoạt hiệu ứng sau khi chọn mục tiêu.
    /// </summary>
    /// <param name="caster"></param>
    /// <returns></returns>
    IEnumerator StartBuff(GameObject caster)
    {
        yield return StartCoroutine(TargetBuffCharacter());

        var roseBuff = new RoseBuff(2, Mathf.RoundToInt(FieldManager.Instance.TargetCharacter.GetComponent<PlayerBattle>().CurrentAttack * (float)(20 + (skillLevel * 0.5f)) / 100));
        
        // Rose chỉ có thể buff duy nhất một nhân vật trên sân.
        // Kiểm tra nếu đang buff cho nhân vật, xóa buff nhân vật đó và add buff vào nhân vật được chọn.
        if (previousBuffedCharacter != null)
        {
            var effect = previousBuffedCharacter.GetComponent<PlayerBattle>().ActiveEffects.Find(x => x.statusName == "Blooming");

            if (effect != null)
            {
                effect.RemoveEffect(previousBuffedCharacter);
                previousBuffedCharacter.GetComponent<PlayerBattle>().ActiveEffects.Remove(effect);
            }
        }
        FieldManager.Instance.TargetCharacter.GetComponent<PlayerBattle>().AddEffect(roseBuff);
        previousBuffedCharacter = FieldManager.Instance.TargetCharacter;
        TriggerHitEffect(caster.transform.position, 0);
        // Thực hiện hành động với đối tượng đã chọn
        caster.GetComponent<PlayerBattle>().IsCompleted = true;
    }
}
