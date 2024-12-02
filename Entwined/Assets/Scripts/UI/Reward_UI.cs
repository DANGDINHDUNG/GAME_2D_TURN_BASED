using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward_UI : MonoBehaviour
{
    #region Defines
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI amount;
    #endregion

    #region Methods
    public void Init(InventorySlot slot)
    {
        UpdateUISlot(slot);
    }

    public void Init(Fragments frag)
    {
        UpdateUISlot(frag);
    }

    void UpdateUISlot(InventorySlot slot)
    {
        rewardImage.sprite = slot.MaterialData.materialIcon;
        amount.text = slot.StackSize.ToString();
    }

    void UpdateUISlot(Fragments frag)
    {
        rewardImage.sprite = frag.fragmentSprite;
        amount.text = "";
    }
    #endregion
}
