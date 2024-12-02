using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList_UI : MonoBehaviour
{
    [SerializeField] private Image characterAvatar;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Image characterElement;
    private GameObject characterDetail;
    private Button button;
    public Transform ParentDisplay { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        if (UIManager.instance.IsCharacterManager)
        {
            button?.onClick.AddListener(OnDetailDisplay);
        }
        else
        {
            button?.onClick.AddListener(OnAddTeam);
        }
        ParentDisplay = GameObject.FindGameObjectWithTag("Info").GetComponent<Transform>();
    }

    public void Init(GameObject character)
    {
        characterDetail = character;
        UpdateUISlot(character.GetComponent<PlayerBase>());
    }

    private void Update()
    {
        // Nếu không ở trong trận sẽ ẩn action bar.
        if (!UIManager.instance.IsCharacterManager && !UIManager.instance.IsTeamSetup)
        {
            ClearSlot();
        }

        UpdateUISlot(characterDetail.GetComponent<PlayerBase>());
    }

    /// <summary>
    /// Hiển thị hình ảnh nhân vật đang được sở hữu
    /// </summary>
    /// <param name="character"></param>
    public void UpdateUISlot(PlayerBase character)
    {
        if (character != null)
        {
            characterAvatar.color = Color.white;
            characterAvatar.sprite = character.CharacterListAvatar;
            characterName.text = character.Name;   
            level.text = character.Level.ToString();   
            characterElement.sprite = Database.Instance.UpdateElementIcons(character.DamageElement);
        }
        else ClearSlot();
    }

    /// <summary>
    /// Làm trống ô.
    /// </summary>
    public void ClearSlot()
    {
        characterAvatar.sprite = null;
        characterName.text = null;
        characterAvatar.color = Color.clear;
    }

    /// <summary>
    /// Thực hiện mở danh sách thông tin chi tiết của một nhân vật khi ấn vào nhân vật đó.
    /// </summary>
    /// <param name="eventData"></param>
    void OnDetailDisplay()
    {
        Transform child = ParentDisplay.Find("Details");
        child.gameObject.SetActive(true);

        child.GetComponent<CharacterDetail_UI>().Init(characterDetail);
    }

    /// <summary>
    /// Thêm nhân vật vào đội.
    /// </summary>
    void OnAddTeam()
    {
        bool isAdd = false;
        foreach (var slot in TeamSetUp.instance.slots)
        {
            if (slot.characterInSlot == null)
            {
                if (!isAdd)
                {
                    slot.characterInSlot = characterDetail;
                    isAdd = true;
                }
            }
        }
    }
}
