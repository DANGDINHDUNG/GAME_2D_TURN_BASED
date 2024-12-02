using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    #region Defines
    /// <summary>
    /// Người chơi đang ở giao diện Lobby
    /// </summary>
    [SerializeField] private bool isLobby;

    /// <summary>
    /// Người chơi đang ở giao diện chọn màn chơi
    /// </summary>
    [SerializeField] private bool isBattleField;

    /// <summary>
    /// Người chơi đang ở giao diện quản lý nhân vật
    /// </summary>
    [SerializeField] private bool isCharacterManager;

    /// <summary>
    /// Người chơi ở giao diện gacha
    /// </summary>
    [SerializeField] private bool isConvergenceField;

    /// <summary>
    /// Người chơi ở giao diện kết quả gacha
    /// </summary>
    [SerializeField] private bool isGachaResult;

    /// <summary>
    /// Người chơi ở giao diện lập team.
    /// </summary>
    [SerializeField] private bool isTeamSetup;

    /// <summary>
    /// Công dụng tắt bật GameObject.
    /// </summary>
    [SerializeField] private GameObject lobbyGameObject;
    [SerializeField] private GameObject battleFieldGameObject;
    [SerializeField] private GameObject characterManagerGameObject;
    [SerializeField] private GameObject convergenceGameObject;
    [SerializeField] private GameObject gachaResultGameObject;
    [SerializeField] private GameObject teamSetupGameObject;

    public static UIManager instance;
    #endregion

    #region Properties
    /// <summary>
    /// Người chơi đang ở giao diện Lobby
    /// </summary>
    public bool IsLobby { get { return isLobby; } set { isLobby = value; } }

    /// <summary>
    /// Người chơi đang ở giao diện chọn màn chơi
    /// </summary>
    public bool IsBattleField { get {  return isBattleField; } set {  isBattleField = value; } }

    /// <summary>
    /// Người chơi đang ở giao diện quản lý nhân vật
    /// </summary>
    public bool IsCharacterManager { get {  return isCharacterManager; } set { isCharacterManager = value; } }

    /// <summary>
    /// Người chơi đang ở giao diện gacha
    /// </summary>
    public bool IsConvergenceField { get {  return isConvergenceField; } set { isConvergenceField = value; } }

    /// <summary>
    /// Người chơi đang ở giao diện kết quả gacha
    /// </summary>
    public bool IsGachaResult { get {  return isGachaResult; } set { isGachaResult = value; } }

    /// <summary>
    /// Người chơi đang ở giao diện lập team
    /// </summary>
    public bool IsTeamSetup { get {  return isTeamSetup; } set { isTeamSetup = value; } }


    #endregion

    #region UnityAction
    public static UnityAction<List<GameObject>> OnUpdateCharacterList;      // Hiển thị danh sách nhân vật đang được sở hữu.
    public static UnityAction OnRemoveCharacterList;      // Xóa danh sách nhân vật đang được sở hữu.

    public static UnityAction<List<MemoryCrystals>> OnUpdateCrystalList;      // Hiển thị danh sách crystal đang được sở hữu.
    public static UnityAction OnRemoveCrystalList;      // Xóa danh sách crystal đang được sở hữu.

    public static UnityAction OnTransitionDone;     // Hiển thị hiệu ứng gacha.
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        instance = this;
        isLobby = true;
        isBattleField = false;
        isCharacterManager = false;
        isConvergenceField = false;
        isGachaResult = false;
        isTeamSetup = false;
    }

    private void Update()
    {
        if (isLobby) lobbyGameObject.SetActive(true);
        else lobbyGameObject.SetActive(false);

        if (IsBattleField)
        {
            if (FieldManager.Instance.InBattle)
            {
                battleFieldGameObject.SetActive(false);
            }
            else
            {
                battleFieldGameObject.SetActive(true);
            }
        }
        else battleFieldGameObject.SetActive(false);

        if (isCharacterManager) characterManagerGameObject.SetActive(true);
        else characterManagerGameObject.SetActive(false);

        if (isConvergenceField) convergenceGameObject.SetActive(true);
        else convergenceGameObject.SetActive(false);

        if (isGachaResult) gachaResultGameObject.SetActive(true);
        else gachaResultGameObject.SetActive(false);

        if (isTeamSetup) teamSetupGameObject.SetActive(true);
        else teamSetupGameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Thực hiện hành động chuyển màn hình tùy vào tùy chọn của người chơi.
    /// </summary>
    public void SwitchScene(string activedScene)
    {
        if (activedScene == "Lobby")
        {
            isLobby = true;
            isBattleField = false;
            isCharacterManager = false;
            isConvergenceField = false;
            isGachaResult = false;
            isTeamSetup = false;
        }
        else if (activedScene == "BattleField")
        {
            isLobby = false;
            isBattleField = true;
            isCharacterManager = false;
            isConvergenceField = false;
            isGachaResult = false;
            isTeamSetup = false;
        }
        else if (activedScene == "CharacterManager")
        {
            isLobby = false;
            isBattleField = false;
            isCharacterManager = true;
            isConvergenceField = false;
            isGachaResult = false;
            isTeamSetup = false;
        }
        else if (activedScene == "ConvergenceField")
        {
            isConvergenceField = true;
            isLobby = false;
            isBattleField = false;
            isCharacterManager = false;
            isGachaResult = false;
            isTeamSetup = false;
        }
        else if (activedScene == "GachaResult")
        {
            isConvergenceField = true;
            isLobby = false;
            isBattleField = false;
            isCharacterManager = false;
            isGachaResult = true;
            isTeamSetup = false;

        }
        else if (activedScene == "TeamSetup")
        {
            isConvergenceField = false;
            isLobby = false;
            isBattleField = false;
            isCharacterManager = false;
            isGachaResult = false;
            isTeamSetup = true;
        }


    }

    /// <summary>
    /// Thực hiện hành động vào BattleField.
    /// </summary>
    public void IntoBattleField()
    {
        SwitchScene("BattleField");
    }

    /// <summary>
    /// Thực hiện mở màn hình quản lý nhân vật.
    /// </summary>
    public void IntoCharacterListManager()
    {
        SwitchScene("CharacterManager");
        StartCoroutine(WaitForUpdateCharacterList());
    }

    /// <summary>
    /// Thực hiện mở màn hình lập team.
    /// </summary>
    public void IntoTeamSetup()
    {
        SwitchScene("TeamSetup");
        StartCoroutine(WaitForUpdateCharacterList());
    }

    /// <summary>
    /// Chờ một khoảng thời gian để cập nhật danh sách nhân vật.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForUpdateCharacterList()
    {
        yield return new WaitForSeconds(0.1f);
        OnUpdateCharacterList.Invoke(TeamManager.GetInstance().obtainedCharacters);
    }

    /// <summary>
    /// Thực hiện mở màn hình gacha nhân vật.
    /// </summary>
    public void IntoConvergenceField()
    {
        SwitchScene("ConvergenceField");
    }

    /// <summary>
    /// Thực hiện mở màn hình kết quả gacha nhân vật.
    /// </summary>
    public void IntoGachaResult()
    {
        SwitchScene("GachaResult");
        StartCoroutine(WaitForTransition());
    }

    IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(0.01f);
        OnTransitionDone.Invoke();
    }

    /// <summary>
    /// Thực hiện hành động trở về Lobby.
    /// </summary>
    public void ReturnToLobby()
    {
        if (isCharacterManager || isTeamSetup)
        {
            OnRemoveCharacterList.Invoke();
        }

        SwitchScene("Lobby");
    }

    /// <summary>
    /// Thực hiện hành động trở về Lobby.
    /// </summary>
    public void ReturnToConvergence(GameObject holder)
    {
        SwitchScene("ConvergenceField");

        Transform[] character = holder.GetComponentsInChildren<Transform>();

        foreach (Transform child in character)
        {
            if (child.gameObject.name != holder.name)
            {
                Destroy(child.gameObject);
            }
        }
    }



    /// <summary>
    /// Chuyển từ màn hình thông tin chi tiết sang màn hình danh sách nhân vật.
    /// </summary>
    public void ReturnToList(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Mở giao diện danh sách crystal mà người chơi sỡ hữu
    /// </summary>
    /// <param name="gameObject"></param>
    public void OpenCrystalList(GameObject gameObject)
    {

        gameObject.SetActive(true);
        OnUpdateCrystalList.Invoke(TeamManager.GetInstance().obtainedCrystals);

    }

    /// <summary>
    /// Đóng danh sách crystal
    /// </summary>
    /// <param name="gameObject"></param>
    public void CloseCrystalList(GameObject gameObject)
    {
        OnRemoveCrystalList.Invoke();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Đóng popup chi tiết skill
    /// </summary>
    /// <param name="gameObject"></param>
    public void CloseSkillDetail(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }


    #endregion
}
