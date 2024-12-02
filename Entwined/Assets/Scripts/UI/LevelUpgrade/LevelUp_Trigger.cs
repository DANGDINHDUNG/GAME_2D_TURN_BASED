using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp_Trigger : MonoBehaviour
{
    #region Defines
    [SerializeField] private GameObject levelUpPopUp;
    [SerializeField] private Button openBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private List<MaterialsSO> materials;

    [Header("Information")]
    [SerializeField] private GameObject upgradeGameObject;
    [SerializeField] private UpgradeType upgradeType;

    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        openBtn.onClick.AddListener(OpenPopUp);
        closeBtn.onClick.AddListener(ClosePopUp);
    }
    #endregion

    #region Methods

    void OpenPopUp()
    {
        levelUpPopUp.SetActive(true);
        levelUpPopUp.GetComponent<LevelUp_Display>().UpdateIngredients(materials);
        levelUpPopUp.GetComponent<LevelUp_Display>().GetUpgradeInfo(upgradeGameObject, upgradeType);
    }

    void ClosePopUp()
    {
        levelUpPopUp?.SetActive(false);
    }
    #endregion
}

public enum UpgradeType
{
    CharacterUpgrade,
    CrystalUpgrade,
    FragmentUpgrade,
    SkillUpgrade,
}
