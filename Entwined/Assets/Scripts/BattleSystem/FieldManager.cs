using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    #region Defines
    /// <summary>
    /// Quái vật đang bị người chơi nhắm vào trong trận.
    /// </summary>
    [SerializeField] private GameObject targetEnemy; 
    
    /// <summary>
    /// Nhân vật đang được chỉ định để nhận buff.
    /// </summary>
    [SerializeField] private GameObject targetCharacter;  
  
    /// <summary>
    /// Kiểm tra nếu người chơi đang ở trong trận.
    /// </summary>
    [SerializeField] private bool inBattle;     

    /// <summary>
    /// Kiểm tra nếu đang trong lượt của người chơi.
    /// </summary>
    [SerializeField] private bool inPlayerTurn;     

    /// <summary>
    /// Người chơi sử dụng điểm kỹ năng đế có thể sử dụng kỹ năng.
    /// Điểm kỹ năng sẽ được hồi khi người chơi đánh thường.
    /// </summary>
    [SerializeField, Range(0, 10)] private int skillPoint;

    /// <summary>
    /// Giao diện khung chứa điểm kỹ năng của người chơi
    /// </summary>
    [SerializeField] private GameObject skillPoint_Gauge;

    /// <summary>
    /// Giao diện điểm kỹ năng của người chơi.
    /// </summary>
    [SerializeField] private GameObject skillPoint_Image;

    /// <summary>
    /// Danh sách số lượng điểm kỹ năng hiện có trong trận.
    /// </summary>
    [SerializeField] private List<GameObject> skillPoints;

    /// <summary>
    /// Địa điểm để Skill Point Gauge spawn vào.
    /// </summary>
    [SerializeField] private Transform skillPoint_PlaceHolder;

    private GameObject parent;      // SkillPoint_Gauge được Spawn ra, được dùng để lấy child transform.

    public static FieldManager Instance;
    #endregion

    #region Properties
    public GameObject TargetEnemy
    {
        get { return targetEnemy; }
        set { targetEnemy = value; }
    }

    public GameObject TargetCharacter
    {
        get { return targetCharacter; }
        set { targetCharacter = value; }
    }

    public bool InBattle { get { return inBattle; } set { inBattle = value; } }

    public bool IsPlayerTurn { get { return inPlayerTurn; } set { inPlayerTurn = value; } }

    public int SkillPoint { get {  return skillPoint; } set {  skillPoint = value; } }
    #endregion

    #region Core MonoBehaviour
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (FieldManager.Instance.InBattle)
        {
            if (targetEnemy == null)
            {
                StartCoroutine(ExcuteTargetEnemy());
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Đợi một khoảng thời gian để hệ thống xử lý quái được chọn làm mục tiêu tấn công.
    /// </summary>
    /// <returns></returns>
    IEnumerator ExcuteTargetEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        if (SpawnHandler.instance.enemyOnField.Count > 0)
        {
            targetEnemy = SpawnHandler.instance.enemyOnField[0];
        }
    }

    /// <summary>
    /// Hiển thị thanh Skill Point khi vào trận.
    /// </summary>
    public void ExcuteSkillPointGauge()
    {
        skillPoint = 10;
        parent = Instantiate(skillPoint_Gauge, skillPoint_PlaceHolder);
        for (int i = 0; i < skillPoint; i++)
        {
            var point = Instantiate(skillPoint_Image, parent.transform.GetChild(0));
            skillPoints.Add(point);
        }
    }

    /// <summary>
    /// Giảm số lượng điểm kỹ năng hiện có tương ứng với kỹ năng sử dụng.
    /// </summary>
    public void DecreaseSkillPoint(int cost)
    {
        int removedCount = 0;
        for (int i =  skillPoints.Count - 1; i >= 0 && removedCount < cost; i--)
        {
            Destroy(skillPoints[i]);
            skillPoints.Remove(skillPoints[i]);
            removedCount++;
        }
    }

    /// <summary>
    /// Tăng điểm kỹ năng khi nhân vật tấn công thường.
    /// </summary>
    /// <param name="recharge"></param>
    public void IncreaseSkillPoint(int recharge)
    {
        for (int i = 0; i < recharge; i++)
        {
            if (skillPoints.Count < 10)
            {
                var point = Instantiate(skillPoint_Image, parent.transform.GetChild(0));
                skillPoints.Add(point);
            }
        }
    }

    /// <summary>
    /// Kiểm tra xem có đủ skillPoint để sử dụng kĩ năng hay không.
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool CheckEnoughSkillPoint(int cost)
    {
        if (skillPoints.Count >= cost)
        {
            return true;
        }
        else return false;
    }

    public void DestroySkillPointGauge()
    {
        Destroy(parent);
        skillPoints.Clear();
    }
    #endregion
}
