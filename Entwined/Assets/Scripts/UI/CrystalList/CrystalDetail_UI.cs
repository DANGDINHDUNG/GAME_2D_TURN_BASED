using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hiển thị thông tin của crystal trong crystal list
/// </summary>
public class CrystalDetail_UI : MonoBehaviour
{
    #region Defines
    /// <summary>
    /// Nhân vật đang được chọn.
    /// </summary>
    [SerializeField] private CharacterDetail_UI characterDetail;

    [SerializeField] private TextMeshProUGUI crName;
    [SerializeField] private TextMeshProUGUI hpPlus;
    [SerializeField] private TextMeshProUGUI atkPlus;
    [SerializeField] private TextMeshProUGUI defPlus;
    [SerializeField] private TextMeshProUGUI crystalEffName;
    [SerializeField] private TextMeshProUGUI crystalEffDesc;

    /// <summary>
    /// Crystal đang được chọn
    /// </summary>
    [SerializeField] public MemoryCrystals c;

    [SerializeField] private Button equipBtn;
    [SerializeField] private Button removeBtn;
    [SerializeField] private Button upgradeBtn;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        equipBtn?.onClick.AddListener(OnEquipCrystal);
        removeBtn?.onClick.AddListener(OnEquipCrystal);
    }

    private void Update()
    {
        UpdateInfo(c);

        SwitchButton();
    }
    #endregion

    #region Methods
    public void Init(MemoryCrystals info)
    {
        c = info;
        UpdateInfo(c);
    }

    private void UpdateInfo(MemoryCrystals memoryCrystals)
    {
        if (memoryCrystals.crystalID != "")
        {
            CrystalEffects crystalEffects = memoryCrystals.crystalEffect;

            crystalEffects.BuffedStat(memoryCrystals);

            crName.text = memoryCrystals.crystalName;
            hpPlus.text = memoryCrystals.buffedHp.ToString();
            atkPlus.text = memoryCrystals.buffedAttack.ToString();
            defPlus.text = memoryCrystals.buffedDefend.ToString();

            crystalEffName.text = crystalEffects.effectName;
            crystalEffects.Description(memoryCrystals, crystalEffDesc);
        }
    }

    public void ClearSlot()
    {
        c = null;

        crName.text = "";
        hpPlus.text = "";
        atkPlus.text = "";
        defPlus.text = "";

        crystalEffName.text = "";
        crystalEffDesc.text = "";
    }

    void OnEquipCrystal()
    {
        PlayerEquipments characterEquip = characterDetail.Character.GetComponent<PlayerEquipments>();
        Characters character = characterDetail.Character.GetComponent<Characters>();


        // Nếu crystal chưa được trang bị cho nhân vật.
        if (c.PlayerUsedID == "")
        {
            // Nếu nhân vật chưa trang bị crystal, trang bị nó.
            if (characterEquip.crystal.PlayerUsedID == "" )
            {
                c.PlayerUsedID = character.ID;

                characterEquip.crystal = c;
            }
            else
            {
                var crystalEquiped = TeamManager.GetInstance().obtainedCrystals
                    .Find(c => c.crystalID == characterEquip.crystal.crystalID);

                crystalEquiped.PlayerUsedID = "";

                c.PlayerUsedID = character.ID;

                characterEquip.crystal = c;
            }
        }
        else
        {
            // Nếu crystal được chọn đang được trang bị bởi nhân vật khác với nhân vật được chọn.
            // Lấy thông tin nhân vật
            var anotherCharacterEquiped = TeamManager.GetInstance().obtainedCharacters
                .Find(x => x.GetComponent<Characters>().ID == c.PlayerUsedID);

            // Nếu nhân vật đang được chọn chưa trang bị crystal, trang bị nó. Còn người trang bị trước đó sẽ trả về null.
            if (characterEquip.crystal.PlayerUsedID == "")
            {
                c.PlayerUsedID = character.ID;

                characterEquip.crystal = c;

                //anotherCharacterEquiped.GetComponent<PlayerEquipments>().crystal.crystalID = "";
            }
            else
            {
                // Crystal đang được nhân vật hiện tại trang bị, gỡ trang bị crystal
                if (character.ID == c.PlayerUsedID)
                {
                    //characterEquip.crystal.crystalID = "";

                    c.PlayerUsedID = "";
                }
                else
                {
                    // Nếu nhân vật được chọn đang trang bị crystal khác, swap crystal.
                    var crystalEquiped = TeamManager.GetInstance().obtainedCrystals
                        .Find(c => c.crystalID == characterEquip.crystal.crystalID);

                    crystalEquiped.PlayerUsedID = anotherCharacterEquiped.GetComponent<Characters>().ID;

                    anotherCharacterEquiped.GetComponent<PlayerEquipments>().crystal = crystalEquiped;

                    c.PlayerUsedID = character.ID;

                    characterEquip.crystal = c;
                }
            }
        }
    }

    /// <summary>
    /// Chuyển đổi qua lại giữa nút Equip và Remove
    /// </summary>
    void SwitchButton()
    {
        if (c.PlayerUsedID != "")
        {
            if (characterDetail.Character.GetComponent<Characters>().ID == c.PlayerUsedID)
            {
                equipBtn.gameObject.SetActive(false);
                removeBtn.gameObject.SetActive(true);
            }
            else
            {
                equipBtn.gameObject.SetActive(true);
                removeBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            equipBtn.gameObject.SetActive(true);
            removeBtn.gameObject.SetActive(false);
        }
    }
    #endregion
}
