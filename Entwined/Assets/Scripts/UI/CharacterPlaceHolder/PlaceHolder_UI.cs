using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceHolder_UI : MonoBehaviour
{
    [SerializeField] private Image characterIcon;
    [SerializeField] private Image border;
    [SerializeField] private Image status;
    [SerializeField] public FloatingBar floatingHealthBar;
    [SerializeField] public FloatingBar floatingShieldBar;
    public GameObject targetedCharacter;

    // Start is called before the first frame update
    void Awake()
    {
        ClearSlot();
    }

    /// <summary>
    /// Lấy dữ liệu từ đội hình hiện tại của người chơi.
    /// Từ đó xuất ra icon các nhân vật trong đội.
    /// </summary>
    /// <param name="slot"></param>
    public void Init(PlayerStatus slot)
    {
        targetedCharacter = slot.gameObject;
        UpdatePlaceHolderSlot(slot);
    }

    private void Update()
    {
        // Nếu không ở trong trận sẽ ẩn action bar.
        if (!FieldManager.Instance.InBattle)
        {
            ClearSlot();
        }

        //UpdatePlaceHolderSlot(targetedCharacter.GetComponent<PlayerStatus>());
    }

    /// <summary>
    /// Xuất icon của nhân vật trong trận.
    /// Trả về null nếu ô đó không có nhân vật hành động.
    /// </summary>
    /// <param name="slot"></param>
    public void UpdatePlaceHolderSlot(PlayerStatus slot)
    {
        if (slot != null)
        {
            floatingHealthBar.SetMaxValue(slot.maxHP);
            floatingShieldBar.SetMaxValue(slot.maxHP);
            floatingShieldBar.SetValue(0);
            characterIcon.color = Color.white;
            characterIcon.sprite = slot.gameObject.GetComponent<CharacterBase>().CharacterIcon;
            SetTransparent(border, 1f);
            SetTransparent(status, 1f);
        }
        else ClearSlot();
    }

    /// <summary>
    /// Làm trống ô.
    /// </summary>
    public void ClearSlot()
    {
        characterIcon.sprite = null;
        characterIcon.color = Color.clear;
        SetTransparent(border, 0f);
        SetTransparent(status, 0f);
        floatingHealthBar.SetMaxValue(0);
        floatingShieldBar.SetMaxValue(0);
    }

    private void SetTransparent(Image image, float offset)
    {
        var color = image.color;
        color.a = offset;
        image.color = color;
    }
}
