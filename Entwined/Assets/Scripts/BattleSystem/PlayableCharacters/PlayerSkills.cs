using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{ 
    #region Defines
    [Header("Skill Button")]
    [SerializeField] private List<Skills> characterSkills;   // Tấn công hiện tại của nhân vật trong trận.
    [SerializeField] private Button normalAttackBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Image normalAttackIcon;
    [SerializeField] private Image skillIcon;

    [SerializeField] private GameObject skillPlaceHolder;     // Ô kĩ năng.
    #endregion

    #region Properties
    public List<Skills> CharacterSkills => characterSkills;
    public Button NormalAttackBtn => normalAttackBtn;
    public Button SkillBtn => skillBtn;
    public Image NormalAttackIcon => normalAttackIcon;
    public Image SkillIcon => skillIcon;
    public GameObject SkillPlaceHolder => skillPlaceHolder;
    #endregion

    #region Core MonoBehaviour
    private void Update()
    {
        if (this.gameObject == TeamManager.GetInstance().characterInAction && FieldManager.Instance.IsPlayerTurn)
        {
            skillPlaceHolder.SetActive(true);       // Chỉ hiện thị ô kĩ năng khi nhân vật đang hành động.
        }
        else
        {
            skillPlaceHolder.SetActive(false);
        }
        SetupIconSkill();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Set up giao diện nút kĩ năng.
    /// </summary>
    private void SetupIconSkill()
    {
        if (characterSkills.Count < 2)
        {
            return;
        }
        normalAttackIcon.sprite = characterSkills[0].SkillIcon;
        skillIcon.sprite = characterSkills[1].SkillIcon;
    }

    #endregion
}
