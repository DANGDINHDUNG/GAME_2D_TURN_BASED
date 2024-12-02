using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// Hiển thị thông tin chi tiết nhân vật được chọn.
/// </summary>
public class CharacterDetail_UI : MonoBehaviour
{
    #region Defines
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI element;
    [SerializeField] private TextMeshProUGUI characterClass;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI defend;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI crRate;
    [SerializeField] private TextMeshProUGUI crDMG;
    [SerializeField] private TextMeshProUGUI mastery;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image elementIcon;
    [SerializeField] private Image crystal;
    [SerializeField] private TextMeshProUGUI crName;
    [SerializeField] private GameObject upgradeBtn;
    [SerializeField] private Sprite addImage;

    private GameObject c;

    /// <summary>
    /// Nhân vật hiện tại.
    /// </summary>
    public GameObject Character => c;
    #endregion

    #region Core MonoBehaviours
    private void Update()
    {
        UpdateInfo(c);
    }
    #endregion

    #region Methods
    public void Init(GameObject info)
    {
        c = info;
        UpdateInfo(info);
    }

    private void UpdateInfo(GameObject character)
    {
        PlayerBase playerBase = character.GetComponent<PlayerBase>();
        PlayerDetails playerDetails = character.GetComponent<PlayerDetails>();
        PlayerEquipments playerEquipments = character.GetComponent<PlayerEquipments>();

        playerDetails.Initialized();

        elementIcon.sprite = Database.Instance.UpdateElementIcons(playerBase.DamageElement);
        characterName.text = playerBase.Name;
        characterSprite.sprite = character.GetComponentInChildren<SpriteRenderer>().sprite;
        element.text = playerBase.DamageElement.ToString();
        characterClass.text = playerBase.CharacterRole.ToString();
        level.text = playerBase.Level.ToString();
        
        hp.text = playerDetails.characterHP.ToString();
        attack.text = playerDetails.characterAttack.ToString();
        defend.text = playerDetails.characterDefend.ToString();
        speed.text = playerDetails.characterSpeed.ToString();
        crRate.text = playerDetails.characterCrRate.ToString();
        crDMG.text = playerDetails.characterCrDMG.ToString();
        mastery.text = playerDetails.characterMastery.ToString();

        RectTransform rt = crystal.GetComponent<RectTransform>();
        if (playerEquipments != null && playerEquipments.crystal.PlayerUsedID != "" && playerEquipments.crystal.PlayerUsedID == character.GetComponent<Characters>().ID)
        {
            //upgradeBtn.SetActive(true);
            crystal.sprite = playerEquipments.crystal.crystalSprite;
            rt.sizeDelta = new Vector2(130f, 130f);
            crName.text = playerEquipments.crystal.crystalName.ToString();
        }
        else
        {
            //upgradeBtn.SetActive(false);
            crystal.sprite = addImage;
            rt.sizeDelta = new Vector2(100f, 100f);
            crName.text = "";
        }
    }
    #endregion 
}
