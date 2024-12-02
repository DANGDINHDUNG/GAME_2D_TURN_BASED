using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleHandler : MonoBehaviour
{
    #region Defines
    [SerializeField] public List<GameObject> participants; // List of all characters (players and enemies)
    [SerializeField] private GameObject currentCharacterTurn; // Reference to UI text for displaying current turn
    [SerializeField] private ActionValue actionValue;
    public static UnityAction<List<GameObject>> OnUpdateActionBar; // UnityAction kích hoạt cập nhật tiến độ Action Bar.
    public static UnityAction<List<GameObject>> OnSetCharacterOnField; // UnityAction kích hoạt hiển thị hình ảnh nhân vật trong trên lên PlaceHolder.
    public static BattleHandler instance;
    #endregion

    #region UnityAction
    private void OnEnable()
    {
        SpawnHandler.OnSetupParticipants += SetupParticipants;
        SpawnHandler.OnSetupNextWaves += SetupNextWaves;
    }

    private void OnDisable()
    {
        SpawnHandler.OnSetupParticipants -= SetupParticipants;
        SpawnHandler.OnSetupNextWaves -= SetupNextWaves;
    }
    #endregion

    #region Core MonoBehaviour
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if(FieldManager.Instance.InBattle)
        {
            CheckDestroy(); // Kiểm tra nếu quái bị tiêu diệt sẽ bị xóa khỏi danh sách.
            OnUpdateActionBar.Invoke(participants); // Liên tục cập nhật thanh Action Bar theo tiến độ trận đấu.
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Thực hiện setup nhân vật và enemy có trong trận vào participants khi mới bắt đầu trận.
    /// </summary>
    /// <returns></returns>
    private void SetupParticipants()
    {
        participants = new List<GameObject>();

        // Add player characters (replace with your character creation logic)
        for (int i = 0; i < TeamManager.GetInstance().CharacterInTeam.Count; i++) {
                participants.Add(SpawnHandler.instance.characterOnField[i]);
        }

        TeamManager.GetInstance().characterInAction = participants.OrderByDescending(c => c.GetComponent<CharacterBase>().Speed).FirstOrDefault();

        // Add enemy characters (replace with your enemy creation logic)
        for (int i = 0; i < SpawnHandler.instance.enemyOnField.Count; i++)
        {
            participants.Add(SpawnHandler.instance.enemyOnField[i]);
        }
        OnSetCharacterOnField.Invoke(SpawnHandler.instance.characterOnField);
        StartCoroutine(WaitForSetup());
    }

    IEnumerator WaitForSetup()
    {
        yield return new WaitForSeconds(0.1f);

        actionValue.SetStartActionValue(participants);
        FieldManager.Instance.ExcuteSkillPointGauge();
        SortTurnByActionValue();
        StartTurn();
    }

    /// <summary>
    /// Thực hiện bỏ enemy vào participants khi bắt đầu waves tiếp theo.
    /// </summary>
    /// <returns></returns>
    private void SetupNextWaves()
    {
        // Add enemy characters (replace with your enemy creation logic)
        for (int i = 0; i < SpawnHandler.instance.enemyOnField.Count; i++)
        {
            participants.Add(SpawnHandler.instance.enemyOnField[i]);
        }
        StartCoroutine(WaitForNextSetUp());

    }

    IEnumerator WaitForNextSetUp()
    {
        yield return new WaitForSeconds(0.1f);

        actionValue.SetStartActionValue(participants);
        SortTurnByActionValue();
        StartTurn();
    }

    void StartTurn()
    {
        SetCurrentCharacterTurn();
        actionValue.SetActionValueCalculator(participants, currentCharacterTurn);
        // Khi mới vô trận sẽ tự động chọn quái đầu tiên để đánh.
        StartCoroutine(NextTurn());
    }

    /// <summary>
    /// Sắp xếp mảng theo thứ tự Speed của từng nhân vật giảm dần.
    /// </summary>
    public void SortTurnByActionValue()
    {
        //participants.Sort((c1, c2) => c1.GetComponent<CharacterBattle>().AV.CompareTo(c2.GetComponent<CharacterBattle>().AV));

        participants.Sort((c1, c2) =>
        {
            if (c1 == null || c2 == null)
                return 0; // Or handle null cases as needed

            var characterBattle1 = c1.GetComponent<CharacterBattle>();
            var characterBattle2 = c2.GetComponent<CharacterBattle>();

            if (characterBattle1 == null || characterBattle2 == null)
                return 0; // Or handle null cases as needed

            return characterBattle1.AV.CompareTo(characterBattle2.AV);
        });
    }

    /// <summary>
    /// Chuyển sang nhân vật hành động kế tiếp trong trận.
    /// </summary>
    IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(0.2f);
        // Allow player input for skill selection (if player character)
        currentCharacterTurn.GetComponent<CharacterBattle>().TakeTurn(this); // Call character's turn logic
    }

    /// <summary>
    /// Hành động tiếp theo sau khi nhân vật đang hành động kết thúc lượt.
    /// </summary>
    public void CharacterActed()
    {
        // Character finished their turn, remove from queue
        //participants.Remove(character);
        if (SpawnHandler.instance.enemyOnField.Count == 0)
        {
            Debug.Log("Win");
        }
        else
        {
            CheckDestroy();
            StartCoroutine(WaitForCheckDestroy());
            // More characters remaining, re-sort queue (optional)
            StartCoroutine(SetNextTurn());
        }
    }

    /// <summary>
    /// Chờ một khoảng thời gian để load nhân vật hành động tiếp theo.
    /// </summary>
    /// <returns></returns>
    IEnumerator NextCharacter()
    {
        yield return new WaitForSeconds(0.1f);
        SetCurrentCharacterTurn();
    }

    /// <summary>
    /// Đợi sau khi Set nhân vật hiện tại xong mới Next Turn để tránh bị lỗi không Set kịp currentCharacter.
    /// </summary>
    /// <returns></returns>
    IEnumerator SetNextTurn()
    {
        yield return StartCoroutine(NextCharacter());
        StartCoroutine(NextTurn());
    }

    /// <summary>
    /// Kiểm tra khi một quái vật bị hạ gục, sẽ xóa quái vật đó trong mảng.
    /// </summary>
    void CheckDestroy()
    {
        for (int i = 0; i < participants.Count; i++)
        {
            if (participants[i] == null)
            {
                // Remove the GameObject from the list
                participants.Remove(participants[i]);
            }
        }
    }

    /// <summary>
    /// Đợi quá trình CheckDestroy xử lý xong mới đặt lại AV.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForCheckDestroy()
    {
        CheckDestroy();
        yield return null;
        actionValue.SetActionValueCalculator(participants);
        SortTurnByActionValue(); // Liên tục cập nhật thứ tự hành động của từng nhân vật theo tiến độ trận đấu.
    }

    /// <summary>
    /// Gán nhân vật đang hành động hiện tại vào currentCharacterTurn.
    /// </summary>
    void SetCurrentCharacterTurn()
    {
        currentCharacterTurn = participants[0];

        if (currentCharacterTurn != null)
        {
            if (currentCharacterTurn.CompareTag("Player") && FieldManager.Instance.InBattle)
            {
                TeamManager.GetInstance().characterInAction = currentCharacterTurn;
                FieldManager.Instance.IsPlayerTurn = true;
            }
            else
            {
                FieldManager.Instance.IsPlayerTurn = false;
            }
        }
    }
    #endregion
}
