using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;


public class InventoryManager : MonoBehaviour
{
    #region Defines
    [SerializeField] public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    [SerializeField] public int gold;
    public static InventoryManager instance;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Thêm material vào inventory.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    public void AddToInventory(MaterialsSO itemToAdd, int amountToAdd)
    {
        // Nếu chưa tồn tại material trong inventory, thêm mới.
        if (!ContainsItem(itemToAdd, inventorySlots)) // Check whether item exists in inventory
        {
            inventorySlots.Add(new InventorySlot(itemToAdd, amountToAdd));
        }
        else        // Nếu material tồn tại trong inventory, thêm số lượng.
        {
            var slot = inventorySlots.Find(s => s.MaterialData == itemToAdd);
            slot.AddToStack(amountToAdd);
        }
    }

    /// <summary>
    /// Kiểm tra material đã tồn tại trong inventory chưa.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="invSlot"></param>
    /// <returns></returns>
    public bool ContainsItem(MaterialsSO itemToAdd, List<InventorySlot> invSlot)
    {
        var slot = invSlot.Find(i => i.MaterialData == itemToAdd);

        return slot == null ? false : true;

    }

    #endregion
}
