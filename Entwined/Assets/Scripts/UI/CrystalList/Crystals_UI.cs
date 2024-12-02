using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crystals_UI : MonoBehaviour
{
    [SerializeField] private Image crystalSprite;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Image characterUsed;
    [SerializeField] private string playerID;
    private Button button;
    private MemoryCrystals crystalDetail;
    public GameObject ParentDisplay { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnDetailDisplay);
        ParentDisplay = GameObject.FindGameObjectWithTag("Crystal");

    }

    public void Init(MemoryCrystals crystal)
    {
        crystalDetail = crystal;
        UpdateUISlot(crystal);
    }

    private void Update()
    {
        // Nếu không ở trong trận sẽ ẩn action bar.
        if (!UIManager.instance.IsCharacterManager)
        {
            ClearSlot();
        }

        UpdateUISlot(crystalDetail);
    }

    /// <summary>
    /// Hiển thị hình ảnh nhân vật đang được sở hữu
    /// </summary>
    /// <param name="character"></param>
    public void UpdateUISlot(MemoryCrystals crystal)
    {
        if (crystal != null)
        {
            crystalSprite.sprite = crystal.crystalSprite;
            level.text = crystal.crystalLevel.ToString();
            rank.text = crystal.refinementRank.ToString();
            playerID = crystal.PlayerUsedID;

            GetCharacterUsedIcon(crystal);
        }
        else ClearSlot();
    }

    /// <summary>
    /// Làm trống ô.
    /// </summary>
    public void ClearSlot()
    {
        crystalSprite.sprite = null;
        level.text = null;
        rank.text = null;
        characterUsed.sprite = null;
    }

    /// <summary>
    /// Nếu crystal đang được trang bị, hiển thị avatar người trang bị.
    /// </summary>
    /// <param name="memCry"></param>
    void GetCharacterUsedIcon(MemoryCrystals memCry)
    {
        if (memCry.PlayerUsedID == "")
        {
            characterUsed.color = new Color(characterUsed.color.r, characterUsed.color.g, characterUsed.color.b, 0f); // Vô hình
        }
        else
        {
            // Tìm nhân vật với ID khớp
            var character = TeamManager.GetInstance().obtainedCharacters
                .Find(c => c.GetComponent<Characters>().ID == memCry.PlayerUsedID);

            if (character != null)
            {
                characterUsed.sprite = character.GetComponent<CharacterBase>().CharacterIcon;
                characterUsed.color = new Color(characterUsed.color.r, characterUsed.color.g, characterUsed.color.b, 1f); // Hiển thị
            }
        }
    }

    /// <summary>
    /// Thực hiện mở danh sách thông tin chi tiết của một nhân vật khi ấn vào nhân vật đó.
    /// </summary>
    /// <param name="eventData"></param>
    void OnDetailDisplay()
    {
        ParentDisplay.GetComponent<CrystalDetail_UI>().Init(crystalDetail);
    }
}
