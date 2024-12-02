using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar_UI : MonoBehaviour
{
    [SerializeField] private Image characterIcon;
    [SerializeField] private Image border;
    [SerializeField] private Image placeHolder;
    [SerializeField] private TextMeshProUGUI actionValue;

    // Start is called before the first frame update
    void Awake()
    {
        actionValue = GetComponentInChildren<TextMeshProUGUI>();
        ClearSlot();
    }

    /// <summary>
    /// Lấy dữ liệu từ nhưng nhân vật đang hành động từ đó xuất lên action bar icon của nhân vật đó.
    /// </summary>
    /// <param name="slot"></param>
    public void Init(CharacterBattle slot)
    {
        UpdateUISlot(slot);
    }

    private void Update()
    {
        // Nếu không ở trong trận sẽ ẩn action bar.
        if (!FieldManager.Instance.InBattle)
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// Xuất icon của nhân vật trong trận lên action bar theo thứ tự hành động.
    /// Trả về null nếu ô đó không có nhân vật hành động.
    /// </summary>
    /// <param name="slot"></param>
    public void UpdateUISlot(CharacterBattle slot)
    {
        if (slot != null)
        {
            characterIcon.color = Color.white;
            characterIcon.sprite = slot.gameObject.GetComponent<CharacterBase>().CharacterIcon;
            actionValue.text = slot.AV.ToString();
            SetTransparent(border, 0.5f);
            SetTransparent(placeHolder, 0.8f);
        }
        else ClearSlot();
    }

    /// <summary>
    /// Làm trống ô.
    /// </summary>
    public void ClearSlot()
    {
        characterIcon.sprite = null;
        actionValue.text = null;
        characterIcon.color = Color.clear;
        SetTransparent(border, 0f);
        SetTransparent(placeHolder, 0f);

    }

    private void SetTransparent(Image image, float offset)
    {
        var color = image.color;
        color.a = offset;
        image.color = color;
    }
}
