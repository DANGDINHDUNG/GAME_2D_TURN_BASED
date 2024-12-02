using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBattle : CharacterBattle
{
    #region Defines
    [SerializeField] protected PlayerSkills playerSkills;
    [SerializeField] private int skillCost;     // Lượng tiêu tốn điểm kỹ năng khi sử dụng kỹ năng.
    [SerializeField] private int skillPointRecovery;     // Lượng điểm kỹ năng hồi lại khi đánh thường.

    [SerializeField] protected double currentCritRate;
    [SerializeField] protected double currentCritDamage;

    [SerializeField] protected int aggroModifier;       // Bội số khiêu khích của nhân vật (thay đổi bởi ngoại cảnh).
    [SerializeField] protected float currentAggro;     // Độ khiêu khích hiện tại của nhân vật.
    [SerializeField] protected float probabilityOfBeingTargeted;     // Xác suất bị tấn công của nhân vật.
    [SerializeField] protected int currentDamageRESPen;         // Chỉ số xuyên kháng của nhân vật.
    public int currentOutgoingHealing;         // Lượng hồi phục của nhân vật.
    public int currentDefIgnore;         // Chỉ số xuyên thủ của nhân vật.

    private PlayerEquipments playerEquipments;

    [Header("Buffed Value")]
    public int buffedAttack;
    public int buffedSpeed;
    public int buffedDefend;
    public int buffedCrRate;
    public int buffedCrDMG;
    #endregion

    #region Properties
    public double CurrentCritRate => currentCritRate;
    public double CurrentCritDamage => currentCritDamage;
    public float CurrentAggro => currentAggro;
    public float ProbabilityOfBeingTargeted => probabilityOfBeingTargeted;
    public int CurrentDamageRESPen => currentDamageRESPen;
    #endregion

    #region Core MonoBehaviour
    private void Start()
    {
        InitializeStats();
        aggroModifier = 0;
        playerEquipments = GetComponent<PlayerEquipments>();
        playerSkills = GetComponent<PlayerSkills>();
        SetupButtonSkill();
    }

    private void Update()
    {
        InitializeStats();
        currentAggro = ac.AggroCalculation(GetComponent<PlayerBase>().BaseAggro, aggroModifier);
        probabilityOfBeingTargeted = ac.ProbabilityOfBeingTargeted(currentAggro, SpawnHandler.instance.characterOnField);
        StartCoroutine(TargetEnemy());        //Player input for skill selection (if player character)
    
        TriggerCrystalEffect();

        CheckStatusEffectDuration();
        CheckElementStackDuration();

        TriggerStatusEffect();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Khởi tạo giá trị các chỉ số.
    /// </summary>
    private void InitializeStats()
    {
        PlayerDetails pd = GetComponent<PlayerDetails>();

        currentSpeed = pd.characterSpeed + buffedSpeed;
        currentDefend = pd.characterDefend + buffedDefend;
        currentAttack = pd.characterAttack + buffedAttack;
        currentCritRate = pd.characterCrRate + buffedCrRate;
        currentCritDamage = pd.characterCrDMG + buffedCrDMG;
    }

    /// <summary>
    /// Kích hoạt hiệu ứng của Crystal.
    /// </summary>
    void TriggerCrystalEffect()
    {
        if (playerEquipments != null && playerEquipments.crystal.PlayerUsedID == this.GetComponent<Characters>().ID && FieldManager.Instance.TargetEnemy != null)
        {
            playerEquipments.crystal.crystalEffect.Effect(this.gameObject, FieldManager.Instance.TargetEnemy);
        }
    }

    /// <summary>
    /// Hành động của nhân vật khi đến lượt.
    /// </summary>
    /// <param name="manager"></param>
    public override void TakeTurn(BattleHandler manager)
    {
        // ... (use UI elements to capture player choice)
        base.TakeTurn(manager);
    }

    /// <summary>
    /// Kiểm tra nếu người chơi chưa thực hiện hành động sẽ tiến hành đợi.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForUserInput()
    {
        // Khi người chơi chưa chạm vào màn hình để thực hiện hành động.
        while (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
        {
            // Trả về null tức là chưa thực hiện hành động.
            yield return null;
        }
    }

    /// <summary>
    /// Thực hiện hành động tiến công của nhân vật.
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetEnemy()
    {
        // Cờ để kiểm tra nếu người chơi đã ấn một lần để tránh lỗi.
        var hasLogged = false;

        // Lặp cho đến khi người chơi ấn vào quái.
        while (!hasLogged)
        {
            // Kiếm tra nếu người chơi đã ấn vào màn hình mới thực hiện hành động phía dưới.
            yield return StartCoroutine(WaitForUserInput());

            // Check for touch on enemy
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.transform.parent.gameObject.CompareTag("Enemy"))
                {
                    GameObject enemyTargeted = hit.collider.transform.parent.gameObject;
                    FieldManager.Instance.TargetEnemy = enemyTargeted;
                    hasLogged = true;
                }
            }
        }
    }

    /// <summary>
    /// Set up chức năng skill vào button.
    /// </summary>
    private void SetupButtonSkill()
    {
        playerSkills.NormalAttackBtn.onClick.AddListener(ActiveNormalAttack);
        playerSkills.SkillBtn.onClick.AddListener(ActiveSkill);
    }

    /// <summary>
    /// Kích hoạt kĩ năng nhân vật khi ấn nút.
    /// </summary>
    private void ActiveSkill()
    {
        if (FieldManager.Instance.IsPlayerTurn && FieldManager.Instance.CheckEnoughSkillPoint(skillCost))
        {
            isAction = true;
            isUsingSkill = true;
            TriggerCrystalEffect();
            playerSkills.CharacterSkills[1].Attack(this.gameObject, FieldManager.Instance.TargetEnemy, skillCost);
            //isCompleted = true;
            StartCoroutine(ResetAction());
        }
        else
        {
            Notification_UI.instance.ShowNotification("Not enough skill point", 0.5f);
            return;
        }
    }

    /// <summary>
    ///  Kích hoạt tấn công thường khi ấn nút.
    /// </summary>
    private void ActiveNormalAttack()
    {
        if (FieldManager.Instance.IsPlayerTurn)
        {
            isAction = true;
            isUsingSkill = false;
            TriggerCrystalEffect();
            playerSkills.CharacterSkills[0].Attack(this.gameObject, FieldManager.Instance.TargetEnemy, skillPointRecovery);
            isCompleted = true;
            StartCoroutine(ResetAction());
        }
    }

    /// <summary>
    /// Kết thúc một lượt hành động
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetAction()
    {
        yield return new WaitForSeconds(0.05f);
        isAction = false;
    }
    #endregion
}
