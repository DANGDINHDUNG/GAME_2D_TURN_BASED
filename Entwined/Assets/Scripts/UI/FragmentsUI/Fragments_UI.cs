using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fragments_UI : MonoBehaviour
{
    [SerializeField] private Image fragmentSprite;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Image characterUsed;
    [SerializeField] private Image rarityBorder;
    private Button button;
    private Fragments fragmentDetail;
    public GameObject ParentDisplay { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnDetailDisplay);
        ParentDisplay = GameObject.FindGameObjectWithTag("Fragment");

    }

    public void Init(Fragments fragment)
    {
        fragmentDetail = fragment;
        UpdateUISlot(fragment);
    }

    private void Update()
    {
        if (!UIManager.instance.IsCharacterManager)
        {
            ClearSlot();
        }

        UpdateUISlot(fragmentDetail);
    }

    /// <summary>
    /// Hiển thị hình ảnh nhân vật đang được sở hữu
    /// </summary>
    /// <param name="character"></param>
    public void UpdateUISlot(Fragments fragment)
    {
        if (fragment != null)
        {
            fragmentSprite.sprite = fragment.fragmentSprite;
            level.text = fragment.fragmentLevel.ToString();

            ShowFragmentRarity(fragment);
            GetCharacterUsedIcon(fragment);
        }
        else ClearSlot();
    }

    /// <summary>
    /// Hiển thị độ hiếm của fragment theo màu.
    /// </summary>
    /// <param name="fragment"></param>
    void ShowFragmentRarity(Fragments fragment)
    {
        switch (fragment.fragmentRarity)
        {
            case Rarity.Mythic:
                rarityBorder.color = Color.yellow;
                break;
            case Rarity.Legend:
                rarityBorder.color = new Color(1f, 0.3f, 1f);
                break;
            case Rarity.Epic:
                rarityBorder.color = new Color(0.17f, 0.64f, 0.8f);
                break;
        }
    }

    /// <summary>
    /// Làm trống ô.
    /// </summary>
    public void ClearSlot()
    {
        fragmentSprite.sprite = null;
        level.text = null;
        characterUsed.sprite = null;
    }

    /// <summary>
    /// Nếu crystal đang được trang bị, hiển thị avatar người trang bị.
    /// </summary>
    /// <param name="fragment"></param>
    void GetCharacterUsedIcon(Fragments fragment)
    {
        if (fragment.playerUsedID == "")
        {
            characterUsed.color = new Color(characterUsed.color.r, characterUsed.color.g, characterUsed.color.b, 0f); // Vô hình
        }
        else
        {
            // Tìm nhân vật với ID khớp
            var character = TeamManager.GetInstance().obtainedCharacters
                .Find(c => c.GetComponent<Characters>().ID == fragment.playerUsedID);

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
        ParentDisplay.GetComponent<FragmentDetail_UI>().Init(fragmentDetail);
    }
}
