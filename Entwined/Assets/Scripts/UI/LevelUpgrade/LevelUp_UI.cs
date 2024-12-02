using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Events;

public class LevelUp_UI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    #region Defines
    [SerializeField] private Image materialIcon;
    [SerializeField] private int value;
    [SerializeField] private int remainAmount;
    [SerializeField] private TextMeshProUGUI amount;
    private MaterialsSO m;
    private InventorySlot invSlot;

    /// <summary>
    /// Thời gian ấn giữ nút.
    /// </summary>
    float holdTimer = 0f;
    bool isHolding;

    public static UnityAction<LevelUp_UI> OnSelectMaterial;
    #endregion

    #region Core MonoBehaviours

    private void Update()
    {
        UpdateMaterial(m);

        CheckHolding();
    }
    #endregion

    #region Methods
    public void Init(MaterialsSO material)
    {
        m = material;
        value = 0;
        UpdateMaterial(material);
    }

    public void Init(MaterialsSO material, int amount)
    {
        m = material;
        value = amount;
        UpdateMaterial(material);
    }

    void UpdateMaterial(MaterialsSO material)
    {
        invSlot = InventoryManager.instance.inventorySlots.Find(m => m.MaterialData == material);
        materialIcon.sprite = material.materialIcon;
        if (invSlot != null)
        {
            remainAmount = invSlot.StackSize;
            amount.SetText("{0} / {1}", value, invSlot.StackSize);
        }
        else amount.SetText("{0} / 0", value);
    }

    /// <summary>
    /// Khi ấn vào sẽ lấy 1 material ra làm exp.
    /// </summary>
    void SelectMaterial()
    {
        isHolding = true;
        if (invSlot != null)
        {
            if (value < invSlot.StackSize)
            {
                OnSelectMaterial.Invoke(this);
            }
        }
    }

    /// <summary>
    /// Kiểm tra điều kiện level để khi tăng cấp không vượt quá level tối đa.
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <param name="maxLevel"></param>
    public void CheckLevelCondition(int currentLevel, int maxLevel)
    {
        if (currentLevel < maxLevel)
        {
            value++;
        }
    }

    /// <summary>
    /// Khi nút được giữ, tự động cộng liên tục số lượng amount;
    /// </summary>
    void CheckHolding()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > 1f)
            {
                SelectMaterial();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectMaterial();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0f;
    }

    /// <summary>
    /// Trả về tổng lượng exp đã chọn.
    /// </summary>
    /// <returns></returns>
    public long GetExp()
    {
        return m.expValue;
    }

    /// <summary>
    /// Kiểm tra nếu đủ nguyên liệu để đột phá.
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughMaterial()
    {
        if (invSlot != null) return value <= remainAmount ? true : false;
        else return false;
    }

    /// <summary>
    /// Trừ đi số lượng stack đã sử dụng khi nâng cấp.
    /// </summary>
    public void RemoveFromStack()
    {
        if (invSlot != null)
        {
            invSlot.RemoveFromStack(value);
            value = 0;
        }
    }

    void ClearSlot()
    {
        materialIcon.sprite = null;
        amount.text = "";
        value = 0;
    }

    private void OnDisable()
    {
        ClearSlot();
    }
    #endregion
}
