using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot
{
    #region Defines
    /// <summary>
    /// Thông tin về material
    /// </summary>
    [SerializeField] protected MaterialsSO materialData;

    /// <summary>
    /// Số lượng của materials.
    /// </summary>
    [SerializeField] protected int stackSize;
    #endregion

    #region Properties
    public MaterialsSO MaterialData => materialData;
    public int StackSize => stackSize;
    #endregion

    #region Methods

    public void ClearSlot()
    {
        stackSize = 0;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        if (stackSize < 1) ClearSlot();
    }
    #endregion
}
