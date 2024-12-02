using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FragmentDetail_UI : MonoBehaviour
{
    #region Defines
    /// <summary>
    /// Nhân vật đang được chọn.
    /// </summary>
    [SerializeField] private CharacterDetail_UI characterDetail;

    [SerializeField] private TextMeshProUGUI frName;
    [SerializeField] private TextMeshProUGUI mainStat;
    [SerializeField] private TextMeshProUGUI mainStatValue;
    [SerializeField] private List<TextMeshProUGUI> subStat;
    [SerializeField] private List<TextMeshProUGUI> subStatValue;
    [SerializeField] private TextMeshProUGUI fragmentSetEffectName;
    [SerializeField] private TextMeshProUGUI fragmentSetEffectDesc;

    /// <summary>
    /// Fragment đang được chọn
    /// </summary>
    [SerializeField] public Fragments fr;

    [SerializeField] private Button equipBtn;
    [SerializeField] private Button removeBtn;
    [SerializeField] private Button upgradeBtn;
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        equipBtn?.onClick.AddListener(OnEquipFragment);
        removeBtn?.onClick.AddListener(OnEquipFragment);
    }

    private void Update()
    {
        UpdateInfo(fr);

        SwitchButton();
    }
    #endregion

    #region Methods
    public void Init(Fragments info)
    {
        ClearSlot();
        fr = info;
        UpdateInfo(fr);
    }

    private void UpdateInfo(Fragments fragment)
    {
        if (fragment.fragmentsID != "")
        {
            FragmentSetEffect effect = fragment.fragmentSetEffect;
            frName.text = fragment.fragmentsName;
            mainStat.text = fragment.mainStat.stats.ToString();
            FragmentStatValue(fragment.mainStat.statValue);
            fragmentSetEffectName.text = fragment.fragmentSet.ToString();
            //effect.BuffedStat(fragment);

            //fragmentSetEffectName.text = effect.effectName;
            //effect.Description(fragment, fragmentSetEffectDesc);

            for (int i = 0; i < fragment.subStats.Count; i++)
            {
                if (fragment.subStats[i] != null)
                {
                    subStat[i].text = fragment.subStats[i].stats.ToString();
                    
                    if (fragment.subStats[i].statValue < 1)
                    {
                        subStatValue[i].text = (fragment.subStats[i].statValue * 100).ToString() + "%";
                    }
                    else subStatValue[i].SetText("{0}", (float)fragment.subStats[i].statValue);                   
                }
            }
        }
    }

    public void ClearSlot()
    {
        fr = null;

        fragmentSetEffectName.text = "";
        fragmentSetEffectDesc.text = "";

        for (int i = 0; i < subStat.Count; i++)
        {
            subStat[i].text = "";
            subStatValue[i].text = "";
        }
    }

    void OnEquipFragment()
    {
        PlayerEquipments characterEquip = characterDetail.Character.GetComponent<PlayerEquipments>();
        Characters character = characterDetail.Character.GetComponent<Characters>();
        FragmentType type = fr.fragmentType;
        Fragments playerFragment = characterEquip.GetFragmentWithType(type);

        // Nếu fragment chưa được trang bị cho nhân vật.
        if (fr.playerUsedID == "")
        {
            // Nếu nhân vật chưa trang bị fragment, trang bị nó.
            if (playerFragment.playerUsedID == "" || playerFragment.playerUsedID != character.ID)
            {
                fr.playerUsedID = character.ID;

                characterEquip.SetFragmentWithType(type, fr);
            }
            else
            {
                playerFragment.playerUsedID = "";

                fr.playerUsedID = character.ID;

                characterEquip.SetFragmentWithType(type, fr);

            }
        }
        else
        {
            //Nếu crystal được chọn đang được trang bị bởi nhân vật khác với nhân vật được chọn.
            // Lấy thông tin nhân vật
            var anotherCharacterEquiped = TeamManager.GetInstance().obtainedCharacters
                .Find(x => x.GetComponent<Characters>().ID == fr.playerUsedID);

            //Nếu nhân vật đang được chọn chưa trang bị crystal, trang bị nó. Còn người trang bị trước đó sẽ trả về null.
            if (playerFragment.playerUsedID == "" || playerFragment.playerUsedID != character.ID)
            {
                anotherCharacterEquiped.GetComponent<PlayerEquipments>().RemoveFragment(fr);
                fr.playerUsedID = character.ID;

                characterEquip.SetFragmentWithType(type, fr);
            }
            else
            {
                //Crystal đang được nhân vật hiện tại trang bị, gỡ trang bị crystal
                if (character.ID == fr.playerUsedID)
                {
                    fr.playerUsedID = "";
                    characterEquip.RemoveFragment(fr);
                }
                else
                {
                    //Nếu nhân vật được chọn đang trang bị crystal khác, swap crystal.
                   var fragmentEquiped = TeamManager.GetInstance().obtainedFragments
                       .Find(c => c.fragmentsID == playerFragment.fragmentsID);

                    fragmentEquiped.playerUsedID = anotherCharacterEquiped.GetComponent<Characters>().ID;

                    anotherCharacterEquiped.GetComponent<PlayerEquipments>().SetFragmentWithType(type, fragmentEquiped);

                    fr.playerUsedID = character.ID;

                    characterEquip.SetFragmentWithType(type, fr);
                }
            }
        }
    }

    /// <summary>
    /// Chuyển đổi qua lại giữa nút Equip và Remove
    /// </summary>
    void SwitchButton()
    {
        if (fr.playerUsedID != "")
        {
            if (characterDetail.Character.GetComponent<Characters>().ID == fr.playerUsedID)
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

    void FragmentStatValue(double value)
    {
        if (value > 1)
        {
            mainStatValue.SetText(value.ToString());
        }
        else
        {
            mainStatValue.text = (value * 100).ToString() + "%";
        }
    }
    #endregion
}
