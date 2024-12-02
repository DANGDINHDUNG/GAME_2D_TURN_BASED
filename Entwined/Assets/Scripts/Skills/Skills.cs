using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Skills : MonoBehaviour
{
    #region Defines
    [SerializeField] public string skillName;
    [SerializeField] public Sprite skillIcon;
    [SerializeField] public string skillShortDescription;
    [SerializeField] public string skillDescription;

    public List<ParticleSystem> hitEffect;

    //[SerializeField] protected string skillType;
    public int skillLevel;

    [SerializeField] protected ActionFunction af;

    [SerializeField] protected ActionCalculation ac;
    #endregion

    #region Properties
    public Sprite SkillIcon => skillIcon;
    #endregion
    public void LevelUp()
    {
        if (skillLevel < 10)
        {
            skillLevel++;
        }
    }
    public abstract void Attack(GameObject caster, GameObject target, int cost);
    public abstract void Description(TextMeshProUGUI textComponent, int level);
    public void TriggerHitEffect(Vector3 position, int index)
    {
        if (hitEffect != null)
        {
            // Instantiate the particle system at the enemy's position and rotation
            ParticleSystem effect = Instantiate(hitEffect[index], position, Quaternion.identity);
            effect.Play();
            // Optionally, destroy the particle system after it has finished playing
            Destroy(effect.gameObject, effect.main.duration);
        }
        else
        {
            Debug.LogWarning("Hit effect particle system not assigned!");
        }
    }

    /// <summary>
    /// Kiểm tra nếu người chơi chưa thực hiện hành động sẽ tiến hành đợi.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForUserInput()
    {
        // Khi người chơi chưa chạm vào màn hình để thực hiện hành động.
        while (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
        {
            // Trả về null tức là chưa thực hiện hành động.
            yield return null;
        }
    }

    /// <summary>
    /// Thực hiện hành động tiến công của nhân vật khi người chơi chọn đúng UI mục tiêu.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TargetBuffCharacter()
    {
        bool hasLogged = false;
        Notification_UI.instance.ShowNotification("Select character avatar to buff", 1f);

        while (!hasLogged)
        {
            // Kiểm tra nếu người chơi đã ấn vào màn hình mới thực hiện hành động phía dưới.
            yield return StartCoroutine(WaitForUserInput());

            // Kiểm tra nếu lần chạm nằm trên UI
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                // Tạo ray từ vị trí chạm và kiểm tra xem có phải Image mục tiêu không
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.GetTouch(0).position
                };

                // Lấy danh sách các đối tượng UI được chạm
                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // Kiểm tra nếu có kết quả trong danh sách
                if (results.Count > 0)
                {
                    // Lấy thông tin parent của đối tượng UI đầu tiên
                    GameObject parentResult = results[0].gameObject.transform.parent.gameObject;

                    // Kiểm tra nếu parentResult là mục tiêu mong muốn
                    if (parentResult.GetComponent<PlaceHolder_UI>() != null)
                    {
                        hasLogged = true;
                        FieldManager.Instance.TargetCharacter = parentResult.GetComponent<PlaceHolder_UI>().targetedCharacter;

                    }
                }
            }
        }
    }
}

[System.Serializable]
public class SkillLevelUp
{
    public int amount;
    public Rarity rarity;
    public long cost;
}

[System.Serializable]
public class SkillLevelUpDatas
{
    public Dictionary<int, SkillLevelUp> skillDatas;

    public SkillLevelUpDatas()
    {
        skillDatas = new Dictionary<int, SkillLevelUp>
        {
            {1, new SkillLevelUp { amount = 2, rarity = Rarity.Epic, cost = 2000 } },
            {2, new SkillLevelUp { amount = 3, rarity = Rarity.Epic, cost = 3000 } },
            {3, new SkillLevelUp { amount = 2, rarity = Rarity.Legend, cost = 6000 } },
            {4, new SkillLevelUp { amount = 3, rarity = Rarity.Legend, cost = 9000 } },
            {5, new SkillLevelUp { amount = 4, rarity = Rarity.Legend, cost = 12000 } },
            {6, new SkillLevelUp { amount = 6, rarity = Rarity.Legend, cost = 18000 } },
            {7, new SkillLevelUp { amount = 5, rarity = Rarity.Mythic, cost = 45000 } },
            {8, new SkillLevelUp { amount = 8, rarity = Rarity.Mythic, cost = 67000 } },
            {9, new SkillLevelUp { amount = 10, rarity = Rarity.Mythic, cost = 90000 } },
            {10, new SkillLevelUp { amount = 12, rarity = Rarity.Mythic, cost = 112000 } },
        };
    }
}
