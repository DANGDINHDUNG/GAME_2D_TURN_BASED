using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Hiển thị quái, nhân vật và môi trường khi vào trận.
/// </summary>
public class SpawnHandler : MonoBehaviour
{
    #region Defines
    [SerializeField] public List<GameObject> enemyOnField;
    [SerializeField] public List<GameObject> characterOnField;
    [SerializeField] private GameObject fieldContainer; // Thư mục chứa quái được spawn vào.
    [SerializeField] private GameObject characterContainer; // Thư mục chứ nhân vật được spawn vào.
    [SerializeField] private Vector3[] playerSpawnPoints;   // Thông tin vị trí spawn nhân vật.
    [SerializeField] private Vector3[] enemySpawnPoints;    // Thông tin vị trí spawn quái.
    [SerializeField] private int currentWaveIndex = 0; // Index of the current wave
    [SerializeField] private List<WaveManager> waves;

    public static UnityAction OnClearWave;
    public static UnityAction OnSetupParticipants;
    public static UnityAction OnSetupNextWaves;

    public static SpawnHandler instance;

    //Prefab
    [SerializeField] private GameObject playerIcon;
    [SerializeField] private GameObject actionBar;

    //PlaceHolder để Prefabs spawn vào
    [SerializeField] private GameObject characterPlaceHolder;
    [SerializeField] private GameObject actionBarPlaceHolder;

    private RewardType rewardType;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private List<InventorySlot> levelUpMaterial;
    [SerializeField] private List<InventorySlot> fragmentMaterial;
    [SerializeField] private List<InventorySlot> crystalMaterial;
    [SerializeField] private List<Fragments> fragments;
    [SerializeField] private Button closeRewardPanel;
    private bool isClaimReward;
    #endregion

    #region Core MonoBehaviour
    private void Awake()
    {
        instance = this;
        closeRewardPanel.onClick.AddListener(ClosePanel);
    }

    private void Update()
    {
        if (FieldManager.Instance.InBattle)
        {
            FinishBattle();
            CheckDestroy();
        }
    }
    #endregion

    #region Method
    /// <summary>
    /// Spawn toàn bộ tài nguyên trận đầu bao gồm địa hình, nhân vật, quái vật.
    /// </summary>
    /// <param name="waveManager"></param>
    /// <param name="fieldBackground"></param>
    /// <param name="flag"></param>
    public void LoadWave(List<WaveManager> waveManager, GameObject fieldBackground, bool flag, RewardType type)
    {
        rewardType = type;
        FieldManager.Instance.InBattle = flag;
        isClaimReward = false;
        SpawnPlayer();
        Instantiate(fieldBackground, fieldContainer.transform);
        Instantiate(playerIcon, characterPlaceHolder.transform);
        Instantiate(actionBar, actionBarPlaceHolder.transform);
        waves = waveManager;
        // Kiểm tra nếu còn wave, sẽ spawn quái có trong wave đó.
        if (waves.Count > 0)
        {
            SpawnWave(currentWaveIndex); // Spawn enemies for the first wave
        }
        OnSetupParticipants.Invoke();
    }

    /// <summary>
    /// Spawn nhân vật trong đội hình ra trận đánh. Dữ liệu được lấy từ TeamManager.
    /// </summary>
    public void SpawnPlayer()
    {
        for (int i = 0; i < TeamManager.GetInstance().CharacterInTeam.Count; i++)
        {
            GameObject playerPreb = TeamManager.GetInstance().CharacterInTeam[i];

            // Choose a random spawn point
            Vector3 spawnPoint = playerSpawnPoints[i];
            // Instantiate the player prefab at the chosen spawn point
            var spawnCharacter = Instantiate(playerPreb, spawnPoint, Quaternion.identity, characterContainer.transform);
            spawnCharacter.SetActive(true);
            characterOnField.Add(spawnCharacter);
        }
    }

    /// <summary>
    /// Spam quái ra trận đánh.
    /// </summary>
    /// <param name="waveIndex"></param>
    /// <param name="waves"></param>
    public void SpawnWave(int waveIndex)
    {
        // Lặp spawn quái có trong wave hiện tại.
        for (int i = 0; i < waves[waveIndex].Enemy.Count; i++)
        {
            // Choose a random spawn point (excluding player spawn points)
            Vector3 spawnPoint = enemySpawnPoints[i];
            GameObject enemyPref = waves[waveIndex].Enemy[i];

            // Instantiate the enemy prefab at the chosen spawn point
            enemyOnField.Add(Instantiate(enemyPref, spawnPoint, Quaternion.identity, fieldContainer.transform));         
        }
    }

    /// <summary>
    /// Kiểm tra wave hiện tại đã clear hay chưa. Nếu Clear rồi thì chuyển qua wave tiếp theo.
    /// Nếu không còn wave sẽ kết thúc trận.
    /// </summary>
    private void FinishBattle()
    {
        // Check if all enemies in current wave are defeated
        if (IsWaveComplete())
        {
            // All enemies defeated, move to the next wave (if available)
            if (currentWaveIndex < waves.Count - 1)
            {
                Debug.Log("wave clear");
                currentWaveIndex++;
                SpawnWave(currentWaveIndex);
                OnSetupNextWaves.Invoke();
            }
            else
            {
                // All waves completed, handle end of game logic (optional)
                OnClearWave.Invoke();
                ClearBattleField();
                FieldManager.Instance.DestroySkillPointGauge();
            }
        }
        else if (characterOnField.Count == 0)
        {
            ClearBattleField();
            FieldManager.Instance.DestroySkillPointGauge();
        }
    }

