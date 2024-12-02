using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGeneration : MonoBehaviour
{
    [SerializeField] private Button testBtn;
    [SerializeField] private List<InventorySlot> materials;

    public void GenerateFragment()
    {
        Fragments newFragment = Fragments.GenerateNewFragment();
        TeamManager.GetInstance().obtainedFragments.Add(newFragment);    
    }

    public void AddMaterial()
    {
        foreach(var slot in materials)
        {
            InventoryManager.instance.AddToInventory(slot.MaterialData, slot.StackSize);
        }
    }
}
