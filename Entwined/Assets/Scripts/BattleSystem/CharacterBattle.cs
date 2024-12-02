using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBattle : MonoBehaviour
{
    #region Defines

    #region Chỉ số trong trận của nhân vật
    [Header("Character Information")]
    [SerializeField] protected int currentSpeed;      // Tốc độ hiện tại của nhân vật trong trận.
    [SerializeField] protected int currentAttack;   // Tấn công hiện tại của nhân vật trong trận.
    [SerializeField] protected int currentDefend;   // Phòng thủ hiện tại của nhân vật trong trận.
    [SerializeField] protected List<StatusEffects> activeEffects = new List<StatusEffects>();   // Danh sách buff hiện có của nhân vật trong trận.
    [SerializeField] protected List<ElementStack> activeElements = new List<ElementStack>();   // Danh sách element hiện có của nhân vật trong trận.
    #endregion

    #region Các biến flag 
    [Header("Character Flag In Game")]
    [SerializeField] protected bool isCompleted;  // Nhân vật đã hành động xong.
    public bool isStartTurn;    // Khi nhân vật bắt đầu hiệp.
    public bool isAction;  // Khi nhân vật hành động.
    public bool isUsingSkill;
    [SerializeField] protected int aV;
    [SerializeField] protected ActionValue actionValue;
    [SerializeField] protected ActionCalculation ac;
    #endregion

    #endregion

    #region Properties
    public int Speed => currentSpeed;
    public int Defend => currentDefend;
    public List<StatusEffects> ActiveEffects => activeEffects;
    public List<ElementStack> ActiveElements => activeElements; 

    public int AV
    {
        get { return aV; }
        set { aV = value; }
    }

    public bool IsCompleted
    {
        get { return isCompleted; }
        set { isCompleted = value; }
    }


    public long CurrentAttack => currentAttack;
    #endregion

    #region Core MonoBehaviour
    private void Awake()
    {
        isCompleted = false;
    }

    private void Update()
    {
        CheckStatusEffectDuration();
        CheckElementStackDuration();

    }
    #endregion

    #region Methods
    /// <summary>
    /// Thêm effect nhân vật phải nhận vào list.
    /// </summary>
    /// <param name="effect"></param>
    public virtual void AddEffect(StatusEffects effect)
    {
        effect.ApplyEffect(this.gameObject);
        activeEffects.Add(effect);
    }

    /// <summary>
    /// Gán nguyên tố vào nhân vật.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="stackAmount"></param>
    public virtual void AddElementStack(Element element, int stackAmount)
    {
        // Tìm kiếm loại nguyên tố
        var stack = activeElements.Find(e => e.elementType == element);

        // Nếu nguyên tố chưa được gán, gán nguyên tố mới
        if (stack == null)
        {
            stack = new ElementStack(element);
            activeElements.Add(stack);
        }

        stack.AddStack(stackAmount);
    }

    /// <summary>
    /// Trả về stackCount của loại element tương ứng.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public virtual int GetElementStack(Element element)
    {
        var stack = activeElements.Find(e => e.elementType == element);
        if (stack == null) return 0;
        else return stack.stackCount;
    }

    /// <summary>
    /// Trả về stackCount của loại element tương ứng.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public virtual void SetElementStack(Element element, int stackAmount)
    {
        var stack = activeElements.Find(e => e.elementType == element);
        stack.stackCount = stackAmount;
    }

    /// <summary>
    /// Kiểm tra thời gian hiệu lực của buff
    /// </summary>
    public virtual void CheckStatusEffectDuration()
    {
        if (activeEffects != null && activeEffects.Count > 0)
        {
            foreach (var effect in activeEffects)
            {
                if (isCompleted)
                {
                    // Mỗi lần nhân vật hành động xong, thời gian hiệu lực của buff sẽ giảm
                    effect.DecrementDuration();

                    // Nếu buff hết hiệu lực, sẽ xóa buff đó.
                    if (effect.IsExpired())
                    {
                        effect.RemoveEffect(this.gameObject);
                        activeEffects.Remove(effect);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Kiểm tra thời gian hiệu lực của element.
    /// </summary>
    public virtual void CheckElementStackDuration()
    {
        
        if (activeElements != null && activeElements.Count > 0)
        {
            foreach (var element in activeElements)
            {
                if (isStartTurn)
                {
                    isStartTurn = false;
                    // Mỗi lần nhân vật hành động xong, thời gian hiệu lực của buff sẽ giảm
                    element.DecrementDuration();

                    // Nếu buff hết hiệu lực, sẽ xóa buff đó.
                    if (element.IsExpired())
                    {
                        element.RemoveStack();
                        activeElements.Remove(element);
                    }

                }
            }
        }
    }

    /// <summary>
    /// Kích hoạt hiệu ứng DoT.
    /// </summary>
    public void TriggerStatusEffect()
    {
        foreach (var effect in activeEffects)
        {
            if (effect.statusType == StatusEffects.StatusType.DoT)
            {
                effect.TriggerEffect(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Hành động của nhân vật khi đến lượt.
    /// </summary>
    /// <param name="manager"></param>
    public virtual void TakeTurn(BattleHandler manager)
    {
        isStartTurn = true;
        // ... (use UI elements to capture player choice)
        StartCoroutine(EndTurn(manager));
    }

    /// <summary>
    /// Sau khi nhân vật hành động, chờ một khoảng thời gian để end turn.
    /// Tránh lỗi Stackover flow.
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    private IEnumerator EndTurn(BattleHandler manager)
    {
        while (!isCompleted) yield return null;
        isCompleted = false;
        isStartTurn = false;
        aV = actionValue.StartActionValueCalculator(currentSpeed);
        manager.CharacterActed(); // Character ends turn
    }
    #endregion
}


//LookAtCharacter.GetInstance().FollowCharacter(hit.collider.gameObject.transform);
//Vector3 eulerAngles = LookAtCharacter.GetInstance().GetTransform().rotation.eulerAngles;
//float yRotation = eulerAngles.y;
//LookAtCharacter.GetInstance().SetRotation(new Vector3(0f, yRotation + 9, 0f));
//Destroy(hit.collider.transform.parent.gameObject);