    /// <summary>
    /// Kiểm tra xem đã hạ toàn bộ quái trong wave đó hay không.
    /// </summary>
    /// <param name="waveIndex"></param>
    private bool IsWaveComplete()
    {
        return enemyOnField.Count == 0;
    }

    /// <summary>
    /// Kiểm tra khi một quái vật bị hạ gục, sẽ xóa quái vật đó trong mảng.
    /// </summary>
    private void CheckDestroy()
    {
        for (int i = 0; i < enemyOnField.Count; i++)
        {
            if (enemyOnField[i] == null)
            {
                // Remove the GameObject from the list
                enemyOnField.Remove(enemyOnField[i]);
            }
        }

        for (int i = 0; i < characterOnField.Count; i++)
        {
            if (characterOnField[i] == null)
            {
                // Remove the GameObject from the list
                characterOnField.Remove(characterOnField[i]);
            }
        }
    }

    /// <summary>
    /// Xóa hết các vật thể có trong trận khi kết thúc trận đấu.
    /// </summary>
    private void ClearBattleField()
    {
        StartCoroutine(WaitForClearBattle());
    }

    IEnumerator WaitForClearBattle()
    {
        yield return new WaitForSeconds(1f);
        enemyOnField.Clear();
        characterOnField.Clear();
        currentWaveIndex = 0;
        ShowReward();
        FieldManager.Instance.InBattle = false;
        //Destroy(fieldContainer.GetComponentInChildren<Transform>().gameObject);
        Transform[] children = characterContainer.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.name != characterContainer.name)
            {
                Destroy(child.gameObject);
            }
        }

        Transform[] field = fieldContainer.GetComponentsInChildren<Transform>();

        foreach (Transform child in field)
        {
            if (child.gameObject.name != fieldContainer.name)
            {
                Destroy(child.gameObject);
            }
        }

        Transform[] icon = characterPlaceHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in icon)
        {
            if (child.gameObject.name != characterPlaceHolder.name)
            {
                Destroy(child.gameObject);
            }
        }

        Transform[] bar = actionBarPlaceHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in bar)
        {
            if (child.gameObject.name != actionBarPlaceHolder.name)
            {
                Destroy(child.gameObject);
            }
        }

        characterContainer.transform.position = Vector3.zero;
        characterContainer.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Hiển thị phần thưởng khi hoàn thành trận.
    /// </summary>
    void GenerateReward()
    {
        if (rewardType == RewardType.Fragment)
        {
            for (int i = 0; i < 5; i++)
            {
                GeneraterNewFragment();
            }
        }
        else if (rewardType == RewardType.LevelUpMaterial)
        {
            AddItemToInventory(levelUpMaterial);
        }
        else if (rewardType == RewardType.CrystalMaterial)
        {
            AddItemToInventory(crystalMaterial);
        }
        else if (rewardType == RewardType.FragmentMaterial)
        {
            AddItemToInventory(fragmentMaterial);
        }
    }

    void AddItemToInventory(List<InventorySlot> materials)
    {
        foreach (var slot in materials)
        {
            InventoryManager.instance.AddToInventory(slot.MaterialData, slot.StackSize);
        }
    }

    void GeneraterNewFragment()
    {
        Fragments newFragment = Fragments.GenerateNewFragment();
        TeamManager.GetInstance().obtainedFragments.Add(newFragment);
        fragments.Add(newFragment);
    }

    void ShowReward()
    {
        rewardPanel.SetActive(true);

        if (!isClaimReward)
        {
            GenerateReward();

            if (rewardType == RewardType.Fragment)
            {
                foreach (var fragment in fragments)
                {
                    var frag = Instantiate(rewardPrefab, rewardPanel.transform);
                    frag.GetComponent<Reward_UI>().Init(fragment);
                }
            }
            else if (rewardType == RewardType.LevelUpMaterial)
            {
                foreach (var level in levelUpMaterial)
                {
                    var material = Instantiate(rewardPrefab, rewardPanel.transform);
                    material.GetComponent<Reward_UI>().Init(level);
                }
            }
            else if (rewardType == RewardType.CrystalMaterial)
            {
                foreach (var crys in levelUpMaterial)
                {
                    var material = Instantiate(rewardPrefab, rewardPanel.transform);
                    material.GetComponent<Reward_UI>().Init(crys);
                }
            }
            else if (rewardType == RewardType.FragmentMaterial)
            {
                foreach (var frag in levelUpMaterial)
                {
                    var material = Instantiate(rewardPrefab, rewardPanel.transform);
                    material.GetComponent<Reward_UI>().Init(frag);
                }
            }

            isClaimReward = true;
        }
    }

    void ClosePanel()
    {
        fragments.Clear();

        Transform[] bar = rewardPanel.GetComponentsInChildren<Transform>();

        foreach (Transform child in bar)
        {
            if (child.gameObject.name != rewardPanel.name)
            {
                Destroy(child.gameObject);
            }
        }

        rewardPanel.SetActive(false);
    }
    #endregion
}
