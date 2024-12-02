using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skills_Detail : MonoBehaviour
{
    #region Defines
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillLevel;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillPredictDescription;

    public Skills skillDetail;
    #endregion

    #region Mono Behaviours
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

    void UpdateSkill(Skills skills)
    {
        skillName.text = skills.skillName;
        skillLevel.SetText("Lv: <color=#00FF00>{0}</color> / 10", skills.skillLevel);
        skills.Description(skillDescription, skills.skillLevel);
        if (skills.skillLevel < 10)
        {
            skills.Description(skillPredictDescription, skills.skillLevel + 1);
        }
        else skillPredictDescription.SetText("Max Level");
    }
    #endregion
}
