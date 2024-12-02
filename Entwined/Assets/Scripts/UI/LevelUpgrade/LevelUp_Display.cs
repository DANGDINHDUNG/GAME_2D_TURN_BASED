using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class LevelUp_Display : MonoBehaviour
{
    #region Defines
    [SerializeField] private List<LevelUp_UI> levelUp_UIs;
    [SerializeField] private LevelUp_UI BT_levelUp_UI;
    [SerializeField] private Slider expProgress;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI costTxt;
    [SerializeField] private TextMeshProUGUI expPointTxt;

    [SerializeField] private GameObject LevelUpIngredients;
    [SerializeField] private GameObject BreakthroughIngredients;

    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button breakthroughBtn;
    [SerializeField] private Button autoFillBtn;

    private static BreakthroughIngredientsDatas breakthroughData = new BreakthroughIngredientsDatas();
    public static SkillLevelUpDatas skillLevelUpData = new SkillLevelUpDatas();
    GameObject go_Upgrade;
    UpgradeType upgradeType;
    [SerializeField] MaterialsSO requireMaterial;

    /// <summary>
    /// Tổng số exp được sử dụng.
    /// </summary>
    long totalExp = 0;

    /// <summary>
    /// Số cấp được tăng lên.
    /// </summary>
    [SerializeField] int nextLevel = 0;
    long cost = 0;
    /// <summary>
    /// Số lượng exp còn thừa khi đã nâng cấp.
    /// </summary>
    [SerializeField] long remainExp = 0;

    /// <summary>
    /// Lượng kinh nghiệm cần để tăng cấp kế tiếp
    /// </summary>
    [SerializeField] long maxExpRequired = 0;

    int currentLevel = 0;
    int maxLevel = 0;

    PlayerBase playerBase;
    MemoryCrystals memoryCrystal;
    Fragments fragment;
    Skills skill;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        LevelUp_UI.OnSelectMaterial += ExpCalculation;
    }

    private void OnDisable()
    {
        LevelUp_UI.OnSelectMaterial -= ExpCalculation;
        ClearData();
        maxExpRequired = 0;
    }
    #endregion

    #region Core MonoBehaviour
    private void Awake()
    {
        upgradeBtn.onClick.AddListener(UpgradeLevel);
        breakthroughBtn.onClick.AddListener(BreakLevel);
    }

    private void Update()
    {
        PredictUpgradeLevel();
    }
    #endregion

    #region Methods

    public void UpdateIngredients(List<MaterialsSO> materials)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            levelUp_UIs[i].Init(materials[i]);
        }
    }

    public void GetUpgradeInfo(GameObject upgradeGameObject, UpgradeType type)
    {
        go_Upgrade = upgradeGameObject;
        upgradeType = type;
        switch (type)
        {
            case UpgradeType.CharacterUpgrade:
                playerBase = upgradeGameObject.GetComponent<CharacterDetail_UI>().Character.GetComponent<PlayerBase>();
                currentLevel = playerBase.Level;
                maxLevel = currentLevel + 1;
                maxExpRequired = playerBase.SetLevelProgress(currentLevel);
                SetMaxProgress(maxExpRequired);
                SetCurrentProgress(playerBase.CurrentLevelPoint);
                break;
            case UpgradeType.CrystalUpgrade:
                memoryCrystal = upgradeGameObject.GetComponent<CrystalDetail_UI>().c;
                currentLevel = memoryCrystal.crystalLevel;
                maxLevel = memoryCrystal.maxLevelBreakThrough;
                maxExpRequired = memoryCrystal.GetMaxExpPoint(currentLevel);
                SetMaxProgress(maxExpRequired);
                SetCurrentProgress(memoryCrystal.currentExpPoint);
                break;
            case UpgradeType.FragmentUpgrade:
                fragment = upgradeGameObject.GetComponent<FragmentDetail_UI>().fr;
                currentLevel = fragment.fragmentLevel;
                maxLevel = fragment.fragmentMaxLevel;
                maxExpRequired = fragment.GetExpPerLevel(currentLevel, fragment.fragmentRarity);
                SetMaxProgress(maxExpRequired);
                SetCurrentProgress(fragment.currentExp);
                break;
            case UpgradeType.SkillUpgrade:
                skill = upgradeGameObject.GetComponent<Skills_Detail>().skillDetail;
                playerBase = GameObject.Find("Details").GetComponent<CharacterDetail_UI>().Character.GetComponent<PlayerBase>();
                BreakthroughIngredients.SetActive(true);
                LevelUpIngredients.SetActive(false);
                currentLevel = skill.skillLevel;
                break;
        }
    }

    /// <summary>
    /// Dựa vào số lượng nguyên liệu được chọn, dự đoán số cấp được tăng lên.
    /// </summary>
    void PredictUpgradeLevel()
    {
        switch (upgradeType)
        {
            case UpgradeType.CharacterUpgrade:
                currentLevel = playerBase.Level;
                SetCurrentProgress(playerBase.CurrentLevelPoint + totalExp);
                cost = Mathf.RoundToInt(totalExp/10);
                maxLevel = currentLevel + nextLevel + 1;
                SetLevelInfo(currentLevel, playerBase.SetLevelProgress(currentLevel), cost);
                CheckLevelUp(playerBase.CurrentLevelPoint, playerBase.SetLevelProgress(currentLevel + nextLevel));
                break;
            case UpgradeType.CrystalUpgrade:
                currentLevel = memoryCrystal.crystalLevel;
                SetCurrentProgress(memoryCrystal.currentExpPoint + totalExp);
                cost = Mathf.RoundToInt(totalExp / 2);
                CheckLevelUp(memoryCrystal.currentExpPoint, memoryCrystal.GetMaxExpPoint(currentLevel + nextLevel));

                // Lấy thông tin nguyên liệu dùng để đột phá ứng với cấp đột phá hiện tại.
                if (currentLevel >= maxLevel && maxLevel <= 90)
                {
                    requireMaterial = Database.Instance.materials.FirstOrDefault(m =>
                        m.materialClass == memoryCrystal.classRequired &&
                        m.materialRarity == breakthroughData.brDatas[maxLevel].rarity);
                    LevelUpIngredients.SetActive(false);
                    upgradeBtn.gameObject.SetActive(false);
                    BreakthroughIngredients.SetActive(true);
                    breakthroughBtn.gameObject.SetActive(true);
                    BT_levelUp_UI.Init(requireMaterial, breakthroughData.brDatas[maxLevel].amount);
                    cost = breakthroughData.brDatas[maxLevel].cost;
                }
                SetLevelInfo(currentLevel, memoryCrystal.GetMaxExpPoint(currentLevel), cost);

                break;
            case UpgradeType.FragmentUpgrade:
                currentLevel = fragment.fragmentLevel;
                SetCurrentProgress(fragment.currentExp + totalExp);
                cost = Mathf.RoundToInt(totalExp / 1.5f);
                SetLevelInfo(currentLevel, fragment.GetExpPerLevel(currentLevel, fragment.fragmentRarity), cost);
                CheckLevelUp(fragment.currentExp, fragment.GetExpPerLevel(currentLevel + nextLevel, fragment.fragmentRarity));
                break;
            case UpgradeType.SkillUpgrade:
                currentLevel = skill.skillLevel;
                maxLevel = skill.skillLevel;
                totalExp = 1;
                if (currentLevel < 10)
                {
                    requireMaterial = Database.Instance.materials.FirstOrDefault(m =>
                        m.materialClass == playerBase.CharacterRole &&
                        m.materialRarity == skillLevelUpData.skillDatas[currentLevel + 1].rarity);
                    LevelUpIngredients.SetActive(false);
                    upgradeBtn.gameObject.SetActive(true);
                    BreakthroughIngredients.SetActive(true);
                    breakthroughBtn.gameObject.SetActive(false);
                    BT_levelUp_UI.Init(requireMaterial, skillLevelUpData.skillDatas[currentLevel + 1].amount);
                    cost = skillLevelUpData.skillDatas[currentLevel + 1].cost;
                }
                SetLevelInfo(currentLevel, 0, cost);
                break;
        }

        if (currentLevel < maxLevel)
        {
            upgradeBtn.gameObject.SetActive(true);
            LevelUpIngredients.SetActive(true);
            BreakthroughIngredients.SetActive(false);
            breakthroughBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Tính toán lượng exp để tăng cấp.
    /// </summary>
    /// <param name="levelUp"></param>
    void ExpCalculation(LevelUp_UI levelUp)
    {
        levelUp.CheckLevelCondition(currentLevel + nextLevel, maxLevel);

        // Cấp tăng không vượt quá cấp tối đa.
        if (currentLevel + nextLevel < maxLevel)
        {
            totalExp += levelUp.GetExp();
            remainExp += levelUp.GetExp();
        }
    }

    /// <summary>
    /// Kiểm tra nếu nhân vật cộng đủ điểm exp sẽ tăng lên một cấp.
    /// Nếu người chơi không tăng cấp ngay mà tiếp tục công điểm exp, thì cấp sẽ cộng dồn lại.
    /// </summary>
    /// <param name="currentExp"></param>
    /// <param name="maxExpRequired"></param>
    /// <param name="value"></param>
    void CheckLevelUp(long currentExp, long nextMaxLevel)
    {
        if (totalExp > 0)
        {
            if (currentExp + totalExp >= maxExpRequired && currentLevel + nextLevel < maxLevel)
            {
                nextLevel++;
                remainExp = currentExp + totalExp - maxExpRequired;
                maxExpRequired += nextMaxLevel;
            }
        }
    }

    public void SetMaxProgress(long value)
    {
        expProgress.maxValue = value;
    }

    public void SetCurrentProgress(long value)
    {
        expProgress.value = value;
    }

    /// <summary>
    /// Hiển thị thông tin số lượng cấp sẽ tăng và số lượng exp point sử dụng.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="maxExpPoint"></param>
    void SetLevelInfo(int level, long maxExpPoint, long cost)
    {
        levelTxt.SetText("Lv: {0} + <color=#007934>{1}</color>", level, nextLevel);
        expPointTxt.SetText("<color=#6C000B>{0}</color> / {1}", totalExp, maxExpPoint);
        costTxt.SetText("Cost: {0}", cost);
    }

    void UpgradeLevel()
    {
        if (totalExp > 0)
        {
            switch (upgradeType)
            {
                case UpgradeType.CharacterUpgrade:
                    playerBase.LevelUpgrade(nextLevel, remainExp);
                    maxExpRequired = playerBase.SetLevelProgress(playerBase.Level);
                    SetMaxProgress(maxExpRequired);

                    break;
                case UpgradeType.CrystalUpgrade:
                    memoryCrystal.UpgradeLevel(nextLevel, remainExp);
                    maxExpRequired = memoryCrystal.GetMaxExpPoint(memoryCrystal.crystalLevel);
                    SetMaxProgress(maxExpRequired);
                    break;
                case UpgradeType.FragmentUpgrade:
                    fragment.LevelUp(nextLevel, remainExp);
                    maxExpRequired = fragment.GetExpPerLevel(currentLevel, fragment.fragmentRarity);
                    SetMaxProgress(maxExpRequired);
                    break;
                case UpgradeType.SkillUpgrade:
                    if (BT_levelUp_UI.IsEnoughMaterial())
                    {
                        BT_levelUp_UI.RemoveFromStack();
                        skill.LevelUp();
                    }
                    break;
            }
            SetCurrentProgress(remainExp);

            foreach (var s in levelUp_UIs)
            {
                s.RemoveFromStack();
            }

            ClearData();
        }
    }

    /// <summary>
    /// Đột phá Level.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    void BreakLevel()
    {
        if (BT_levelUp_UI.IsEnoughMaterial())
        {
            BT_levelUp_UI.RemoveFromStack();
            memoryCrystal.maxLevelBreakThrough += 10;
            maxLevel = memoryCrystal.maxLevelBreakThrough;
        }
        else Debug.Log("Not Enough material");
    }
    
    void ClearData()
    {
        nextLevel = 0;
        totalExp = 0;
        remainExp = 0;
        cost = 0;
    }
    #endregion
}
