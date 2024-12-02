using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldInformation : MonoBehaviour
{
    #region Defines
    private Button button;
    [SerializeField] private FieldSO enemySO;
    [SerializeField] private RewardType rewardType;
    public SpawnHandler ParentDisplay { get; private set; }
    #endregion

    #region Method
    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnSpawnCharacter);
        ParentDisplay = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnHandler>();

    }

    public void OnSpawnCharacter()
    {
        if (TeamManager.GetInstance().CharacterInTeam.Count == 4)
        {
            ParentDisplay?.LoadWave(enemySO.waveManagers, enemySO.fieldBackground, true, rewardType);
        }
        else
        {
            Notification_UI.instance.ShowNotification("Require 4 characters in the team to enter the battle", 0.7f);
        }
    }
    #endregion

}

public enum RewardType
{
    Fragment,
    LevelUpMaterial,
    CrystalMaterial,
    FragmentMaterial,
}
