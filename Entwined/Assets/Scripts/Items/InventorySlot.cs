using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[System.Serializable]
public class InventorySlot : ItemSlot
{

    public InventorySlot(MaterialsSO materials, int size)
    {
        materialData = materials;
        stackSize = size;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void UpdateInventorySlot(MaterialsSO materials, int size)
    {
        materialData = materials;
        stackSize = size;
    }
}
