using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skills_UI : MonoBehaviour
{
    #region Defines
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillLevel;
    [SerializeField] private Skills skillDetail;
    private Button button;
    private Transform ParentDisplay;

    #endregion

    #region Mono Behaviours
    private void Awake()
    {
        skillIcon = GetComponent<Image>();
        skillLevel = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnShowDetail);
        ParentDisplay = GameObject.FindGameObjectWithTag("Info").GetComponent<Transform>();
    }

    private void Update()
    {
        UpdateSkill(skillDetail);
    }
    #endregion

    #region Methods
    public void Init(Skills skill)
    {
        skillDetail = skill;
        UpdateSkill(skill);
    }

    private void UpdateSkill(Skills skill)
    {
        skillIcon.sprite = skill.SkillIcon;
        skillLevel.SetText("<color=#00FF00>{0}</color> / 10", skill.skillLevel);
    }

    void OnShowDetail()
    {
        Transform child = ParentDisplay.Find("SkillDetail");
        child.gameObject.SetActive(true);

        child.GetComponent<Skills_Detail>().Init(skillDetail);
    }
    #endregion
}
